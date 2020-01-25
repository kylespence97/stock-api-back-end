using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamA.Data
{
    public static class ProductManagementDbInitialiser
    {
        public static async Task SeedTestData(ProductManagementDb context,
                                              IServiceProvider services)
        {
            if (context.Stock.Any() && context.ResellHistory.Any())
            {
                //db seems to be seeded
                return;
            }

            var stock = new List<Models.StockDto>
            {
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 10, ResellPrice = 10.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 20, ResellPrice = 20.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 30, ResellPrice = 30.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 40, ResellPrice = 40.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 50, ResellPrice = 50.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 60, ResellPrice = 60.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 70, ResellPrice = 70.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 80, ResellPrice = 80.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 90, ResellPrice = 90.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 100, ResellPrice = 100.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 110, ResellPrice = 110.99},
                new Models.StockDto {ProductID = Guid.NewGuid(), StockLevel = 120, ResellPrice = 120.99},
            };
            stock.ForEach(s => context.Stock.Add(s));

            var resellHistory = new List<Models.ResellHistoryDto>
            {
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 22.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 31, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 21.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 32, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 20.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 33, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 19.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 34, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 18.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 35, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 17.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 36, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 16.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 37, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 15.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 38, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 14.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 39, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 13.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 40, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 12.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 41, 00)},
                new Models.ResellHistoryDto {ProductID = stock[0].ProductID, ResellPrice = 11.99, TimeUpdated = new DateTime(2019, 11, 21, 16, 42, 00)},
            };
            resellHistory.ForEach(r => context.ResellHistory.Add(r));

            await context.SaveChangesAsync();
        }
    }
}
