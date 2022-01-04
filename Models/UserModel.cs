namespace HomeStrategiesApi.Models
{
    public class UserModel
    {
        public uint id {get; set;}
        public string firstname {get; set;}
        public string surname {get; set;}
        public string email {get; set;}

        public UserModel(){

        }
        public UserModel(uint id, string firstname, string surname, string email){
            this.id = id;
            this.firstname = firstname;
            this.surname = surname;
            this.email = email;
        }

        public UserModel(string firstname, string surname, string email){
            this.firstname = firstname;
            this.surname = surname;
            this.email = email;
        }
    }
}