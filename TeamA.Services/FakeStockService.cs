using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamA.Models;

namespace TeamA.Services
{
    public class FakeStockService
    {
        public static readonly List<StockDto> _stock = new List<StockDto>
        {
            new StockDto {ID = new Guid("68cb71f9-93e4-41ed-99b4-0e0c805585d0"),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                StockLevel = 123,
                ResellPrice = 100.50,
            },
            new StockDto {ID = new Guid("6de2ebdc-d39a-4f12-b7d9-44453833bfee"),
                ProductID = new Guid("d9f17828-d2d4-47a7-b9f7-f3da369ef321"),
                StockLevel = 456,
                ResellPrice = 200.50,
            },
            new StockDto {ID = new Guid("36004789-ebe9-47f9-a45c-6502232e5304"),
                ProductID = new Guid("9ba76f81-ad1e-47ec-825c-a43fcdf027bc"),
                StockLevel = 789,
                ResellPrice = 300.50,
            }
        };

        public static readonly List<ResellPriceDto> _resellPrice = new List<ResellPriceDto>
        {
            new ResellPriceDto {
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrice = 100.50,
            },
            new ResellPriceDto {
                ProductID = new Guid("d9f17828-d2d4-47a7-b9f7-f3da369ef321"),
                ResellPrice = 200.50,
            },
            new ResellPriceDto {
                ProductID = new Guid("9ba76f81-ad1e-47ec-825c-a43fcdf027bc"),
                ResellPrice = 300.50,
            }
        };

        public static readonly List<ResellHistoryDto> _resellHistory = new List<ResellHistoryDto>
        {
            new ResellHistoryDto {ID = new Guid("68cb71f9-93e4-41ed-99b4-0e0c805585d0"),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrice = 1000.50,
                TimeUpdated = new DateTime(2019, 12, 2, 12, 00, 00),
            },
            new ResellHistoryDto {ID = new Guid("68cb71f9-93e4-41ed-99b4-0e0c805585d0"),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrice = 500.50,
                TimeUpdated = new DateTime(2019, 12, 2, 12, 30, 00),
            },
            new ResellHistoryDto {ID = new Guid("68cb71f9-93e4-41ed-99b4-0e0c805585d0"),
                ProductID = new Guid("db231658-e8ee-4b33-8c78-9abd30fa0e76"),
                ResellPrice = 100.50,
                TimeUpdated = new DateTime(2019, 12, 2, 13, 00, 00),
            },
        };

        public static StockDto GetStockByProductID(Guid productID)
        {
            var stock = _stock.Where(s => s.ProductID == productID).FirstOrDefault();

            return stock;
        }

        public static List<StockDto> GetStockByStockLevel(int stockLevel)
        {
            var stock = _stock.Where(s => s.StockLevel <= stockLevel).ToList();

            return stock;
        }

        public static ResellPriceDto GetResellPriceOfStock(Guid productID)
        {
            var stock = _resellPrice.Where(r => r.ProductID == productID).FirstOrDefault();

            return stock;
        }

        public static List<ResellHistoryDto> GetResellHistoryOfStock(Guid productID)
        {
            var resellHistory = _resellHistory.Where(r => r.ProductID == productID).ToList();

            return resellHistory;
        }

        public static StockDto SetResellPriceOfStock(Guid productID, double resellPrice)
        {
            var stock = _stock.Where(s => s.ProductID == productID).FirstOrDefault();

            if (stock != null && resellPrice >= 0)
            {
                stock.ResellPrice = resellPrice;
            }

            return stock;
        }

        public static StockDto SetStockLevelOfStock(Guid productID, int stockLevel)
        {
            var stock = _stock.Where(s => s.ProductID == productID).FirstOrDefault();

            if (stock != null && stockLevel >=0)
            {
                stock.StockLevel = stockLevel;
            }

            return stock;
        }
    }
}
