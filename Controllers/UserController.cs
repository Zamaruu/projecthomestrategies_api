using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using HomeStrategiesApi.Models;
using HomeStrategiesApi.Helper;
using System.Threading.Tasks;
using System.Data;

namespace HomeStrategiesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public UserController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public String Get()
        {

            return "normaler Benutzer";
        }

        [HttpGet("{id}")]
        public async Task<UserModel> Get(int id)
        {   
            MySqlDbClient client = new MySqlDbClient();
            return await client.GetUserWithID(id);
        }

        [HttpPost("create")]
        [Consumes("application/json")]
        public async Task<IActionResult> Post([FromBody]UserModel user){
            MySqlDbClient client = new MySqlDbClient();
            int result = await client.CreateNewUser(user);
            
            if(result == 1){
                return Ok(result);
            }
            else{
                return BadRequest(result);
            }
        }
    }
}
