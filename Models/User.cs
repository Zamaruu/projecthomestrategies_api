﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int UserColor { get; set; }
        public UserType Type { get; set; }
        
        public Household Household { get; set; }
    }
}
