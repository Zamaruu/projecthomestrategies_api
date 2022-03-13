using HomeStrategiesApi.Models.MongoDB;
using HomeStrategiesApi.MongoDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace projecthomestrategies_api.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        readonly MongoServiceClient mongoServiceClient;
        public RecipeController(MongoServiceClient mongoServiceClient)
        {
            this.mongoServiceClient = mongoServiceClient;
        }

        // GET: api/<RecipeController>
        [HttpGet("Household/{householdId}")]
        public IActionResult GetRecipesForHousehold(int householdId)
        {
            if (householdId <= 0)
            {
                return BadRequest("Id kann nicht verarbeitet werden");
            }

            return Ok(mongoServiceClient.GetRecipes(householdId));
        }

        // GET api/<RecipeController>/5
        [HttpGet("{id}")]
        public IActionResult GetSingleRecipe(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id kann nicht verarbeitet werden");
            }

            return Ok(mongoServiceClient.GetRecipe(id));
        }

        [HttpGet("Public")]
        public IActionResult GetPublicRecipes()
        {
            return Ok(mongoServiceClient.GetPublicRecipes());
        }

        // POST api/<RecipeController>
        [HttpPost]
        public IActionResult CreateRecipe([FromBody] Recipe newRecipe)
        {
            if (newRecipe == null)
            {
                return BadRequest("Das Rezept konnte nicht übertragen werden");
            }

            var createdRecipe = mongoServiceClient.CreateNewRecipe(newRecipe);

            return Ok(createdRecipe);
        }

        // PUT api/<RecipeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<RecipeController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRecipe(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id kann nicht verarbeitet werden");
            }

            var result = mongoServiceClient.DeleteRecipe(id);

            if (result.IsAcknowledged)
            {
                return Ok("Rezept gelöscht");
            }

            return BadRequest(result);
        }
    }
}
