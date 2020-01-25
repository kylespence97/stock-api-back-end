using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Moq;
using TeamA.ProductManagementAPI.Controllers;
using TeamA.Data;
using TeamA.Models;
using TeamA.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace TeamA.Tests
{
    public class StockRepositoryTests
    {
        private IStockService _stockService;
        private Mock<ILogger<StockRepository>> _logger;
        private Guid goodProductID;
        private Guid badProductID;
        private int goodStockLevel;
        private int badStockLevel;
        private double goodResellPrice;
        private double badResellPrice;

        private ProductManagementDb GetProductManagementDb()
        {
            var options = new DbContextOptionsBuilder<ProductManagementDb>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new ProductManagementDb(options);

            FakeStockService._stock.ForEach(s => context.Stock.Add(s));

            context.SaveChanges();

            return context;
        }

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<StockRepository>>();
            _stockService = new StockRepository(GetProductManagementDb(), _logger.Object);
            goodProductID = FakeStockService._stock[1].ProductID;
            badProductID = Guid.NewGuid();
            goodStockLevel = FakeStockService._stock[1].StockLevel;
            badStockLevel = -50;
            goodResellPrice = FakeStockService._stock[1].ResellPrice;
            badResellPrice = -100.00;
        }

        #region GetAllStock
        [Test]
        public async Task GetAllStock_WithValidStock()
        {
            // Arrange
            var expected = FakeStockService._stock;

            // Act
            var result = await _stockService.GetAllStock();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<StockDto>>(result);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i].ID, result[i].ID);
                Assert.AreEqual(expected[i].ProductID, result[i].ProductID);
                Assert.AreEqual(expected[i].ResellPrice, result[i].ResellPrice);
                Assert.AreEqual(expected[i].StockLevel, result[i].StockLevel);
            }
        }
        #endregion GetAllStock

        #region GetStockByProductID
        [Test]
        public async Task GetStockByProductID_WithValidProductID_ShouldReturnStock()
        {
            // Arrange
            var expected = FakeStockService._stock[1];

            // Act
            var result = await _stockService.GetStockByProductID(expected.ProductID);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<StockDto>(result);
            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.ProductID, result.ProductID);
            Assert.AreEqual(expected.ResellPrice, result.ResellPrice);
            Assert.AreEqual(expected.StockLevel, result.StockLevel);
        }

        [Test]
        public async Task GetStockByProductID_WithInvalidProductID_ShouldReturnNull()
        {
            // Arrange
            var expected = FakeStockService.GetStockByProductID(badProductID);

            // Act
            var result = await _stockService.GetStockByProductID(badProductID);

            // Assert
            Assert.IsNull(result);
            Assert.IsNull(expected);
            Assert.AreEqual(expected, result);
        }
        #endregion GetStockByProductID

        #region GetStockByStocklevel
        [Test]
        public async Task GetStockByStockLevel_WithValidStockLevel_ShouldReturnStock()
        {
            // Arrange
            var expected = FakeStockService.GetStockByStockLevel(goodStockLevel);

            // Act
            var result = await _stockService.GetStockByStockLevel(goodStockLevel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<StockDto>>(result);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i].ID, result[i].ID);
                Assert.AreEqual(expected[i].ProductID, result[i].ProductID);
                Assert.AreEqual(expected[i].ResellPrice, result[i].ResellPrice);
                Assert.AreEqual(expected[i].StockLevel, result[i].StockLevel);
            }
        }

        [Test]
        public async Task GetStockByStockLevel_WithInvalidStockLevel_ShouldReturnEmpty()
        {
            // Arrange
            var expected = FakeStockService.GetStockByStockLevel(badStockLevel);

            // Act
            var result = await _stockService.GetStockByStockLevel(badStockLevel);

            // Assert
            Assert.IsEmpty(result);
            Assert.IsInstanceOf<List<StockDto>>(result);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i].ID, result[i].ID);
                Assert.AreEqual(expected[i].ProductID, result[i].ProductID);
                Assert.AreEqual(expected[i].ResellPrice, result[i].ResellPrice);
                Assert.AreEqual(expected[i].StockLevel, result[i].StockLevel);
            }
        }
        #endregion GetStockByStocklevel

        #region GetResellPriceOfStock
        [Test]
        public async Task GetResellPriceOfStock_WithValidProductID_ShouldReturnResellPrice()
        {
            // Arrange
            var expected = FakeStockService._resellPrice[1];

            // Act
            var result = await _stockService.GetResellPriceOfStock(expected.ProductID);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ResellPriceDto>(result);
            Assert.AreEqual(expected.ProductID, result.ProductID);
            Assert.AreEqual(expected.ResellPrice, result.ResellPrice);
        }

        [Test]
        public async Task GetResellPriceOfStock_WithInvalidProductID_ShouldReturnNull()
        {
            // Arrange
            var expected = FakeStockService.GetResellPriceOfStock(badProductID);

            // Act
            var result = await _stockService.GetResellPriceOfStock(badProductID);

            // Assert
            Assert.IsNull(result);
            Assert.IsNull(expected);
            Assert.AreEqual(expected, result);
        }
        #endregion GetResellPriceOfStock

        #region GetResellHistoryOfStock
        [Test]
        public async Task GetResellHistoryOfStock_WithValidProductID_ShouldReturnHistory()
        {
            // Arrange
            var expected = FakeStockService.GetResellHistoryOfStock(goodProductID);

            // Act
            var result = await _stockService.GetResellHistoryOfStock(goodProductID);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<ResellHistoryDto>>(result);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i].ID, result[i].ID);
                Assert.AreEqual(expected[i].ProductID, result[i].ProductID);
                Assert.AreEqual(expected[i].ResellPrice, result[i].ResellPrice);
                Assert.AreEqual(expected[i].TimeUpdated, result[i].TimeUpdated);
            }
        }

        [Test]
        public async Task GetResellHistoryOfStock_WithInvalidProductID_ShouldReturnEmpty()
        {
            // Arrange
            var expected = FakeStockService.GetResellHistoryOfStock(badProductID);

            // Act
            var result = await _stockService.GetResellHistoryOfStock(badProductID);

            // Assert
            Assert.IsEmpty(result);
            Assert.IsInstanceOf<List<ResellHistoryDto>>(result);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i].ID, result[i].ID);
                Assert.AreEqual(expected[i].ProductID, result[i].ProductID);
                Assert.AreEqual(expected[i].ResellPrice, result[i].ResellPrice);
                Assert.AreEqual(expected[i].TimeUpdated, result[i].TimeUpdated);
            }
        }
        #endregion GetResellHistoryOfStock

        #region SetResellPriceOfStock
        [Test]
        public async Task SetResellPriceOfStock_WithValidProductID__WithValidResellPrice_ShouldReturnStock()
        {
            // Arrange
            var expected = FakeStockService._stock[1];

            // Act
            var result = await _stockService.SetResellPriceOfStock(expected.ProductID, expected.ResellPrice);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<StockDto>(result);
            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.ProductID, result.ProductID);
            Assert.AreEqual(expected.StockLevel, result.StockLevel);
            Assert.AreEqual(expected.ResellPrice, result.ResellPrice);
        }

        [Test]
        public async Task SetResellPriceOfStock_WithInvalidProductID__WithValidResellPrice_ShouldReturnNull()
        {
            // Arrange
            var expected = FakeStockService.SetResellPriceOfStock(badProductID, goodResellPrice);

            // Act
            var result = await _stockService.SetResellPriceOfStock(badProductID, goodResellPrice);

            // Assert
            Assert.IsNull(result);
            Assert.IsNull(expected);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task SetResellPriceOfStock_WithValidProductID__WithInvalidResellPrice_ShouldReturnStock()
        {
            // Arrange
            var expected = FakeStockService.SetResellPriceOfStock(goodProductID, badResellPrice);

            // Act
            var result = await _stockService.SetResellPriceOfStock(goodProductID, badResellPrice);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<StockDto>(result);
            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.ProductID, result.ProductID);
            Assert.AreEqual(expected.StockLevel, result.StockLevel);
            Assert.AreEqual(expected.ResellPrice, result.ResellPrice);
        }

        [Test]
        public async Task SetResellPriceOfStock_WithInvalidProductID__WithInvalidResellPrice_ShouldReturnNull()
        {
            // Arrange
            var expected = FakeStockService.SetResellPriceOfStock(badProductID, badResellPrice);

            // Act
            var result = await _stockService.SetResellPriceOfStock(badProductID, badResellPrice);

            // Assert
            Assert.IsNull(result);
            Assert.IsNull(expected);
            Assert.AreEqual(expected, result);
        }
        #endregion SetResellPriceOfStock

        #region SetStockLevelOfStock
        [Test]
        public async Task SetStockLevelOfStock_WithValidProductID_WithValidStockLevel_ShouldReturnStock()
        {
            // Arrange
            var expected = FakeStockService._stock[1];

            // Act
            var result = await _stockService.SetStockLevelOfStock(expected.ProductID, expected.StockLevel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<StockDto>(result);
            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.ProductID, result.ProductID);
            Assert.AreEqual(expected.StockLevel, result.StockLevel);
            Assert.AreEqual(expected.ResellPrice, result.ResellPrice);
        }

        [Test]
        public async Task SetStockLevelOfStock_WithInvalidProductID_WithValidStockLevel_ShouldReturnNull()
        {
            // Arrange
            var expected = FakeStockService.SetStockLevelOfStock(badProductID, goodStockLevel);

            // Act
            var result = await _stockService.SetStockLevelOfStock(badProductID, goodStockLevel);

            // Assert
            Assert.IsNull(result);
            Assert.IsNull(expected);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task SetStockLevelOfStock_WithValidProductID_WithInvalidStockLevel_ShouldReturnStock()
        {
            // Arrange
            var expected = FakeStockService.SetStockLevelOfStock(goodProductID, badStockLevel);

            // Act
            var result = await _stockService.SetStockLevelOfStock(goodProductID, badStockLevel);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<StockDto>(result);
            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.ProductID, result.ProductID);
            Assert.AreEqual(expected.StockLevel, result.StockLevel);
            Assert.AreEqual(expected.ResellPrice, result.ResellPrice);
        }

        [Test]
        public async Task SetStockLevelOfStock_WithInvalidProductID_WithInvalidStockLevel_ShouldReturnNull()
        {
            // Arrange
            var expected = FakeStockService.SetStockLevelOfStock(badProductID, badStockLevel);

            // Act
            var result = await _stockService.SetStockLevelOfStock(badProductID, badStockLevel);

            // Assert
            Assert.IsNull(result);
            Assert.IsNull(expected);
            Assert.AreEqual(expected, result);
        }
        #endregion SetResellPriceOfStock
    }
}