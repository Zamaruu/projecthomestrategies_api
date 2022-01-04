using System.Data;
using System.Threading.Tasks;
using HomeStrategiesApi.Models;
using MySql.Data.MySqlClient;

namespace HomeStrategiesApi.Helper
{
    public class MySqlDbClient
    {
        private readonly MySqlConnection SqlConnection;

        public MySqlDbClient(){
            SqlConnection = new HomeStrategiesDBContext().GetSQLConnection();
            SqlConnection.Open();
        }

        public void Dispose()
        {
            SqlConnection.Close();
        }

        //Benutzermethoden
        public async Task<UserModel> GetUserWithID(int id){
            DataSet ds = new DataSet();

            MySqlCommand query = SqlConnection.CreateCommand();
            query.CommandText = @"SELECT idUser, Vorname, Nachname, Email FROM User WHERE (idUser = @id)";
            query.Parameters.AddWithValue("@id", id);

            MySqlDataReader reader = await query.ExecuteReaderAsync() as MySqlDataReader;
            
            UserModel user = new UserModel();

            while (reader.Read()) {
                user.id = reader.GetUInt32("idUser");
                user.firstname = reader.GetString("Vorname");
                user.surname = reader.GetString("Nachname");
                user.email = reader.GetString("Email");
            }

            return user;
        }

        /// <summary>
        /// Erstellt einen neuen Benutzer in der Datenbank
        /// </summary>
        /// <param name="user">Datenmodel des zu erstellenden Benutzer</param>
        /// <returns>Wenn 1 zurückgegeben dann war DB-Operation erfolgreich, bei anderer Zahl nicht!</returns>
        public async Task<int> CreateNewUser(UserModel user){
            MySqlCommand query = SqlConnection.CreateCommand();
            query.CommandText = @"INSERT INTO User(Vorname,Nachname,Email) VALUES (@Vorname,@Nachname,@Email)";
            query.Parameters.AddWithValue("@Vorname", user.firstname);
            query.Parameters.AddWithValue("@Nachname", user.surname);
            query.Parameters.AddWithValue("@Email", user.email);

            return await query.ExecuteNonQueryAsync();
        }
    }
}