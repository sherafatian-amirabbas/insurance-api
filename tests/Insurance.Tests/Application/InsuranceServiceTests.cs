﻿using Insurance.Api.Controllers;
using Insurance.Application;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Plugins.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Insurance.Tests.Application
{
    public class InsuranceServiceTests
    {
        private readonly Mock<IDataApiProxy> mockDataApiProxy;
        private readonly InsuranceService insuranceService;


        public InsuranceServiceTests()
        {
            mockDataApiProxy = new Mock<IDataApiProxy>();
            insuranceService = new InsuranceService(mockDataApiProxy.Object);
        }


        #region No Insurance Provided

        [Theory]
        [InlineData("Test")]
        [InlineData("Laptops")]
        [InlineData("Smartphones")]
        public async Task CalculateInsurance_InsuranceNotProvidedForTheProductType_InsuranceValueIsZero(string productTypeName)
        {
            // arrange
            const float expectedInsuranceValue = 0;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = productTypeName,
                    SalesPrice = 1000,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = productTypeName,
                    CanBeInsured = false
                }));

            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        #endregion


        #region SalesPrice Less Than 500

        [Fact]
        public async Task CalculateInsurance_SalesPriceIsLessThan500AndProductTypeIsNotLaptopsNorSmartphones_InsuranceValueIs0()
        {
            // arrange
            const float expectedInsuranceValue = 0;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = string.Empty,
                    SalesPrice = 0,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = "Test",
                    CanBeInsured = true
                }));


            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIsLessThan500ButProductTypeIsLaptops_InsuranceValueIs500()
        {
            // arrange
            const float expectedInsuranceValue = 500;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = string.Empty,
                    SalesPrice = 0,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = "Laptops",
                    CanBeInsured = true
                }));

            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIsLessThan500ButProductTypeIsSmartphones_InsuranceValueIs500()
        {
            // arrange
            const float expectedInsuranceValue = 500;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = string.Empty,
                    SalesPrice = 0,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = "Smartphones",
                    CanBeInsured = true
                }));

            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        #endregion


        #region SalesPrice Is 500

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs500AndProductTypeIsNotLaptopsNorSmartphones_InsuranceValueIs1000()
        {
            // arrange
            const float expectedInsuranceValue = 1000;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = string.Empty,
                    SalesPrice = 500,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = "Test",
                    CanBeInsured = true
                }));

            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs500AndProductTypeIsLaptops_InsuranceValueIs1500()
        {
            // arrange
            const float expectedInsuranceValue = 1500;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = string.Empty,
                    SalesPrice = 500,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = "Laptops",
                    CanBeInsured = true
                }));

            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs500AndProductTypeIsSmartphones_InsuranceValueIs1500()
        {
            // arrange
            const float expectedInsuranceValue = 1500;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = string.Empty,
                    SalesPrice = 500,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = "Smartphones",
                    CanBeInsured = true
                }));

            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        #endregion


        #region SalesPrice Is 2000

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs2000AndProductTypeIsNotLaptopsNorSmartphones_InsuranceValueIs2000()
        {
            // arrange
            const float expectedInsuranceValue = 2000;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = string.Empty,
                    SalesPrice = 2000,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = "Test",
                    CanBeInsured = true
                }));

            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs2000AndProductTypeIsLaptops_InsuranceValueIs2500()
        {
            // arrange
            const float expectedInsuranceValue = 2500;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = string.Empty,
                    SalesPrice = 2000,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = "Laptops",
                    CanBeInsured = true
                }));

            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs2000AndProductTypeIsSmartphones_InsuranceValueIs2500()
        {
            // arrange
            const float expectedInsuranceValue = 2500;

            mockDataApiProxy.Setup(u => u.GetProductAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new Product()
                {
                    Id = 1,
                    Name = string.Empty,
                    SalesPrice = 2000,
                    ProductTypeId = 1
                }));

            mockDataApiProxy.Setup(u => u.GetProductTypeAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductType()
                {
                    Id = 1,
                    Name = "Smartphones",
                    CanBeInsured = true
                }));

            // act
            var result = await insuranceService.CalculateInsuranceAsync(1);

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        #endregion
    }
}