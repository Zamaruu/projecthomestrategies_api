using HomeStrategiesApi.Models.MongoDB;

namespace HomeStrategiesApi.Models
{
    public class CompleteRecipe
    {
        public Recipe Recipe { get; set; }
        public User Creator { get; set; }
        public bool IsFavourite { get; set; }
        public CompleteRecipe () { }
        public CompleteRecipe (Recipe r, User c, bool f) {
            Recipe = r;
            Creator = c;
            IsFavourite = f;
        }
    }
}
