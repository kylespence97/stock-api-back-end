using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TeamA.Models;
using Microsoft.Extensions.Logging;

namespace TeamA.Services
{
    public class CustomersRepository : ICustomersService
    {
        private readonly List<CustomerDto> _customers;
        private readonly ILogger<CustomersRepository> _logger;

        public CustomersRepository(ILogger<CustomersRepository> logger)
        {
            _customers = FakeCustomersService._customers;
            _logger = logger;
        }

        public async Task<List<CustomerDto>> GetAllCustomers(/*string token*/)
        {
            try
            {
                _logger.LogInformation("Database call: Getting all customers");

                var customers = _customers;

                _logger.LogInformation($"Database call: Returning {customers.Count} customers");

                return await Task.FromResult(customers);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<CustomerDto> GetCustomerByID(Guid customerID)
        {
            try
            {
                _logger.LogInformation($"Database call: Getting customer with ID {customerID}");

                var customer = _customers.Where(c => c.ID.Equals(customerID)).FirstOrDefault();

                _logger.LogInformation($"Database call: Returning customer with ID {customer.ID}");

                return await Task.FromResult(customer);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<CustomerDto> SetPurchaseAbilityOfCustomer(Guid customerID, bool canPurchase)
        {
            try
            {
                _logger.LogInformation($"Database call: Getting customer with ID {customerID}");

                var customer = _customers.Where(c => c.ID == customerID).FirstOrDefault();

                if (customer != null)
                {
                    _logger.LogInformation($"Database call: Setting puchase ability of customer with ID {customer.ID} to {canPurchase}");

                    customer.CanPurchase = canPurchase;
                }

                _logger.LogInformation($"Database call: Returning customer with ID {customer.ID} and new purchase ability {customer.CanPurchase}");

                return await Task.FromResult(customer);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }
    }
}
