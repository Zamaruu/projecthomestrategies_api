using HomeStrategiesApi.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HomeStrategiesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private HomeStrategiesContext _context;

        public BillingController(HomeStrategiesContext context)
        {
            _context = context;
        }

        [HttpGet("{householdId}")]
        public IActionResult Get(int householdId)
        {
            var result = _context.Bills.Where(h => h.Household.HouseholdId.Equals(householdId)).ToList();
            if (result.Count == 0) return NotFound();
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("single/{billid}")]
        public IActionResult GetSingleBill(int billid)
        {
            var result = _context.Bills.Where(h => h.BillId.Equals(billid)).ToList();
            if (result != null) return NotFound();
            else
            {
                return Ok(result);
            }
        }
    }
}
