namespace HomeStrategiesApi.MongoDB
{
    public class MongoConnectionSettings : IMongoConnectionSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IMongoConnectionSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
