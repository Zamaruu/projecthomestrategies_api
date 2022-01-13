using HomeStrategiesApi.Helper;
using HomeStrategiesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeStrategiesApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HouseholdController : ControllerBase
    {
        private HomeStrategiesContext _context;

        public HouseholdController(HomeStrategiesContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _context.Households.FirstOrDefault(h => h.HouseholdId.Equals(id));
            if (result == null) return NotFound();
            else
            {
                result.HouseholdMember = GetHouseholdMember(id);
                return Ok(result);
            }
        }

        [HttpGet("members/{id}")]
        public IActionResult GetHouseholdUsers(int id)
        {
            var result = GetHouseholdMember(id);
            if (result == null) return NotFound();
            else
            {
                return Ok(result);
            }
        }

        [HttpPost]
        [Consumes("application/json")]
        public IActionResult Post([FromBody] Household household)
        {
            try
            {
                _context.Households.Add(household);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
                throw;
            }
        }

        [HttpPost("addUser")]
        [Consumes("application/json")]
        public async Task<IActionResult> AddUserToHousehold(string email, int householdId)
        {
            var user = _context.User.FirstOrDefault(u => u.Email.Equals(email));
            try
            {
                if (user.Household == null)
                {
                    var household = await _context.Households.FindAsync(householdId);
                    if(household != null)
                    {
                        user.Household = household;
                        _context.Entry(user).CurrentValues.SetValues(user);
                        _context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Haushalt wurde nicht gefunden");
                    }
                }
                else
                {
                    return BadRequest("Benutzer ist bereits einem Haushalt zugewiesen");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e);
                throw;
            }
        }

        [HttpPut("{id}")]
        [Consumes("application/json")]
        public IActionResult Put(int id, [FromBody] Household newValues)
        {
            var household = _context.Households.FirstOrDefault(s => s.HouseholdId == id);
            if (household != null)
            {
                _context.Entry(household).CurrentValues.SetValues(newValues);
                _context.SaveChanges();

                return Ok(_context.Households.FirstOrDefault(s => s.HouseholdId == id));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async  Task<IActionResult> Delete(int id)
        {
            var household = _context.Households
                .Include(hshld => hshld.HouseholdMember)
                .Where(hshld => hshld.HouseholdId.Equals(id))
                .FirstOrDefault();

            if (household != null)
            {
                foreach (User user in household.HouseholdMember)
                {
                    await ChangeHouseholdOfUser(user.UserId, 0);
                }
                _context.Households.Remove(household);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("removeUser/{id}")]
        public async Task<IActionResult> RemoveUserFromHousehold(int id)
        {
            var result = await ChangeHouseholdOfUser(id, 0);
            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Fehler beim entfernen des Benutzers");
            }
        }

        //Private Methoden
        private List<User> GetHouseholdMember(int householdId)
        {
            //TODO: nochmal überarbeiten
            var household = _context.Households
                .Include(hshld => hshld.HouseholdMember)
                .Where(hshld => hshld.HouseholdId.Equals(householdId))
                .FirstOrDefault();


            return household.HouseholdMember;
        }

        private async Task<int> ChangeHouseholdOfUser(int userId, int newHouseholdId)
        {
            var user = _context.User.FirstOrDefault(s => s.UserId == userId);

            if (user != null)
            {
                if (newHouseholdId.Equals(0))
                {
                    user.Household = null;
                    _context.Entry(user).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                    return 1;
                }
                else
                {
                    var household = await _context.Households.FindAsync(newHouseholdId);
                    if (household != null)
                    {
                        user.Household = household;
                        _context.Entry(user).CurrentValues.SetValues(user);
                        _context.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else
            {
                return 0;
            }
        }

    }
}
