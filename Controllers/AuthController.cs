﻿using HomeStrategiesApi.Helper;
using HomeStrategiesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using projecthomestrategies_api.Helper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace projecthomestrategies_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HomeStrategiesContext _context;
        private readonly JWTSettings _jwtSettings;

        public AuthController(HomeStrategiesContext context, IOptions<JWTSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("signin/{credentials}")]
        public IActionResult SignIn(string credentials)
        {
            if (string.IsNullOrEmpty(credentials))
            {
                return BadRequest("Credentials are missing!");
            }
            else
            {
                var user = GetUserFromBase64Credential(credentials);

                if (user == null)
                    return Unauthorized("E-Mail oder Passwort sind nicht korrekt!");

                Dictionary<string, dynamic> signInResult = new Dictionary<string, dynamic>();
                signInResult.Add("user", user);
                signInResult.Add("token", GenerateToken(user));

                return Ok(signInResult);
            }
        }

        [HttpPost("signup/basic")]
        public IActionResult SignUpBasic([FromBody]RegisterModel registerModel)
        {
            var newUser = new User
            {
                Email = registerModel.Email,
                Password = registerModel.Password,
                Firstname = registerModel.Firstname,
                FcmToken = registerModel.FcmToken,
                Surname = registerModel.Surname,
                Type = UserType.Basic,
            };

            if (CheckIfUserExistsWithMail(newUser.Email))
            {
                return Conflict("Benutzer mit dieser Mail existiert bereits");
            }

            _context.User.Add(newUser);
            _context.SaveChanges();

            var newlyCreatedUser = _context.User.Where(u => u.Email.Equals(registerModel.Email));
            if(newlyCreatedUser == null)
            {
                return BadRequest("New user could not be found!");
            }
            else
            {
                return Created("", newlyCreatedUser);
            }
        }

        [HttpPost("signup/admin")]
        public IActionResult SignUpAdmin([FromBody] RegisterModel registerModel)
        {
            var newUser = new User
            {
                Email = registerModel.Email,
                Password = registerModel.Password,
                Firstname = registerModel.Firstname,
                Surname = registerModel.Surname,
                Type = UserType.Admin,
            };

            _context.User.Add(newUser);
            _context.SaveChanges();

            var newlyCreatedAdmin = _context.User.Where(u => u.Email.Equals(registerModel.Email));
            
            if (newlyCreatedAdmin == null)
            {
                return BadRequest("New user could not be found!");
            }
            else
            {
                return Ok(newlyCreatedAdmin);
            }
        }

        private User GetUserFromBase64Credential(string rawCredentials)
        {
            try
            {
                var bytes = Convert.FromBase64String(rawCredentials);
                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");

                var email = credentials[0];
                var password = credentials[1];

                var user = _context.User
                                .Include(u => u.Household)
                                .Where(u => u.Email.Equals(email) && u.Password.Equals(password))
                                .Select(u => new User
                                {
                                    UserId = u.UserId,
                                    Firstname = u.Firstname,
                                    Surname = u.Surname,
                                    Email = email,
                                    FcmToken = u.FcmToken,
                                    UserColor = u.UserColor,
                                    Household = u.Household,

                                })
                                .FirstOrDefault();

                if (user == null)
                {
                    return null;
                }
                else
                {
                    return user;
                }
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.AppSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.GroupSid, user.Household != null ? user.Household.HouseholdId.ToString() : string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };

            var systemToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(systemToken);
        }

        private bool CheckIfUserExistsWithMail(string email)
        {
            var response = _context.User.Where(u => u.Email.Equals(email)).FirstOrDefault();

            if(response != null)
            {
                return true;
            }
            return false;
        }
    }
}
