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

namespace projecthomestrategies_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly HomeStrategiesContext _context;

        public BillsController(HomeStrategiesContext context)
        {
            _context = context;
        }

        // GET: api/Bills
        [HttpGet("{householdId}")]
        public async Task<ActionResult<IEnumerable<Bill>>> GetBills(int householdId)
        {
            var bills = await _context.Bills
                            .Include(bl => bl.Category)
                            .Include(bl => bl.Buyer)
                            .Include(bl => bl.Household)
                            .Where(bls => bls.Household.HouseholdId.Equals(householdId))
                            .ToListAsync();

            var soretedBills = bills.ToList();
            soretedBills.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

            return bills;
        }

        // GET: api/Bills/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Bill>> GetBill(int id)
        //{
        //    var bill = await _context.Bills.FindAsync(id);

        //    if (bill == null)
        //    {
        //        return NotFound();
        //    }

        //    return bill;
        //}

        // PUT: api/Bills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill(int id, Bill bill)
        {
            if (id != bill.BillId)
            {
                return BadRequest();
            }

            _context.Entry(bill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Bills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBill(Bill bill)
        {
            try
            {
                bill.Household = await _context.Households.FindAsync(bill.Household.HouseholdId);
                bill.Buyer = await _context.User.FindAsync(bill.Buyer.UserId);
                bill.Category = await _context.BillCategories.FindAsync(bill.Category.BillCategoryId);
                _context.Bills.Add(bill);
                await _context.SaveChangesAsync();

                return Ok(bill);
            }
            catch (Exception)
            {
                return StatusCode(500, "Es ist ein Serverfehler beim erstellen der Rechnung aufgetreten!");
            }

        }

        // DELETE: api/Bills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var bill = await _context.Bills.FindAsync(id);
            if (bill == null)
            {
                return NotFound();
            }

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.BillId == id);
        }
    }
}
