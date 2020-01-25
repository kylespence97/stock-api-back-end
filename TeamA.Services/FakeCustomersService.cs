using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamA.Models;

namespace TeamA.Services
{
    public class FakeCustomersService
    {
        public static readonly List<CustomerDto> _customers = new List<CustomerDto>
        {
            new CustomerDto {ID = new Guid("3854b6c9-e018-4fb7-8b63-1067384210a9"),
                FirstName = "User",
                LastName = "A",
                Email = "UserA@gmail.com",
                Address = "House A",
                Postcode = "TS1",
                DOB = new DateTime(1997, 11, 20),
                LoggedOnAt = new DateTime(2019, 11, 25, 16, 30, 00),
                PhoneNumber = "123",
                CanPurchase = true
            },
            new CustomerDto {ID = new Guid("ca72dc93-4ffa-48a7-9408-6332907fc9a7"),
                FirstName = "User",
                LastName = "B",
                Email = "UserB@gmail.com",
                Address = "House B",
                Postcode = "TS2",
                DOB = new DateTime(1997, 11, 21),
                LoggedOnAt = new DateTime(2019, 11, 25, 16, 31, 00),
                PhoneNumber = "456",
                CanPurchase = true
            },
            new CustomerDto {ID = new Guid("1ba5067e-2869-40b2-ad5c-e4d6e2ac8958"),
                FirstName = "User",
                LastName = "C",
                Email = "UserC@gmail.com",
                Address = "House C",
                Postcode = "TS3",
                DOB = new DateTime(1997, 11, 22),
                LoggedOnAt = new DateTime(2019, 11, 25, 16, 32, 00),
                PhoneNumber = "789",
                CanPurchase = true
            }
        };

        public static CustomerDto GetCustomerByID(Guid customerID)
        {
            var customer = _customers.Where(c => c.ID == customerID).FirstOrDefault();

            return customer;
        }

        public static CustomerDto SetPurchaseAbilityOfCustomer(Guid customerID, bool canPurchase)
        {
            var customer = _customers.Where(c => c.ID == customerID).FirstOrDefault();

            if (customer != null)
            {
                customer.CanPurchase = canPurchase;
            }

            return customer;
        }
    }
}
