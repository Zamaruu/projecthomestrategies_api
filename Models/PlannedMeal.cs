using System;

namespace HomeStrategiesApi.Models
{
    public class PlannedMeal
    {
        public int PlannedMealId { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public long Color { get; set; }
        public string BasicRecipeName { get; set; }
        public string RecipeId { get; set; }
        public User Creator { get; set; }
        public Household Household { get; set; }

        public PlannedMeal () { }
    }

    public class FullPlannedMeal
    {
        public int PlannedMealId { get; set; }
        public DateTime StartDay { get; set; }
        public DateTime EndDay { get; set; }
        public long Color { get; set; }
        public string BasicRecipeName { get; set; }
        public CompleteRecipe Recipe { get; set; }
        public User Creator { get; set; }
        public Household Household { get; set; }

        public FullPlannedMeal() { }
    }
}
