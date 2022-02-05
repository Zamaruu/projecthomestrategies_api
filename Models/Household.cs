using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStrategiesApi.Models
{
    public class Household
    {
        public int HouseholdId { get; set; }
        public string HouseholdName { get; set; }
        public int AdminId { get; set; }
        public DateTime CreatedAt { get; set; }
        public User HouseholdCreator { get; set; }
        public List<User> HouseholdMember { get; set; }
        public List<Bill> HouseholdBills { get; set; }
        public List<BillCategory> HouseholdBillCategories { get; set; }
    }
}
