using System;
using System.Collections.Generic;

namespace HomeStrategiesApi.Models.MongoDB
{
    public class Recipe
    {
        public string Id { get; set; }
        public int HouseholdId { get; set; }
        public int CreatorId { get; set; }
        public string Name { get; set; }
        public string Desctiption { get; set; }
        public string DisplayImage { get; set; }
        public int CookingTime { get; set; }
        public bool MakePublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CoockingStep> CookingSteps { get; set; }

        public Recipe() { }

        /// <summary>
        /// Dieser Konstruktor wird nur aufgerufen wenn ein neues Rezept angelegt werden soll, da das Objekt eine neue GUID bekommt wenn dieser Konstruktor ausgeführt wird!
        /// </summary>
        /// <param name="newRecipe">Objekt welches für die Datenbank initialisiert werden soll</param>
        public Recipe(Recipe newRecipe) {
            Id = Guid.NewGuid().ToString();
            HouseholdId = newRecipe.HouseholdId;
            CreatorId = newRecipe.CreatorId;
            Name = newRecipe.Name;
            Desctiption = newRecipe.Desctiption;
            DisplayImage = newRecipe.DisplayImage;
            CookingTime = newRecipe.CookingTime;
            MakePublic = newRecipe.MakePublic;
            CreatedAt = DateTime.UtcNow;
            CookingSteps = InitalizeCookingSteps(newRecipe.CookingSteps);
        }

        private static List<CoockingStep> InitalizeCookingSteps(List<CoockingStep> newSteps)
        {
            List<CoockingStep> steps = new();

            foreach (var step in newSteps)
            {
                steps.Add(new CoockingStep(step));
            }

            return steps;
        }
    }
}
