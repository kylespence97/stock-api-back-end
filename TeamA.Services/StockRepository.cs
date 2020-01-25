using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using TeamA.Models;
using TeamA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TeamA.Services
{
    public class StockRepository : IStockService
    {
        private readonly ProductManagementDb _context;
        private readonly ILogger<StockRepository> _logger;

        public StockRepository(ProductManagementDb context, ILogger<StockRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<StockDto>> GetAllStock()
        {
            try
            {
                _logger.LogInformation("Database call: Getting all stock");

                var stock = await _context.Stock.ToListAsync();

                _logger.LogInformation($"Database call: Returning {stock.Count} stock");

                return await Task.FromResult(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<StockDto> GetStockByProductID(Guid productID)
        {
            try
            {
                _logger.LogInformation($"Database call: Getting stock with product ID {productID}");

                var stock = _context.Stock.Where(s => s.ProductID == productID).FirstOrDefault();

                _logger.LogInformation($"Database call: Returning stock with product ID {stock.ProductID}");

                return await Task.FromResult(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<List<StockDto>> GetStockByStockLevel(int stockLevel)
        {
            try
            {
                _logger.LogInformation($"Database call: Getting stock filtered by stock level {stockLevel}");

                var stock = await _context.Stock.Where(s => s.StockLevel <= stockLevel).ToListAsync();

                _logger.LogInformation($"Database call: Returning stock filtered by stock level {stockLevel}");

                return await Task.FromResult(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<ResellPriceDto> GetResellPriceOfStock(Guid productID)
        {
            try
            {
                _logger.LogInformation($"Database call: Getting resell price of stock with product ID {productID}");

                var stock = _context.Stock
                               .Select(s => new ResellPriceDto
                               {
                                   ProductID = s.ProductID,
                                   ResellPrice = s.ResellPrice
                               }).Where(s => s.ProductID == productID)
                               .FirstOrDefault();

                _logger.LogInformation($"Database call: Returning resell price of stock with product ID {stock.ProductID}");

                return await Task.FromResult(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<StockDto> SetResellPriceOfStock(Guid productID, double resellPrice)
        {
            try
            {
                _logger.LogInformation($"Database call: Getting stock with product ID {productID}");

                var stock = await _context.Stock.Where(s => s.ProductID == productID).FirstOrDefaultAsync();

                if (stock != null && resellPrice >= 0)
                {
                    _logger.LogInformation($"Database call: Setting resell price of stock with product ID {stock.ProductID} to {resellPrice}");

                    stock.ResellPrice = resellPrice;

                    _context.Stock.Update(stock);

                    var resellHistory = new ResellHistoryDto
                    {
                        ID = Guid.NewGuid(),
                        ProductID = productID,
                        ResellPrice = resellPrice,
                        TimeUpdated = DateTime.UtcNow
                    };

                    _context.ResellHistory.Add(resellHistory);

                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation($"Database call: Returning stock with product ID {stock.ProductID} and new resell price {stock.ResellPrice}");

                return await Task.FromResult(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<StockDto> SetStockLevelOfStock(Guid productID, int stockLevel)
        {
            try
            {
                _logger.LogInformation($"Database call: Getting stock with product ID {productID}");

                var stock = await _context.Stock.Where(s => s.ProductID == productID).FirstOrDefaultAsync();

                if (stock != null && stockLevel >= 0)
                {
                    _logger.LogInformation($"Database call: Setting stock level of stock with product ID {stock.ProductID} to {stockLevel}");

                    stock.StockLevel = stockLevel;

                    _context.Stock.Update(stock);

                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation($"Database call: Returning stock with product ID {stock.ProductID} and new stock level {stock.StockLevel}");

                return await Task.FromResult(stock);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }

        public async Task<List<ResellHistoryDto>> GetResellHistoryOfStock(Guid productID)
        {
            try
            {
                _logger.LogInformation($"Database call: Getting resell history of stock with product ID {productID}");

                var resellHistory = await _context.ResellHistory.Where(s => s.ProductID == productID).ToListAsync();

                _logger.LogInformation($"Database call: Returning resell history of stock with product ID {productID}");

                return await Task.FromResult(resellHistory);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Database error: {e.Message}\nStack Trace: {e.StackTrace}");
                return null;
            }
        }
    }
}
