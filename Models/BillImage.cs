using System.ComponentModel.DataAnnotations.Schema;

namespace HomeStrategiesApi.Models
{
    public class BillImage
    {
        public int BillImageId { get; set; }
        //Datentyp ist zu klein für bild...eventuell im frontend komprimieren
        public byte[] Image { get; set; }

        [Column(TypeName = "MEDIUMBLOB")]
        public Bill Bill { get; set; }
    }
}
