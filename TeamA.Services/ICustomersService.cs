using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamA.Models;

namespace TeamA.Services
{
    public interface ICustomersService
    {
        Task<List<CustomerDto>> GetAllCustomers(/*string token*/);
        Task<CustomerDto> GetCustomerByID(Guid id);
        Task<CustomerDto> SetPurchaseAbilityOfCustomer(Guid customerID, bool canPurchase);
    }
}
