using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStrategiesApi.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public float Amount { get; set; }
        public DateTime Date { get; set; }
        public User Buyer { get; set; }
        public Household Household { get; set; }
        public BillCategory Category { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
