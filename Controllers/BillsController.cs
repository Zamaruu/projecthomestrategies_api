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
using HomeStrategiesApi.Controllers;
using HomeStrategiesApi.Auth;
using System.Security.Claims;

namespace HomeStrategiesApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly HomeStrategiesContext _context;
        private readonly INotificationService _notificationService;

        public BillsController(HomeStrategiesContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // GET: api/Bills
        [HttpGet("{householdId}")]
        public async Task<ActionResult<IEnumerable<Bill>>> GetBills(int householdId)
        {
            var bills = await _context.Bills
                            .Include(bl => bl.Category)
                            .Include(bl => bl.Buyer)
                            .Include(bl => bl.Household)
                            //.Include(bl => bl.Images) // IMPORTANT <- when this line is included, the first load time will be significantly higher. Recommended to load images when bill is selected
                            .Where(bls => bls.Household.HouseholdId.Equals(householdId))
                            .ToListAsync();

            var soretedBills = bills.ToList();
            soretedBills.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

            return bills;
        }

        [HttpGet("Paged/{householdId}")]
        public async Task<ActionResult<IEnumerable<Bill>>> GetBillsPaged(int householdId, int pageNumber = 1, int pageSize = 50)
        {
            var bills = await _context.Bills
                            .Include(bl => bl.Category)
                            .Include(bl => bl.Buyer)
                            .Include(bl => bl.Household)
                            .OrderByDescending(x => x.Date)
                            //.Include(bl => bl.Images) // IMPORTANT <- when this line is included, the first load time will be significantly higher. Recommended to load images when bill is selected
                            .Where(bls => bls.Household.HouseholdId.Equals(householdId))
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();


            return bills;
        }

        // GET: api/Bills/5
        [HttpGet("Single/{id}")]
        public async Task<ActionResult<Bill>> GetBill(int id, bool includeImages = false)
        {
            Bill bill;

            if (includeImages)
            {
                bill = await _context.Bills
                    .Include(bl => bl.Category)
                    .Include(bl => bl.Buyer)
                    .Include(bl => bl.Household)
                    .Include(bl => bl.Images) // IMPORTANT <- when this line is included, the first load time will be significantly higher. Recommended to load images when bill is selected
                    .Where(bls => bls.BillId.Equals(id))
                    .FirstAsync();
            }
            else
            {
                bill = await _context.Bills
                    .Include(bl => bl.Category)
                    .Include(bl => bl.Buyer)
                    .Include(bl => bl.Household)
                    .Where(bls => bls.BillId.Equals(id))
                    .FirstAsync();
            }

            if (bill == null)
            {
                return NotFound();
            }

            return bill;
        }

        // PUT: api/Bills/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBill(int id, Bill bill)
        {
            if (id != bill.BillId)
            {
                return BadRequest();
            }

            var newCategory = await _context.BillCategories.FindAsync(bill.Category.BillCategoryId);
            bill.Category = newCategory;

            await HandleEditImage(bill.Images);

            _context.Entry(bill).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var changedBill = await _context.Bills
                                    .Include(b => b.Buyer)
                                    .Include(b => b.Household)
                                    .Include(b => b.Category)
                                    .Include(b => b.Images)
                                    .Where(b => b.BillId.Equals(id))
                                    .FirstOrDefaultAsync();
                return Ok(changedBill);
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
        }

        // POST: api/Bills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostBill(Bill bill)
        {
            try
            {
                //Database operations
                bill.Household = await _context.Households.FindAsync(bill.Household.HouseholdId);
                bill.Buyer = await _context.User.FindAsync(bill.Buyer.UserId);
                bill.Category = await _context.BillCategories.FindAsync(bill.Category.BillCategoryId);
                bill.CreatedAt = DateTime.UtcNow;

                var createBill = new Bill {
                    Amount = bill.Amount,
                    Description = bill.Description,
                    Date = bill.Date,
                    Household = await _context.Households.FindAsync(bill.Household.HouseholdId),
                    Buyer = await _context.User.FindAsync(bill.Buyer.UserId),
                    Images = bill.Images,
                    Category = await _context.BillCategories.FindAsync(bill.Category.BillCategoryId),
                    CreatedAt = DateTime.UtcNow,
                };

                _context.Bills.Add(createBill);
                await _context.SaveChangesAsync();

                //Notification operations
                var notification = new Notification
                {
                    Title = "Neue Rechnung",
                    Content = bill.Buyer.Firstname + " hat eine neu Rechnung über " + createBill.Amount.ToString("0.00") + " € erstellt.",
                    Type = NotificationType.Created,
                    Created = DateTime.UtcNow,
                    CreatorName = await new AuthenticationClaimsHelper(HttpContext.User.Identity as ClaimsIdentity).GetUserNameFromClaims(_context),
                    FirebaseNotificationData = new FirebaseNotificationData
                    {
                        Route = NotificationRoute.Bills,
                    }
                };
      
                var notificationHelper = new NotificationHelper(
                    notification,
                    _context,
                    _notificationService
                );
                notificationHelper.CreateNotificationForHousehold(createBill.Household.HouseholdId, createBill.Buyer.UserId);

                //Returning
                return Ok(createBill);
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
                return NotFound("Die Rechnung konnte nicht gefunden werden!");
            }

            _context.Bills.Remove(bill);
            await _context.SaveChangesAsync();

            return Ok("Rechnung wurde gelöscht.");
        }

        [HttpDelete("Images")]
        public async Task<IActionResult> DeleteBillImages(List<int> imageIds)
        {
            if (imageIds == null || imageIds.Count < 1)
            {
                return BadRequest("Es gab Fehler bei der Übertragung der Ids");
            }

            try
            {
                BillImage deleteImage; 

                foreach (var id in imageIds)
                {
                    deleteImage = await _context.BillImages.FindAsync(id);
                    _context.BillImages.Remove(deleteImage);
                }

                await _context.SaveChangesAsync();
                return Ok("Bilder erfolgreich gelöscht!");
            }
            catch
            {
                return BadRequest("Fehler beim löschen der Bilder!");
            }

        }

        private async Task HandleEditImage(List<BillImage> images)
        {
            foreach (var image in images)
            {
                if (!BillImageExists(image.BillImageId))
                {
                    _context.BillImages.Add(image);
                }
            }

            await _context.SaveChangesAsync();
        }

        private bool BillExists(int id)
        {
            return _context.Bills.Any(e => e.BillId == id);
        }

        private bool BillImageExists(int id)
        {
            return _context.BillImages.Any(e => e.BillImageId == id);
        }
    }
}
