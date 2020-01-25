using System;
namespace TeamA.Models
{
    public class ResellHistoryDto
    {
        public Guid ID { get; set; }
        public Guid ProductID { get; set; }
        public double ResellPrice { get; set; }
        public DateTime TimeUpdated { get; set; }

    }
}
