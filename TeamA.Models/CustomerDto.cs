using System;
namespace TeamA.Models
{
    public class CustomerDto
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public DateTime DOB { get; set; }
        public DateTime LoggedOnAt { get; set; }
        public string PhoneNumber { get; set; }
        public bool CanPurchase { get; set; }
    }
}
