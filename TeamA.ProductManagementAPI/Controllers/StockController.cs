using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamA.Data;
using TeamA.Models;
using TeamA.Services;
using Polly;
using Microsoft.Extensions.Logging;

namespace TeamA.ProductManagementAPI.Controllers
{
    [Route("")]
    [EnableCors("AllowAll")]
    [ApiController]
    [Authorize(Policy = "Staff")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly ICustomersService _customersService;
        private readonly ILogger<StockController> _logger;

        public StockController(IStockService stockService, ICustomersService customersService, ILogger<StockController> logger)
        {
            _stockService = stockService;
            _customersService = customersService;
            _logger = logger;
        }

        #region Stock Methods (Internal)

        // GET: api/Stock/GetAllStock
        //e.g. https://localhost:44305/api/Stock/GetAllStock
        [HttpGet("api/Stock/GetAllStock")]
        public async Task<IActionResult> GetAllStock()
        {
            try
            {
                _logger.LogInformation("Attempting to get all stock");

                var stock = await Policy
                            .Handle<Exception>()
                            .RetryAsync(3)
                            .ExecuteAsync(async () => await _stockService.GetAllStock())
                            .ConfigureAwait(false);

                _logger.LogInformation($"Returning {stock.Count} stock");

                return Ok(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET: api/Stock/GetStockByProductID
        //e.g. https://localhost:44305/api/Stock/GetStockByProductID?productID=397945de-54c3-413f-9b6a-5ad187069fd9
        [HttpGet("api/Stock/GetStockByProductID")]
        public async Task<IActionResult> GetStockByProductID(Guid productID)
        {
            try
            {
                _logger.LogInformation($"Attempting to get stock with product ID {productID}");

                var stock = await Policy
                        .Handle<Exception>()
                        .RetryAsync(3)
                        .ExecuteAsync(async () => await _stockService.GetStockByProductID(productID))
                        .ConfigureAwait(false);

                if (stock == null)
                {
                    _logger.LogInformation($"Stock with product ID {productID} could not be found");
                    return NotFound();
                }

                _logger.LogInformation($"Returning stock with product ID {stock.ProductID}");
                return Ok(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500);
            }
        }

        // GET: api/Stock/GetStockByStockLevel
        //e.g. https://localhost:44305/api/Stock/GetStockByStockLevel?stockLevel=50
        [HttpGet("api/Stock/GetStockByStockLevel")]
        public async Task<IActionResult> GetStockByStockLevel(int stockLevel)
        {
            try
            {
                if (stockLevel < 0)
                {
                    _logger.LogInformation($"{stockLevel} is an invalid value, stock level cannot be less than 0");
                    return BadRequest();
                }

                _logger.LogInformation($"Attempting to get stock filtered by stock level {stockLevel}");

                var stock = await Policy
                        .Handle<Exception>()
                        .RetryAsync(3)
                        .ExecuteAsync(async () => await _stockService.GetStockByStockLevel(stockLevel))
                        .ConfigureAwait(false);

                _logger.LogInformation($"Returning stock filtered by stock level {stockLevel}");
                return Ok(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500);
            }
        }

        // GET: api/Stock/GetResellPriceOfStock
        // e.g. https://localhost:44305/api/Stock/GetResellPriceOfStock?productID=fd03e5c1-551a-4eee-9922-fd695c63b4d9
        [HttpGet("api/Stock/GetResellPriceOfStock")]
        public async Task<IActionResult> GetResellPriceOfStock(Guid productID)
        {
            try
            {
                _logger.LogInformation($"Attempting to get resell price of stock with product ID {productID}");

                var stock = await Policy
                        .Handle<Exception>()
                        .RetryAsync(3)
                        .ExecuteAsync(async () => await _stockService.GetResellPriceOfStock(productID))
                        .ConfigureAwait(false);

                if (stock == null)
                {
                    _logger.LogInformation($"Stock with product ID {productID} could not be found");
                    return NotFound();
                }

                _logger.LogInformation($"Returning resell price of stock with product ID {productID}");
                return Ok(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500);
            }
        }

        // GET: api/Stock/GetResellHistoryOfStock
        // e.g. https://localhost:44305/api/Stock/GetResellHistoryOfStock?productID=397945de-54c3-413f-9b6a-5ad187069fd9
        [HttpGet("api/Stock/GetResellHistoryOfStock")]
        public async Task<IActionResult> GetResellHistoryOfStock(Guid productID)
        {
            try
            {
                _logger.LogInformation($"Attempting to get resell history of stock with product ID {productID}");

                var stock = await Policy
                        .Handle<Exception>()
                        .RetryAsync(3)
                        .ExecuteAsync(async () => await _stockService.GetResellHistoryOfStock(productID))
                        .ConfigureAwait(false);

                if (stock == null)
                {
                    _logger.LogInformation($"Stock with product ID {productID} could not be found");
                    return NotFound();
                }

                _logger.LogInformation($"Returning resell history of stock with product ID {productID}");
                return Ok(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500);
            }
        }

        // POST: api/Stock/SetResellPriceOfStock
        // e.g. https://localhost:44305/api/Stock/SetResellPriceOfStock?productID=397945de-54c3-413f-9b6a-5ad187069fd9&resellPrice=33.33
        [HttpPost("api/Stock/SetResellPriceOfStock")]
        public async Task<IActionResult> SetResellPriceOfStock([Bind("ProductID,ResellPrice")]StockDto objectStock)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"An invalid stock object was constructed");
                    return NoContent();
                }

                if (objectStock.ResellPrice < 0)
                {
                    _logger.LogInformation($"{objectStock.ResellPrice} is an invalid value, resell price cannot be less than 0");
                    return BadRequest();
                }

                _logger.LogInformation($"Attempting to set resell price of stock with product ID {objectStock.ProductID} to {objectStock.ResellPrice}");

                var stock = await Policy
                        .Handle<Exception>()
                        .RetryAsync(3)
                        .ExecuteAsync(async () => await _stockService.SetResellPriceOfStock(objectStock.ProductID, objectStock.ResellPrice))
                        .ConfigureAwait(false);

                if (stock == null)
                {
                    _logger.LogInformation($"Stock with product ID {objectStock.ProductID} could not be found");
                    return NotFound();
                }

                _logger.LogInformation($"Returning stock with product ID {objectStock.ProductID} and new resell price {objectStock.ResellPrice}");
                return Ok(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500);
            }
        }

        // POST: api/Stock/SetStockLevelOfStock
        // e.g. https://localhost:44305/api/Stock/SetStockLevelOfStock?productID=d8bd8422-0e51-4a7c-b508-cc17e3db1be9&stockLevel=69
        [HttpPost("api/Stock/SetStockLevelOfStock")]
        public async Task<IActionResult> SetStockLevelOfStock([Bind("ProductID,StockLevel")]StockDto objectStock)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"An invalid stock object was constructed");
                    return NoContent();
                }

                if (objectStock.StockLevel < 0)
                {
                    _logger.LogInformation($"{objectStock.StockLevel} is an invalid value, stock level cannot be less than 0");
                    return BadRequest();
                }

                _logger.LogInformation($"Attempting to set stock level of stock with product ID {objectStock.ProductID} to {objectStock.StockLevel}");

                var stock = await Policy
                        .Handle<Exception>()
                        .RetryAsync(3)
                        .ExecuteAsync(async () => await _stockService.SetStockLevelOfStock(objectStock.ProductID, objectStock.StockLevel))
                        .ConfigureAwait(false);

                if (stock == null)
                {
                    _logger.LogInformation($"Stock with product ID {objectStock.ProductID} could not be found");
                    return NotFound();
                }

                _logger.LogInformation($"Returning stock with product ID {objectStock.ProductID} and new stock level {objectStock.StockLevel}");
                return Ok(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500);
            }
        }

        #endregion Stock Methods

        #region Customer Methods (External)

        // GET: api/Stock/GetAllCustomers
        // e.g. https://localhost:44305/api/Stock/GetAllCustomers
        [HttpGet("api/Stock/GetAllCustomers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                _logger.LogInformation("Attempting to get all customers");

                // for integration
                //var token = HttpContext.Request.Headers["Authorization"].ToString();

                var customers = await Policy
                        .Handle<Exception>()
                        .RetryAsync(3)
                        .ExecuteAsync(async () => await _customersService.GetAllCustomers(/*token*/))
                        .ConfigureAwait(false);

                _logger.LogInformation($"Returning {customers.Count} customers");
                return Ok(customers);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500);
            }
        }

        // GET: api/Stock/GetCustomerByID
        // e.g. https://localhost:44305/api/Stock/GetCustomerByID?id=3854b6c9-e018-4fb7-8b63-1067384210a9
        [HttpGet("api/Stock/GetCustomerByID")]
        public async Task<IActionResult> GetCustomerByID(Guid id)
        {
            try
            {
                _logger.LogInformation($"Attempting to get customer with ID {id}");

                var customer = await Policy
                        .Handle<Exception>()
                        .RetryAsync(3)
                        .ExecuteAsync(async () => await _customersService.GetCustomerByID(id))
                        .ConfigureAwait(false);

                if (customer == null)
                {
                    _logger.LogInformation($"Customer with ID {id} could not be found");
                    return NotFound();
                }

                _logger.LogInformation($"Returning customer with ID {id}");
                return Ok(customer);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500);
            }
        }

        // POST: api/Stock/SetPurchaseAbilityOfCustomer
        // e.g. https://localhost:44305/api/Stock/SetPurchaseAbilityOfCustomer?id=3854b6c9-e018-4fb7-8b63-1067384210a9&canPurchase=false
        [HttpPost("api/Stock/SetPurchaseAbilityOfCustomer")]
        public async Task<IActionResult> SetPurchaseAbilityOfCustomer([Bind("ID,CanPurchase")]CustomerDto objectCustomer)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogInformation($"An invalid customer object was constructed");
                    return NoContent();
                }

                _logger.LogInformation($"Attempting to set stock purchase ability of customer with ID {objectCustomer.ID} to {objectCustomer.CanPurchase}");

                var customer = await Policy
                        .Handle<Exception>()
                        .RetryAsync(3)
                        .ExecuteAsync(async () => await _customersService.SetPurchaseAbilityOfCustomer(objectCustomer.ID, objectCustomer.CanPurchase))
                        .ConfigureAwait(false);

                if (customer == null)
                {
                    _logger.LogInformation($"Customer with ID {objectCustomer.ID} could not be found");
                    return NotFound();
                }

                _logger.LogInformation($"Returning customer with ID {objectCustomer.ID} and new purchase ability {objectCustomer.CanPurchase}");
                return Ok(customer);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Error message: {e.Message}\nStack trace: {e.StackTrace}");
                return StatusCode(500);
            }
        }

        #endregion Customer Methods
    }
}
