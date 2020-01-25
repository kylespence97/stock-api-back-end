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
    //public class LiveCustomersRepository : ICustomersService
    //{
    //    private HttpClient _httpClient;

    //    public LiveCustomersRepository(ILogger<LiveCustomersRepository> logger, HttpClient httpClient)
    //    {
    //        _httpClient = httpClient;
    //    }

    //    public async Task<List<CustomerDto>> GetAllCustomers(/*string token*/)
    //    {
    //        // for integration
    //        //_httpClient.DefaultRequestHeaders.Add("Authorization", token);

    //        using (HttpResponseMessage responseMessage = await _httpClient.GetAsync("api/Accounts/GetCustomers"))
    //        {
    //            if (responseMessage.IsSuccessStatusCode)
    //            {
    //                return await responseMessage.Content.ReadAsAsync<List<CustomerDto>>();
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //    }

    //    public async Task<CustomerDto> GetCustomerByID(Guid customerID)
    //    {
    //        using (HttpResponseMessage responseMessage = await _httpClient.GetAsync("api/Accounts/GetCustomer?accountId=" + customerID))
    //        {
    //            if (responseMessage.IsSuccessStatusCode)
    //            {
    //                return await responseMessage.Content.ReadAsAsync<CustomerDto>();
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //    }

    //    public async Task<CustomerDto> SetPurchaseAbilityOfCustomer(Guid customerID, bool canPurchase)
    //    {
    //        using (HttpResponseMessage responseMessage = await _httpClient.GetAsync("api/Accounts/UpdatePurchaseAbility?accountId=" + customerID + "&purchaseAbility=" + canPurchase))
    //        {
    //            if (responseMessage.IsSuccessStatusCode)
    //            {
    //                return await responseMessage.Content.ReadAsAsync<CustomerDto>();
    //            }
    //            else
    //            {
    //                return null;
    //            }
    //        }
    //    }
    //}
}
