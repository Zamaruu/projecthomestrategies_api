using System;

namespace HomeStrategiesApi.Models.MongoDB
{
    public class CoockingStep
    {
        public string Id { get; set; }
        public int StepNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public CoockingStep () { }

        /// <summary>
        /// Dieser Konstruktor wird nur aufgerufen wenn ein neuer Kochschritt angelegt werden soll, da das Objekt eine neue GUID bekommt wenn dieser Konstruktor ausgeführt wird!
        /// </summary>
        /// <param name="newCookingStep">Objekt welches für die Datenbank initialisiert werden soll</param>
        public CoockingStep (CoockingStep newCookingStep) {
            Id = Guid.NewGuid().ToString();
            StepNumber = newCookingStep.StepNumber;
            Title = newCookingStep.Title;
            Description = newCookingStep.Description;
        }
    }
}
