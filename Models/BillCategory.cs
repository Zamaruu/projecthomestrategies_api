using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStrategiesApi.Models
{
    public class BillCategory
    {
        public int BillCategoryId { get; set; }
        public string BillCategoryName { get; set; }
        public Household Household { get; set; }
    }
}
