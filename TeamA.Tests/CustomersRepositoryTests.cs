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
    public class CustomersRepositoryTests
    {
        private ICustomersService _customerService;
        private Mock<ILogger<CustomersRepository>> _logger;
        private Guid badID;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<CustomersRepository>>();
            _customerService = new CustomersRepository(_logger.Object);
            badID = Guid.NewGuid();
        }

        #region GetAllCustomers
        [Test]
        public async Task GetAllStock_WithValidCustomers()
        {
            // Arrange
            var expected = FakeCustomersService._customers;

            // Act
            var result = await _customerService.GetAllCustomers();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<CustomerDto>>(result);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expected[i].ID, result[i].ID);
                Assert.AreEqual(expected[i].FirstName, result[i].FirstName);
                Assert.AreEqual(expected[i].LastName, result[i].LastName);
                Assert.AreEqual(expected[i].Email, result[i].Email);
                Assert.AreEqual(expected[i].Address, result[i].Address);
                Assert.AreEqual(expected[i].Postcode, result[i].Postcode);
                Assert.AreEqual(expected[i].DOB, result[i].DOB);
                Assert.AreEqual(expected[i].LoggedOnAt, result[i].LoggedOnAt);
                Assert.AreEqual(expected[i].PhoneNumber, result[i].PhoneNumber);
                Assert.AreEqual(expected[i].CanPurchase, result[i].CanPurchase);
            }
        }
        #endregion GetAllCustomers

        #region GetCustomerByID
        [Test]
        public async Task GetCustomerByID_WithValidID_ShouldReturnCustomer()
        {
            // Arrange
            var expected = FakeCustomersService._customers[1];

            // Act
            var result = await _customerService.GetCustomerByID(expected.ID);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CustomerDto>(result);
            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.FirstName, result.FirstName);
            Assert.AreEqual(expected.LastName, result.LastName);
            Assert.AreEqual(expected.Email, result.Email);
            Assert.AreEqual(expected.Address, result.Address);
            Assert.AreEqual(expected.Postcode, result.Postcode);
            Assert.AreEqual(expected.DOB, result.DOB);
            Assert.AreEqual(expected.LoggedOnAt, result.LoggedOnAt);
            Assert.AreEqual(expected.PhoneNumber, result.PhoneNumber);
            Assert.AreEqual(expected.CanPurchase, result.CanPurchase);
        }

        [Test]
        public async Task GetCustomerByID_WithInvalidID_ShouldReturnNull()
        {
            // Arrange
            var expected = FakeCustomersService.GetCustomerByID(badID);

            // Act
            var result = await _customerService.GetCustomerByID(badID);

            // Assert
            Assert.IsNull(result);
            Assert.IsNull(expected);
            Assert.AreEqual(expected, result);
        }
        #endregion GetCustomerByID

        #region SetPurchaseAbilityOfCustomer
        [Test]
        public async Task SetPurchaseAbilityOfCustomer_WithValidID_ShouldReturnCustomer()
        {
            // Arrange
            var expected = FakeCustomersService._customers[1];

            // Act
            var result = await _customerService.SetPurchaseAbilityOfCustomer(expected.ID, expected.CanPurchase);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<CustomerDto>(result);
            Assert.AreEqual(expected.ID, result.ID);
            Assert.AreEqual(expected.FirstName, result.FirstName);
            Assert.AreEqual(expected.LastName, result.LastName);
            Assert.AreEqual(expected.Email, result.Email);
            Assert.AreEqual(expected.Address, result.Address);
            Assert.AreEqual(expected.Postcode, result.Postcode);
            Assert.AreEqual(expected.DOB, result.DOB);
            Assert.AreEqual(expected.LoggedOnAt, result.LoggedOnAt);
            Assert.AreEqual(expected.PhoneNumber, result.PhoneNumber);
            Assert.AreEqual(expected.CanPurchase, result.CanPurchase);
        }

        [Test]
        public async Task SetPurchaseAbilityOfCustomer_WithInvalidID_ShouldReturnNull()
        {
            // Arrange
            var expected = FakeCustomersService.SetPurchaseAbilityOfCustomer(badID, true);

            // Act
            var result = await _customerService.SetPurchaseAbilityOfCustomer(badID, true);

            // Assert
            Assert.IsNull(result);
            Assert.IsNull(expected);
            Assert.AreEqual(expected, result);
        }
        #endregion SetPurchaseAbilityOfCustomer
    }
}