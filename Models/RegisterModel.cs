using System.ComponentModel.DataAnnotations;

namespace HomeStrategiesApi.Models
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }
    }
}
