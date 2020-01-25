using System;
namespace TeamA.Models
{
    public class StockDto
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; } // won't need set after data initialisation
        public int StockLevel { get; set; }
        public double ResellPrice { get; set; }
    }
}
