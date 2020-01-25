using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamA.Models;

namespace TeamA.Services
{
    public interface IStockService
    {
        Task<List<StockDto>> GetAllStock();
        Task<StockDto> GetStockByProductID(Guid productID);
        Task<List<StockDto>> GetStockByStockLevel(int stockLevel);
        Task<ResellPriceDto> GetResellPriceOfStock(Guid productID);
        Task<StockDto> SetResellPriceOfStock(Guid productID, double resellPrice);
        Task<StockDto> SetStockLevelOfStock(Guid productID, int stockLevel);
        Task<List<ResellHistoryDto>> GetResellHistoryOfStock(Guid productID);
    }
}
