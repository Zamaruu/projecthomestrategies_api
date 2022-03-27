using HomeStrategiesApi.Models.MongoDB;
using HomeStrategiesApi.MongoDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HomeStrategiesApi.Models;
using HomeStrategiesApi.Helper;
using System.Security.Claims;
using HomeStrategiesApi.Auth;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeStrategiesApi.Controllers
{
    [Authorize]
    //[AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly MongoServiceClient mongoServiceClient;
        private readonly HomeStrategiesContext _context;

        public RecipeController(MongoServiceClient mongoServiceClient, HomeStrategiesContext context)
        {
            this.mongoServiceClient = mongoServiceClient;
            _context = context;
        }

        // GET: api/<RecipeController>
        [HttpGet("Household/{householdId}")]
        public IActionResult GetRecipesForHousehold(int householdId)
        {
            if (householdId <= 0)
            {
                return BadRequest("Id kann nicht verarbeitet werden");
            }
            var recipes = mongoServiceClient.GetRecipes(householdId);
                
            return Ok(GetCompleteRecipesBasic(recipes));
        }

        // GET api/<RecipeController>/5
        [HttpGet("{id}")]
        public IActionResult GetSingleRecipe(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest("Id kann nicht verarbeitet werden");
            }

            var rawRecipe = mongoServiceClient.GetRecipe(id);
            var creator =  Models.User.GetUserBasic(_context, rawRecipe.CreatorId);
            var isFavourite = IsRecipeFavourite(id);

            if (rawRecipe == null || creator == null)
            {
                return BadRequest("Es kam zu einem Fehler beim lesen des Rezeptes!");
            }

            var recipe = new CompleteRecipe(rawRecipe, creator, isFavourite);

            return Ok(recipe);
        }

        [HttpGet("Public")]
        public IActionResult GetPublicRecipes(int pageNumber = 1, int pageSize = 25)
        {
            var recipes = mongoServiceClient.GetPublicRecipes(pageNumber, pageSize);

            return Ok(GetCompleteRecipesBasic(recipes));
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

        // POST api/Recipe/Favourite/id
        [HttpPost("Favourites")]
        public async Task<IActionResult> SetFavouriteRecipe(string recipeId)
        {
            if (string.IsNullOrWhiteSpace(recipeId))
            {
                return BadRequest("Es gab einen Fehler bei der Übertragung");
            }

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = new AuthenticationClaimsHelper(identity).GetIdClaimFromUser();
            try
            {
                var user = await _context.User.FindAsync(userId);
                _context.RecipeFavourites.Add(new RecipeFavourite
                {
                    User = user,
                    RecipeId = recipeId
                });
                await _context.SaveChangesAsync();

                return Ok("Favorit wurde erfolgreich gespeichert!");
            }
            catch
            {
                return BadRequest("Favorit konnte nicht gespeichert werden!");
            }

        }

        [HttpDelete("Favourites")]
        public async Task<IActionResult> DeleteFavouriteRecipe(string recipeId)
        {
            if (string.IsNullOrWhiteSpace(recipeId))
            {
                return BadRequest("Es gab einen Fehler bei der Übertragung");
            }

            try
            {
                var userId = GetIdOfSessionUser();
                var deleteBell = _context.RecipeFavourites
                                            .Where(fav => fav.RecipeId.Equals(recipeId) && fav.User.UserId.Equals(userId))
                                            .FirstOrDefault();
                
                _context.RecipeFavourites.Remove(deleteBell);
                await _context.SaveChangesAsync();

                return Ok("Favorit wurde erfolgreich gelöscht");
            }
            catch 
            {
                return BadRequest("Favorit konnte nicht entfernt werden!");
            }
        }

        [HttpGet("Favourites")]
        public IActionResult GetFavourites()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = new AuthenticationClaimsHelper(identity).GetIdClaimFromUser();

            if (userId < 1)
            {
                return BadRequest("Das Rezept konnte nicht übertragen werden");
            }

            try
            {
                var rawFavs = _context.RecipeFavourites
                                        .Where(fav => fav.User.UserId.Equals(userId))
                                        .ToList();

                var favourites = new List<CompleteRecipe>();

                foreach (var fav in rawFavs)
                {
                    favourites.Add(GetCompleteRecipeBasic(fav.RecipeId));
                }

                return Ok(favourites);
            }
            catch
            {
                return BadRequest("Favoriten konnten nicht abgerufen werden!");
            }
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

        private bool IsRecipeFavourite(string recipeId)
        {
            var userId = GetIdOfSessionUser();
            return _context.RecipeFavourites.Any(f => f.RecipeId.Equals(recipeId) && f.User.UserId.Equals(userId));
        }

        private CompleteRecipe GetCompleteRecipeBasic(string id)
        {
            var recipe = mongoServiceClient.GetRecipe(id);
            var creator = Models.User.GetUserBasic(_context, recipe.CreatorId);
            var isFavourite = IsRecipeFavourite(id);

            var basicRecipe = new Recipe
            {
                Id = id,
                CreatedAt = recipe.CreatedAt,
                DisplayImage = recipe.DisplayImage,
                Name = recipe.Name,
                CreatorId = recipe.CreatorId,
                CookingTime = recipe.CookingTime,
            };

            return new CompleteRecipe(basicRecipe, creator, isFavourite);
        }

        private List<CompleteRecipe> GetCompleteRecipesBasic(List<Recipe> recipes)
        {
            var basicRecipes = new List<CompleteRecipe>();

            Recipe basicRecipeTemp;
            User creatorTemp;
            bool isFavourite;

            foreach (var r in recipes)
            {
                basicRecipeTemp = new Recipe
                {
                    Id = r.Id,
                    CreatedAt = r.CreatedAt,
                    DisplayImage = r.DisplayImage,
                    Name = r.Name,
                    CreatorId = r.CreatorId,
                    CookingTime = r.CookingTime,
                };
                creatorTemp = Models.User.GetUserBasic(_context, r.CreatorId);
                isFavourite = IsRecipeFavourite(r.Id);

                basicRecipes.Add(new CompleteRecipe(basicRecipeTemp, creatorTemp, isFavourite));
            }

            return basicRecipes;
        }

        private int GetIdOfSessionUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            return new AuthenticationClaimsHelper(identity).GetIdClaimFromUser();
        }
    }
}
