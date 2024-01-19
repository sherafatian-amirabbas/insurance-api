using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Insurance.Api.Models;
using Insurance.Application;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Plugins.Infrastructure;

namespace Insurance.Tests.Application
{
    public class InsuranceServiceTests
    {
        private readonly Mock<IDataApiProxy> mockDataApiProxy;
        private readonly Mock<ILogger<InsuranceService>> mockLogger;
        private readonly InsuranceService insuranceService;
        

        public InsuranceServiceTests()
        {
            mockDataApiProxy = new Mock<IDataApiProxy>();
            mockLogger = new Mock<ILogger<InsuranceService>>();
            insuranceService = new InsuranceService(mockDataApiProxy.Object, mockLogger.Object);
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

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = productTypeName,
                        SalesPrice = 1000,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = productTypeName,
                        CanBeInsured = false
                    }
                }));

            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

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

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = string.Empty,
                        SalesPrice = 0,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = "Test",
                        CanBeInsured = true
                    }
                }));


            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIsLessThan500ButProductTypeIsLaptops_InsuranceValueIs500()
        {
            // arrange
            const float expectedInsuranceValue = 500;

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = string.Empty,
                        SalesPrice = 0,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = "Laptops",
                        CanBeInsured = true
                    }
                }));

            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIsLessThan500ButProductTypeIsSmartphones_InsuranceValueIs500()
        {
            // arrange
            const float expectedInsuranceValue = 500;

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = string.Empty,
                        SalesPrice = 0,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = "Smartphones",
                        CanBeInsured = true
                    }
                }));

            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

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

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = string.Empty,
                        SalesPrice = 500,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = "Test",
                        CanBeInsured = true
                    }
                }));

            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs500AndProductTypeIsLaptops_InsuranceValueIs1500()
        {
            // arrange
            const float expectedInsuranceValue = 1500;

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = string.Empty,
                        SalesPrice = 500,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = "Laptops",
                        CanBeInsured = true
                    }
                }));

            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs500AndProductTypeIsSmartphones_InsuranceValueIs1500()
        {
            // arrange
            const float expectedInsuranceValue = 1500;

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = string.Empty,
                        SalesPrice = 500,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = "Smartphones",
                        CanBeInsured = true
                    }
                }));

            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

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

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = string.Empty,
                        SalesPrice = 2000,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = "Test",
                        CanBeInsured = true
                    }
                }));

            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs2000AndProductTypeIsLaptops_InsuranceValueIs2500()
        {
            // arrange
            const float expectedInsuranceValue = 2500;

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = string.Empty,
                        SalesPrice = 2000,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = "Laptops",
                        CanBeInsured = true
                    }
                }));

            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateInsurance_SalesPriceIs2000AndProductTypeIsSmartphones_InsuranceValueIs2500()
        {
            // arrange
            const float expectedInsuranceValue = 2500;

            mockDataApiProxy.Setup(u => u.GetProductCompleteAsync(It.IsAny<int>()))
                .Returns(() => Task.FromResult(new ProductComplete(1)
                {
                    Product = new Product()
                    {
                        Id = 1,
                        Name = string.Empty,
                        SalesPrice = 2000,
                        ProductTypeId = 1
                    },
                    ProductType = new ProductType()
                    {
                        Id = 1,
                        Name = "Smartphones",
                        CanBeInsured = true
                    }
                }));

            // act
            var result = await insuranceService.CalculateProductInsuranceAsync(new ProductInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        #endregion
    }
}
