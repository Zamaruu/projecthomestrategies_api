using System;

namespace HomeStrategiesApi.Models.MongoDB
{
    public class RecipeIngredient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }

        public RecipeIngredient() { }

        /// <summary>
        /// Dieser Konstruktor wird nur aufgerufen wenn ein neuer Kochschritt angelegt werden soll, da das Objekt eine neue GUID bekommt wenn dieser Konstruktor ausgeführt wird!
        /// </summary>
        /// <param name="ingredient">Objekt welches für die Datenbank initialisiert werden soll</param>
        public RecipeIngredient(RecipeIngredient ingredient)
        {
            Id = Guid.NewGuid().ToString();
            Name = ingredient.Name;
            Amount = ingredient.Amount;
        }
    }
}
