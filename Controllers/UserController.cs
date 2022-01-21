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

namespace HomeStrategiesApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private HomeStrategiesContext _context;

        public UserController(ILogger<UserController> logger, HomeStrategiesContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.User);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, bool includeDetails = false)
        {
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
                return NotFound();
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

        //[HttpPost("basic")]
        //[Consumes("application/json")]
        //public IActionResult Post([FromBody]User user){
        //    try
        //    {
        //        user.Type = UserType.Basic;
        //        _context.User.Add(user);
        //        _context.SaveChanges();
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //        throw;
        //    }
        //}

        [HttpPut("{id}")]
        [Consumes("application/json")]
        public IActionResult Put(int id, [FromBody] User newValues)
        {
            var user = _context.User.FirstOrDefault(s => s.UserId == id);
            if (user != null)
            {
                _context.Entry(user).CurrentValues.SetValues(newValues);
                _context.SaveChanges();

                return Ok(_context.User.FirstOrDefault(s => s.UserId == id));
            }
            else
            {
                return NotFound();
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
    }
}
