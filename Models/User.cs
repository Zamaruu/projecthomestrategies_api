using HomeStrategiesApi.Helper;
using HomeStrategiesApi.Models.MongoDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HomeStrategiesApi.Models
{
    public enum UserType
    {
        Basic,
        Admin,
    }

    public class User
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FcmToken { get; set; }
        public long UserColor { get; set; }
        public String Image { get; set; }
        public UserType Type { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public Household Household { get; set; }
        public Household AdminOfHousehold { get; set; }
        public List<Notification> Notifications { get; set; }

        public User () { }

        public static User GetUserBasic(HomeStrategiesContext context, int id)
        {
            var user = context.User
                           .Where(u => u.UserId.Equals(id))
                           .Select(u => new User
                           {
                               UserId = u.UserId,
                               Firstname = u.Firstname,
                               Surname = u.Surname,
                               Email = u.Email,
                               UserColor = u.UserColor,
                           })
                           .FirstOrDefault();

            return user;
        }
    }
}
