using System;
namespace TeamA.Models
{
    public class StockLevelDto
    {
        public Guid ProductID { get; set; } // won't need set after data initialisation
        public int StockLevel { get; set; }
    }
}
