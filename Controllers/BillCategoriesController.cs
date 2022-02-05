using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeStrategiesApi.Helper;
using HomeStrategiesApi.Models;
using Microsoft.AspNetCore.Authorization;
using HomeStrategiesApi.Auth;
using System.Security.Claims;

namespace projecthomestrategies_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillCategoriesController : ControllerBase
    {
        private readonly HomeStrategiesContext _context;

        public BillCategoriesController(HomeStrategiesContext context)
        {
            _context = context;
        }

        // GET: api/BillCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BillCategory>>> GetBillCategories()
        {
            return await _context.BillCategories.ToListAsync();
        }

        // GET: api/BillCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BillCategory>> GetBillCategory(int id)
        {
            var billCategory = await _context.BillCategories.FindAsync(id);

            if (billCategory == null)
            {
                return NotFound();
            }

            return billCategory;
        }

        [HttpGet("ForHousehold/{householdId}")]
        public IActionResult GetBillCategoryForHousehold(int householdId)
        {
            var billCategories =  _context.BillCategories
                                    .Where(bc => bc.Household.HouseholdId.Equals(householdId))
                                    .ToList();

            if (billCategories == null)
            {
                return NotFound();
            }

            return Ok(billCategories);
        }

        // PUT: api/BillCategories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBillCategory(int id, BillCategory billCategory)
        {
            if (id != billCategory.BillCategoryId)
            {
                return BadRequest();
            }

            _context.Entry(billCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(billCategory);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/BillCategories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBillCategory(BillCategory billCategory)
        {
            try
            {
                var household = await _context.Households.FindAsync(billCategory.Household.HouseholdId);
                var newCategory = new BillCategory
                {
                    BillCategoryName = billCategory.BillCategoryName,
                    Household = household,
                    CreatedAt = DateTime.UtcNow,
                };

                var authHelper = new AuthenticationClaimsHelper(HttpContext.User.Identity as ClaimsIdentity);
                if (!authHelper.IsAuthenticatedForHousehold(billCategory.Household.HouseholdId))
                {
                    return Unauthorized("Benutzer ist nicht autorisiert für einen anderen Haushalt Kategorien anzulegen!");
                }

                _context.BillCategories.Add(newCategory);
                await _context.SaveChangesAsync();

                return Ok(newCategory);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.ToString());
                //return StatusCode(500, "Ein Fehler bei der Anfragebearbeitung ist aufgetreten!");
            }

        }

        // DELETE: api/BillCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBillCategory(int id)
        {
            var bills = _context.Bills.Where(b => b.Category.BillCategoryId.Equals(id));
                
            if(bills.Any())
            {
                return BadRequest("Kategorie kann nicht gelöscht werden da sie noch in Rechnungen verwendet wird.");
            }

            var billCategory = _context.BillCategories.Find(id);
            if (billCategory == null)
            {
                return NotFound("Die zu löschende Kategorie wurde nicht gefunden");
            }

            _context.BillCategories.Remove(billCategory);
            await _context.SaveChangesAsync();

            return Ok("Kategorie wurde gelöscht");
        }

        private bool BillCategoryExists(int id)
        {
            return _context.BillCategories.Any(e => e.BillCategoryId == id);
        }
    }
}
