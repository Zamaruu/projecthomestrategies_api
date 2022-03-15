using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStrategiesApi.Models.MongoDB
{
    public class RecipeFavourite
    {
        public int RecipeFavouriteId { get; set; }
        public string RecipeId { get; set; }
        public User User { get; set; }

        public RecipeFavourite() { }
    }
}
