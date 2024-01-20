using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Insurance.Contracts.Application.Models;
using Insurance.Application;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Plugins.Infrastructure;
using System.Collections.Generic;
using Insurance.Contracts.Plugins.Database;

namespace Insurance.Tests.Application
{
    public class InsuranceServiceTests
    {
        private readonly Mock<IDataApiProxy> mockDataApiProxy;
        private readonly Mock<ILogger<InsuranceService>> mockLogger;
        private readonly Mock<ISurchargeService> mockSurchargeService;
        private readonly InsuranceService insuranceService;


        public InsuranceServiceTests()
        {
            mockDataApiProxy = new Mock<IDataApiProxy>();
            mockLogger = new Mock<ILogger<InsuranceService>>();

            mockSurchargeService = new Mock<ISurchargeService>();
            mockSurchargeService.Setup(u => u.GetSurchargesAsync(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new List<Surcharge>()));

            insuranceService = new InsuranceService(mockDataApiProxy.Object,
                mockLogger.Object, mockSurchargeService.Object);
        }


        #region No Insurance Provided

        [Theory]
        [InlineData("Test")]
        [InlineData("Laptops")]
        [InlineData("Smartphones")]
        public async Task CalculateProductInsuranceAsync_InsuranceNotProvidedForTheProductType_InsuranceValueIsZero(string productTypeName)
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
        public async Task CalculateProductInsuranceAsync_SalesPriceIsLessThan500AndProductTypeIsNotLaptopsNorSmartphones_InsuranceValueIs0()
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
        public async Task CalculateProductInsuranceAsync_SalesPriceIsLessThan500ButProductTypeIsLaptops_InsuranceValueIs500()
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
        public async Task CalculateProductInsuranceAsync_SalesPriceIsLessThan500ButProductTypeIsSmartphones_InsuranceValueIs500()
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
        public async Task CalculateProductInsuranceAsync_SalesPriceIs500AndProductTypeIsNotLaptopsNorSmartphones_InsuranceValueIs1000()
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
        public async Task CalculateProductInsuranceAsync_SalesPriceIs500AndProductTypeIsLaptops_InsuranceValueIs1500()
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
        public async Task CalculateProductInsuranceAsync_SalesPriceIs500AndProductTypeIsSmartphones_InsuranceValueIs1500()
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
        public async Task CalculateProductInsuranceAsync_SalesPriceIs2000AndProductTypeIsNotLaptopsNorSmartphones_InsuranceValueIs2000()
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
        public async Task CalculateProductInsuranceAsync_SalesPriceIs2000AndProductTypeIsLaptops_InsuranceValueIs2500()
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
        public async Task CalculateProductInsuranceAsync_SalesPriceIs2000AndProductTypeIsSmartphones_InsuranceValueIs2500()
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


        #region Surcharge

        [Fact]
        public async Task CalculateOrderInsuranceAsync_SurchargeRateDefinedForOneProductTypeAndThereIsOnlyOneProductOfThatType_ItIsAddedToInsuranceValue()
        {
            // arrange
            const float expectedInsuranceValue = 2200;

            mockSurchargeService.Setup(u => u.GetSurchargesAsync(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new List<Surcharge>()
                {
                    new Surcharge()
                    {
                        ProductTypeId = 1,
                        PercentRate = 10
                    }
                }));

            mockDataApiProxy.Setup(u => u.GetProductCompletesAsync(It.IsAny<List<int>>()))
                .Returns(() => Task.FromResult(new List<ProductComplete>()
                {
                    new ProductComplete(1)
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
                    }
                }));

            // act
            var result = await insuranceService.CalculateOrderInsuranceAsync(new OrderInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateOrderInsuranceAsync_SurchargeRateDefinedForOneProductTypeAndThereAreTwoProductsOfThatType_ItIsAddedToInsuranceValue()
        {
            // arrange
            const float expectedInsuranceValue = 3300;

            mockSurchargeService.Setup(u => u.GetSurchargesAsync(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new List<Surcharge>()
                {
                    new Surcharge()
                    {
                        ProductTypeId = 1,
                        PercentRate = 10
                    }
                }));

            mockDataApiProxy.Setup(u => u.GetProductCompletesAsync(It.IsAny<List<int>>()))
                .Returns(() => Task.FromResult(new List<ProductComplete>()
                {
                    new ProductComplete(1)
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
                    },
                    new ProductComplete(1)
                    {
                        Product = new Product()
                        {
                            Id = 2,
                            Name = string.Empty,
                            SalesPrice = 1000,
                            ProductTypeId = 1
                        },
                        ProductType = new ProductType()
                        {
                            Id = 1,
                            Name = "Test",
                            CanBeInsured = true
                        }
                    }
                }));

            // act
            var result = await insuranceService.CalculateOrderInsuranceAsync(new OrderInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        [Fact]
        public async Task CalculateOrderInsuranceAsync_SurchargeRateDefinedForOneProductTypeAndThereAreThreeProductOfDifferentProductType_ItIsAddedToInsuranceValue()
        {
            // arrange
            const float expectedInsuranceValue = 4300;

            mockSurchargeService.Setup(u => u.GetSurchargesAsync(It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new List<Surcharge>()
                {
                    new Surcharge()
                    {
                        ProductTypeId = 1,
                        PercentRate = 10
                    }
                }));

            mockDataApiProxy.Setup(u => u.GetProductCompletesAsync(It.IsAny<List<int>>()))
                .Returns(() => Task.FromResult(new List<ProductComplete>()
                {
                    new ProductComplete(1)
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
                    },
                    new ProductComplete(1)
                    {
                        Product = new Product()
                        {
                            Id = 2,
                            Name = string.Empty,
                            SalesPrice = 1000,
                            ProductTypeId = 1
                        },
                        ProductType = new ProductType()
                        {
                            Id = 1,
                            Name = "Test",
                            CanBeInsured = true
                        }
                    },
                    new ProductComplete(1)
                    {
                        Product = new Product()
                        {
                            Id = 3,
                            Name = string.Empty,
                            SalesPrice = 500,
                            ProductTypeId = 2
                        },
                        ProductType = new ProductType()
                        {
                            Id = 2,
                            Name = "Test",
                            CanBeInsured = true
                        }
                    }
                }));

            // act
            var result = await insuranceService.CalculateOrderInsuranceAsync(new OrderInsurance());

            // assert
            Assert.Equal(expectedInsuranceValue, result.InsuranceValue);
        }

        #endregion
    }
}
