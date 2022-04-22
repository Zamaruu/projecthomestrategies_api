using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using HomeStrategiesApi.Models;
using HomeStrategiesApi.Helper;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using HomeStrategiesApi.Auth;
using System.ComponentModel.DataAnnotations;

namespace HomeStrategiesApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly HomeStrategiesContext _context;

        public UserController(ILogger<UserController> logger, HomeStrategiesContext context)
        {
            _logger = logger;
            _context = context;
        }

        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        [HttpGet("All")]
        public IActionResult GetAllUsers()
        {
            return Ok(_context.User);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool includeDetails = false)
        {
            var authorized = IsCallAuthorized(checkUserId: id);
            User result;
            if (includeDetails)
            {
                result = _context.User
                    .Include(u => u.Household)
                    .Where(u => u.UserId.Equals(id))
                    .FirstOrDefault();
            }
            else
            {
                result = await _context.User.FindAsync(id);
            }
            
            if(result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("ForHousehold/{email}")]
        public IActionResult GetUserForHousehold(string email)
        {
            User result = _context.User
                    .Include(u => u.Household)
                    .Where(u => u.Email.Equals(email))
                    .Select(u => new User
                    {
                        UserId = u.UserId,
                        Firstname = u.Firstname,
                        Surname = u.Surname,
                        Email = u.Email,
                        UserColor = u.UserColor,
                        Household = u.Household,
                    })
                    .FirstOrDefault();

            if (result == null)
            {
                return NotFound("Unbekannter Benutzer mit E-Mail: " + email);
            }
            else
            {
                if(result.Household == null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Benutzer ist bereits in einem Haushalt");
                }
            }
        }

        [HttpPut("Color")]
        [Consumes("application/json")]
        public IActionResult Put([Required] int userId, [Required] long color)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var jwtId = new AuthenticationClaimsHelper(identity).GetIdClaimFromUser();

            if(!userId.Equals(jwtId))
            {
                return Unauthorized("Sie können nur Ihre eigenen Daten bearbeiten!");
            }

            var user = _context.User.FirstOrDefault(s => s.UserId == userId);
            if (user != null)
            {
                user.UserColor = color;
                _context.Entry(user).CurrentValues.SetValues(user);
                _context.SaveChanges();

                return Ok(_context.User.FirstOrDefault(s => s.UserId == userId));
            }
            else
            {
                return NotFound("Benutzer mit der ID konnte nicht gefunden werden!");
            }
        }

        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var user = _context.User.FirstOrDefault(s => s.UserId == id);
            if (user != null)
            {
                _context.User.Remove(user);
                _context.SaveChanges();
            }
        }

        private bool IsCallAuthorized(int checkUserId = 0, int checkHouseholdId = 0)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier).Value;
                var householdIdClaim = identity.FindFirst(ClaimTypes.GroupSid).Value;

                var userID = int.Parse(userIdClaim);
                var householdId = string.IsNullOrEmpty(householdIdClaim)? 0: int.Parse(householdIdClaim);

                if(userID == checkUserId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
