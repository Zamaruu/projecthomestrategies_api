using HomeStrategiesApi.Models.MongoDB;
using MongoDB.Driver;
using System.Collections.Generic;

namespace HomeStrategiesApi.MongoDB
{
    enum Collections
    {
        Recipes,
    }

    public class MongoServiceClient
    {
        readonly MongoClient mongoClient;
        readonly IMongoDatabase database;
        
        public MongoServiceClient(IMongoConnectionSettings settings)
        {
            mongoClient = new MongoClient(settings.ConnectionString);
            database = mongoClient.GetDatabase(settings.DatabaseName);
        }

        private IMongoCollection<T> GetCollection<T>(Collections collection) where T : new()
        {
            switch (collection)
            {
                case Collections.Recipes:
                    var coll = database.GetCollection<Recipe>(collection.ToString());
                    return (IMongoCollection<T>)coll;
                default:
                    return null;
            }
        }

        // ------------------------------------------------------------------------------------
        // Rezepte

        public List<Recipe> GetPublicRecipes()
        {
            var collection = GetCollection<Recipe>(Collections.Recipes);
            return collection.Find(recipe => recipe.MakePublic).ToList();
        }

        public Recipe GetRecipe(string id)
        {
            var collection = GetCollection<Recipe>(Collections.Recipes);
            return collection.Find(recipe => recipe.Id.Equals(id)).FirstOrDefault();
        }

        public List<Recipe> GetRecipes(int householdId)
        {
            var collection = GetCollection<Recipe>(Collections.Recipes);
            return collection.Find(recipe => recipe.HouseholdId.Equals(householdId)).ToList();
        }

        public Recipe CreateNewRecipe(Recipe newRecipe)
        {
            // Initialize Recipe with new GUID
            var recipe = new Recipe(newRecipe);

            var collection = GetCollection<Recipe>(Collections.Recipes);
            collection.InsertOne(recipe);

            return recipe;
        }

        public DeleteResult DeleteRecipe(string id)
        {
            var collection = GetCollection<Recipe>(Collections.Recipes);
            return collection.DeleteOne(id);
        }

        // ------------------------------------------------------------------------------------
    }
}
