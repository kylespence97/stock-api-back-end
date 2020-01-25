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

namespace TeamA.Tests
{
    public class StockControllerTests
    {
        private Mock<IStockService> _mockStockService;
        private Mock<ICustomersService> _mockCustomerService;
        private Mock<ILogger<StockController>> _mockLogger;
        private StockController _stockController;
        private List<StockDto> _stock;
        private List<ResellPriceDto> _resellPrice;
        private List<ResellHistoryDto> _resellHistory;
        private List<CustomerDto> _customers;
        private Guid badProductID;
        private double goodResellPrice;
        private double badResellPrice;
        private int goodStockLevel;
        private int badStockLevel;
        private StockDto objectStock;
        private CustomerDto objectCustomer;

        [SetUp]
        public void Setup()
        {
            _mockStockService = new Mock<IStockService>();
            _mockCustomerService = new Mock<ICustomersService>();
            _mockLogger = new Mock<ILogger<StockController>>();
            _stockController = new StockController(_mockStockService.Object, _mockCustomerService.Object, _mockLogger.Object);
            _stock = FakeStockService._stock;
            _resellPrice = FakeStockService._resellPrice;
            _resellHistory = FakeStockService._resellHistory;
            _customers = FakeCustomersService._customers;
            badProductID = Guid.NewGuid();
            goodResellPrice = 100.00;
            badResellPrice = -100.00;
            goodStockLevel = 50;
            badStockLevel = -50;
            objectStock = new StockDto();
            objectCustomer = new CustomerDto();
        }

        #region GetAllStock
        [Test]
        public async Task GetAllStock_ShouldOkObject()
        {
            // Arrange
            _mockStockService.Setup(repo => repo.GetAllStock()).ReturnsAsync(_stock).Verifiable();

            // Act
            var result = await _stockController.GetAllStock();

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var stockResult = objResult.Value as IEnumerable<StockDto>;
            Assert.IsNotNull(stockResult);
            var stockResultList = stockResult.ToList();
            Assert.AreEqual(_stock.Count, stockResultList.Count);
            for (int i = 0; i < _stock.Count; i++)
            {
                Assert.AreEqual(_stock[i].ID, stockResultList[i].ID);
                Assert.AreEqual(_stock[i].ProductID, stockResultList[i].ProductID);
                Assert.AreEqual(_stock[i].StockLevel, stockResultList[i].StockLevel);
                Assert.AreEqual(_stock[i].ResellPrice, stockResultList[i].ResellPrice);
            }
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.GetAllStock(), Times.Once);
        }
        #endregion GetAllStock

        #region GetStockByProductID
        [Test]
        public async Task GetStockByProductID_WithInvalidProductID_ShouldNotFound()
        {
            // Arrange
            _mockStockService.Setup(repo => repo.GetStockByProductID(badProductID)).ReturnsAsync(null as StockDto).Verifiable();

            // Act
            var result = await _stockController.GetStockByProductID(badProductID);

            // Assert
            Assert.IsNotNull(result);
            var notResult = result as NotFoundResult;
            Assert.IsNotNull(notResult);
            Assert.AreNotEqual(notResult.StatusCode, 500);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.GetStockByProductID(badProductID), Times.Once);
        }

        [Test]
        public async Task GetStockByProductID_WithValidProductID_ShouldOkObject()
        {
            // Arrange
            var expected = _stock[1];
            _mockStockService.Setup(repo => repo.GetStockByProductID(expected.ProductID)).ReturnsAsync(expected).Verifiable();

            // Act
            var result = await _stockController.GetStockByProductID(expected.ProductID);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var stockResult = objResult.Value as StockDto;
            Assert.IsNotNull(stockResult);
            Assert.AreEqual(expected.ID, stockResult.ID);
            Assert.AreEqual(expected.ProductID, stockResult.ProductID);
            Assert.AreEqual(expected.StockLevel, stockResult.StockLevel);
            Assert.AreEqual(expected.ResellPrice, stockResult.ResellPrice);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.GetStockByProductID(expected.ProductID), Times.Once);
        }
        #endregion GetStockByProductID

        #region GetStockByStockLevel
        [Test]
        public async Task GetStockByStockLevel_WithValidStockLevel_ShouldOkObject()
        {
            // Arrange
            _mockStockService.Setup(repo => repo.GetStockByStockLevel(goodStockLevel)).ReturnsAsync(_stock).Verifiable();

            // Act
            var result = await _stockController.GetStockByStockLevel(goodStockLevel);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var stockResult = objResult.Value as IEnumerable<StockDto>;
            Assert.IsNotNull(stockResult);
            var stockResultList = stockResult.ToList();
            Assert.AreEqual(_stock.Count, stockResultList.Count);
            for (int i = 0; i < _stock.Count; i++)
            {
                Assert.AreEqual(_stock[i].ID, stockResultList[i].ID);
                Assert.AreEqual(_stock[i].ProductID, stockResultList[i].ProductID);
                Assert.AreEqual(_stock[i].StockLevel, stockResultList[i].StockLevel);
                Assert.AreEqual(_stock[i].ResellPrice, stockResultList[i].ResellPrice);
            }
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.GetStockByStockLevel(goodStockLevel), Times.Once);
        }

        [Test]
        public async Task GetStockByStockLevel_WithInvalidStockLevel_ShouldBadRequest()
        {
            // Arrange
            // Nothing else to arrange as BadRequest will be hit before repo is accessed

            // Act
            var result = await _stockController.GetStockByStockLevel(badStockLevel);

            // Assert
            Assert.IsNotNull(result);
            var badResult = result as BadRequestResult;
            Assert.IsNotNull(badResult);
            Assert.AreNotEqual(badResult.StatusCode, 500);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.GetStockByStockLevel(badStockLevel), Times.Never);
        }
        #endregion GetStockByStockLevel

        #region GetResellPriceOfStock
        [Test]
        public async Task GetResellPriceOfStock_WithInvalidProductID_ShouldNotFound()
        {
            // Arrange
            _mockStockService.Setup(repo => repo.GetResellPriceOfStock(badProductID)).ReturnsAsync(null as ResellPriceDto).Verifiable();

            // Act
            var result = await _stockController.GetResellPriceOfStock(badProductID);

            // Assert
            Assert.IsNotNull(result);
            var notResult = result as NotFoundResult;
            Assert.IsNotNull(notResult);
            Assert.AreNotEqual(notResult.StatusCode, 500);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.GetResellPriceOfStock(badProductID), Times.Once);
        }

        [Test]
        public async Task GetResellPriceOfStock_WithValidProductID_ShouldOkObject()
        {
            // Arrange
            var expected = _resellPrice[1];
            _mockStockService.Setup(repo => repo.GetResellPriceOfStock(expected.ProductID)).ReturnsAsync(expected).Verifiable();

            // Act
            var result = await _stockController.GetResellPriceOfStock(expected.ProductID);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var stockResult = objResult.Value as ResellPriceDto;
            Assert.IsNotNull(stockResult);
            Assert.AreEqual(expected.ProductID, stockResult.ProductID);
            Assert.AreEqual(expected.ResellPrice, stockResult.ResellPrice);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.GetResellPriceOfStock(expected.ProductID), Times.Once);
        }
        #endregion GetResellPriceOfStock

        #region GetResellHistoryOfStock
        [Test]
        public async Task GetResellHistoryOfStock_WithInvalidProductID_ShouldNotFound()
        {
            // Arrange
            _mockStockService.Setup(repo => repo.GetResellHistoryOfStock(badProductID)).ReturnsAsync(null as List<ResellHistoryDto>).Verifiable();

            // Act
            var result = await _stockController.GetResellHistoryOfStock(badProductID);

            // Assert
            Assert.IsNotNull(result);
            var notResult = result as NotFoundResult;
            Assert.AreNotEqual(notResult.StatusCode, 500);
            Assert.IsNotNull(notResult);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.GetResellHistoryOfStock(badProductID), Times.Once);
        }

        [Test]
        public async Task GetResellHistoryOfStock_WithValidProductID_ShouldOkObject()
        {
            // Arrange
            var expected = _resellHistory;
            _mockStockService.Setup(repo => repo.GetResellHistoryOfStock(expected[1].ProductID)).ReturnsAsync(expected).Verifiable();

            // Act
            var result = await _stockController.GetResellHistoryOfStock(expected[1].ProductID);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var resellHistoryResult = objResult.Value as IEnumerable<ResellHistoryDto>;
            Assert.IsNotNull(resellHistoryResult);
            var resellHistoryResultList = resellHistoryResult.ToList();
            Assert.AreEqual(_resellHistory.Count, resellHistoryResultList.Count);
            for (int i = 0; i < _resellHistory.Count; i++)
            {
                Assert.AreEqual(_resellHistory[i].ID, resellHistoryResultList[i].ID);
                Assert.AreEqual(_resellHistory[i].ProductID, resellHistoryResultList[i].ProductID);
                Assert.AreEqual(_resellHistory[i].ResellPrice, resellHistoryResultList[i].ResellPrice);
                Assert.AreEqual(_resellHistory[i].TimeUpdated, resellHistoryResultList[i].TimeUpdated);
            }
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.GetResellHistoryOfStock(expected[1].ProductID), Times.Once);
        }
        #endregion GetResellHistoryOfStock

        #region SetResellPriceOfStock
        [Test]
        public async Task SetResellPriceOfStock_WithInvalidProductID_WithValidResellPrice_ShouldNotFound()
        {
            // Arrange
            _mockStockService.Setup(repo => repo.SetResellPriceOfStock(badProductID, goodResellPrice)).ReturnsAsync(null as StockDto).Verifiable();
            objectStock.ProductID = badProductID;
            objectStock.ResellPrice = goodResellPrice;

            // Act
            var result = await _stockController.SetResellPriceOfStock(objectStock);

            // Assert
            Assert.IsNotNull(result);
            var notResult = result as NotFoundResult;
            Assert.IsNotNull(notResult);
            Assert.AreNotEqual(notResult.StatusCode, 500);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.SetResellPriceOfStock(badProductID, goodResellPrice), Times.Once);
        }

        [Test]
        public async Task SetResellPriceOfStock_WithValidProductID__WithValidResellPrice_ShouldOkObject()
        {
            // Arrange
            var expected = _stock[1];
            _mockStockService.Setup(repo => repo.SetResellPriceOfStock(expected.ID, expected.ResellPrice)).ReturnsAsync(expected).Verifiable();
            objectStock.ProductID = expected.ID;
            objectStock.ResellPrice = expected.ResellPrice;

            // Act
            var result = await _stockController.SetResellPriceOfStock(objectStock);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var stockResult = objResult.Value as StockDto;
            Assert.IsNotNull(stockResult);
            Assert.AreEqual(expected.ID, stockResult.ID);
            Assert.AreEqual(expected.ProductID, stockResult.ProductID);
            Assert.AreEqual(expected.StockLevel, stockResult.StockLevel);
            Assert.AreEqual(expected.ResellPrice, stockResult.ResellPrice);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.SetResellPriceOfStock(expected.ID, expected.ResellPrice), Times.Once);
        }

        [Test]
        public async Task SetResellPriceOfStock_WithInvalidProductID_WithInvalidResellPrice_ShouldBadRequest()
        {
            // Arrange
            objectStock.ProductID = badProductID;
            objectStock.ResellPrice = badResellPrice;

            // Act
            var result = await _stockController.SetResellPriceOfStock(objectStock);

            // Assert
            Assert.IsNotNull(result);
            var badResult = result as BadRequestResult;
            Assert.IsNotNull(badResult);
            Assert.AreNotEqual(badResult.StatusCode, 500);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.SetResellPriceOfStock(badProductID, badResellPrice), Times.Never);
        }

        [Test]
        public async Task SetResellPriceOfStock_WithValidProductID_WithInvalidResellPrice_ShouldBadRequest()
        {
            // Arrange
            var expected = _stock[1];
            objectStock.ProductID = expected.ID;
            objectStock.ResellPrice = badResellPrice;

            // Act
            var result = await _stockController.SetResellPriceOfStock(objectStock);

            // Assert
            Assert.IsNotNull(result);
            var badResult = result as BadRequestResult;
            Assert.IsNotNull(badResult);
            Assert.AreNotEqual(badResult.StatusCode, 500);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.SetResellPriceOfStock(expected.ID, badResellPrice), Times.Never);
        }
        #endregion SetResellPriceOfStock

        #region SetStockLevelOfStock
        [Test]
        public async Task SetStockLevelOfStock_WithInvalidProductID_WithValidStockLevel_ShouldNotFound()
        {
            // Arrange
            _mockStockService.Setup(repo => repo.SetStockLevelOfStock(badProductID, goodStockLevel)).ReturnsAsync(null as StockDto).Verifiable();
            objectStock.ProductID = badProductID;
            objectStock.StockLevel = goodStockLevel;

            // Act
            var result = await _stockController.SetStockLevelOfStock(objectStock);

            // Assert
            Assert.IsNotNull(result);
            var notResult = result as NotFoundResult;
            Assert.IsNotNull(notResult);
            Assert.AreNotEqual(notResult.StatusCode, 500);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.SetStockLevelOfStock(badProductID, goodStockLevel), Times.Once);
        }


        [Test]
        public async Task SetStockLevelOfStock_WithValidProductID_WithValidStockLevel_ShouldOkObject()
        {
            // Arrange
            var expected = _stock[1];
            _mockStockService.Setup(repo => repo.SetStockLevelOfStock(expected.ID, expected.StockLevel)).ReturnsAsync(expected).Verifiable();
            objectStock.ProductID = expected.ID;
            objectStock.StockLevel = expected.StockLevel;

            // Act
            var result = await _stockController.SetStockLevelOfStock(objectStock);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var stockResult = objResult.Value as StockDto;
            Assert.IsNotNull(stockResult);
            Assert.AreEqual(expected.ID, stockResult.ID);
            Assert.AreEqual(expected.ProductID, stockResult.ProductID);
            Assert.AreEqual(expected.StockLevel, stockResult.StockLevel);
            Assert.AreEqual(expected.ResellPrice, stockResult.ResellPrice);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.SetStockLevelOfStock(expected.ID, expected.StockLevel), Times.Once);
        }

        [Test]
        public async Task SetStockLevelOfStock_WithInvalidProductID_WithInvalidStockLevel_ShouldBadRequest()
        {
            // Arrange
            objectStock.ProductID = badProductID;
            objectStock.StockLevel = badStockLevel;

            // Act
            var result = await _stockController.SetStockLevelOfStock(objectStock);

            // Assert
            Assert.IsNotNull(result);
            var badResult = result as BadRequestResult;
            Assert.IsNotNull(badResult);
            Assert.AreNotEqual(badResult.StatusCode, 500);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.SetStockLevelOfStock(badProductID, badStockLevel), Times.Never);
        }

        [Test]
        public async Task SetStockLevelOfStock_WithValidProductID_WithInvalidStockLevel_ShouldBadRequest()
        {
            // Arrange
            var expected = _stock[1];
            objectStock.ProductID = expected.ID;
            objectStock.StockLevel = badStockLevel;

            // Act
            var result = await _stockController.SetStockLevelOfStock(objectStock);

            // Assert
            Assert.IsNotNull(result);
            var badResult = result as BadRequestResult;
            Assert.IsNotNull(badResult);
            Assert.AreNotEqual(badResult.StatusCode, 500);
            _mockStockService.Verify();
            _mockStockService.Verify(repo => repo.SetStockLevelOfStock(expected.ID, badStockLevel), Times.Never);
        }
        #endregion SetStockLevelOfStock

        #region GetAllCustomers
        [Test]
        public async Task GetAllCustomers_ShouldOkObject()
        {
            // Arrange
            _mockCustomerService.Setup(repo => repo.GetAllCustomers()).ReturnsAsync(_customers).Verifiable();

            // Act
            var result = await _stockController.GetAllCustomers();

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var customersResult = objResult.Value as IEnumerable<CustomerDto>;
            Assert.IsNotNull(customersResult);
            var customersResultList = customersResult.ToList();
            Assert.AreEqual(_customers.Count, customersResultList.Count);
            for (int i = 0; i < _stock.Count; i++)
            {
                Assert.AreEqual(_customers[i].ID, customersResultList[i].ID);
                Assert.AreEqual(_customers[i].FirstName, customersResultList[i].FirstName);
                Assert.AreEqual(_customers[i].LastName, customersResultList[i].LastName);
                Assert.AreEqual(_customers[i].Email, customersResultList[i].Email);
                Assert.AreEqual(_customers[i].Address, customersResultList[i].Address);
                Assert.AreEqual(_customers[i].Postcode, customersResultList[i].Postcode);
                Assert.AreEqual(_customers[i].DOB, customersResultList[i].DOB);
                Assert.AreEqual(_customers[i].LoggedOnAt, customersResultList[i].LoggedOnAt);
                Assert.AreEqual(_customers[i].PhoneNumber, customersResultList[i].PhoneNumber);
                Assert.AreEqual(_customers[i].CanPurchase, customersResultList[i].CanPurchase);
            }
            _mockCustomerService.Verify();
            _mockCustomerService.Verify(repo => repo.GetAllCustomers(), Times.Once);
        }
        #endregion GetAllCustomers

        #region GetCustomerByID
        [Test]
        public async Task GetCustomerByID_WithInvalidID_ShouldNotFound()
        {
            // Arrange
            _mockCustomerService.Setup(repo => repo.GetCustomerByID(badProductID)).ReturnsAsync(null as CustomerDto).Verifiable();

            // Act
            var result = await _stockController.GetCustomerByID(badProductID);

            // Assert
            Assert.IsNotNull(result);
            var notResult = result as NotFoundResult;
            Assert.AreNotEqual(notResult.StatusCode, 500);
            Assert.IsNotNull(notResult);
            _mockCustomerService.Verify();
            _mockCustomerService.Verify(repo => repo.GetCustomerByID(badProductID), Times.Once);
        }

        [Test]
        public async Task GetCustomerByID_WithValidID_ShouldOkObject()
        {
            // Arrange
            var expected = _customers[1];
            _mockCustomerService.Setup(repo => repo.GetCustomerByID(expected.ID)).ReturnsAsync(expected).Verifiable();

            // Act
            var result = await _stockController.GetCustomerByID(expected.ID);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var customerResult = objResult.Value as CustomerDto;
            Assert.IsNotNull(customerResult);
            Assert.AreEqual(expected.ID, customerResult.ID);
            Assert.AreEqual(expected.FirstName, customerResult.FirstName);
            Assert.AreEqual(expected.LastName, customerResult.LastName);
            Assert.AreEqual(expected.Email, customerResult.Email);
            Assert.AreEqual(expected.Address, customerResult.Address);
            Assert.AreEqual(expected.Postcode, customerResult.Postcode);
            Assert.AreEqual(expected.DOB, customerResult.DOB);
            Assert.AreEqual(expected.LoggedOnAt, customerResult.LoggedOnAt);
            Assert.AreEqual(expected.PhoneNumber, customerResult.PhoneNumber);
            Assert.AreEqual(expected.CanPurchase, customerResult.CanPurchase);
            _mockCustomerService.Verify();
            _mockCustomerService.Verify(repo => repo.GetCustomerByID(expected.ID), Times.Once);
        }
        #endregion GetCustomerByID

        #region SetPurchaseAbilityOfCustomer
        [Test]
        public async Task SetPurchaseAbilityOfCustomer_WithInvalidID_ShouldNotFound()
        {
            // Arrange
            _mockCustomerService.Setup(repo => repo.SetPurchaseAbilityOfCustomer(badProductID, true)).ReturnsAsync(null as CustomerDto).Verifiable();
            objectCustomer.ID = badProductID;
            objectCustomer.CanPurchase = true;

            // Act
            var result = await _stockController.SetPurchaseAbilityOfCustomer(objectCustomer);

            // Assert
            Assert.IsNotNull(result);
            var notResult = result as NotFoundResult;
            Assert.IsNotNull(notResult);
            Assert.AreNotEqual(notResult.StatusCode, 500);
            _mockCustomerService.Verify();
            _mockCustomerService.Verify(repo => repo.SetPurchaseAbilityOfCustomer(badProductID, true), Times.Once);
        }


        [Test]
        public async Task SetPurchaseAbilityOfCustomer_WithValidID_ShouldOkObject()
        {
            // Arrange
            var expected = _customers[1];
            _mockCustomerService.Setup(repo => repo.SetPurchaseAbilityOfCustomer(expected.ID, expected.CanPurchase)).ReturnsAsync(expected).Verifiable();
            objectCustomer.ID = expected.ID;
            objectCustomer.CanPurchase = expected.CanPurchase;

            // Act
            var result = await _stockController.SetPurchaseAbilityOfCustomer(objectCustomer);

            // Assert
            Assert.IsNotNull(result);
            var objResult = result as OkObjectResult;
            Assert.IsNotNull(objResult);
            Assert.AreNotEqual(objResult.StatusCode, 500);
            var customerResult = objResult.Value as CustomerDto;
            Assert.IsNotNull(customerResult);
            Assert.AreEqual(expected.ID, customerResult.ID);
            Assert.AreEqual(expected.FirstName, customerResult.FirstName);
            Assert.AreEqual(expected.LastName, customerResult.LastName);
            Assert.AreEqual(expected.Email, customerResult.Email);
            Assert.AreEqual(expected.Address, customerResult.Address);
            Assert.AreEqual(expected.Postcode, customerResult.Postcode);
            Assert.AreEqual(expected.DOB, customerResult.DOB);
            Assert.AreEqual(expected.LoggedOnAt, customerResult.LoggedOnAt);
            Assert.AreEqual(expected.PhoneNumber, customerResult.PhoneNumber);
            Assert.AreEqual(expected.CanPurchase, customerResult.CanPurchase);
            _mockCustomerService.Verify();
            _mockCustomerService.Verify(repo => repo.SetPurchaseAbilityOfCustomer(expected.ID, expected.CanPurchase), Times.Once);
        }
        #endregion SetPurchaseAbilityOfCustomer
    }
}