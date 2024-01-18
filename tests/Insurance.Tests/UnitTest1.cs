using System;
using System.Collections.Generic;
using System.Linq;
using Insurance.Api.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Xunit;

namespace Insurance.Tests
{
    public class InsuranceTests : IClassFixture<ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;

        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        #region No Insurance Provided

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void CalculateInsurance_InsuranceNotProvidedForTheProductType_InsuranceValueIsZero(int productId)
        {
            // arrange
            const float expectedInsuranceValue = 0;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = productId,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        #endregion


        #region SalesPrice Less Than 500

        [Fact]
        public void CalculateInsurance_SalesPriceIsLessThan500AndProductTypeIsNotLaptopsNorSmartphones_InsuranceValueIs0()
        {
            // arrange
            const float expectedInsuranceValue = 0;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = 5,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_SalesPriceIsLessThan500ButProductTypeIsLaptops_InsuranceValueIs500()
        {
            // arrange
            const float expectedInsuranceValue = 500;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = 6,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_SalesPriceIsLessThan500ButProductTypeIsSmartphones_InsuranceValueIs500()
        {
            // arrange
            const float expectedInsuranceValue = 500;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = 7,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        #endregion


        #region SalesPrice Is 500

        [Fact]
        public void CalculateInsurance_SalesPriceIs500AndProductTypeIsNotLaptopsNorSmartphones_InsuranceValueIs1000()
        {
            // arrange
            const float expectedInsuranceValue = 1000;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = 8,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_SalesPriceIs500AndProductTypeIsLaptops_InsuranceValueIs1500()
        {
            // arrange
            const float expectedInsuranceValue = 1500;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = 9,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_SalesPriceIs500AndProductTypeIsSmartphones_InsuranceValueIs1500()
        {
            // arrange
            const float expectedInsuranceValue = 1500;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = 10,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        #endregion


        #region SalesPrice Is 2000

        [Fact]
        public void CalculateInsurance_SalesPriceIs2000AndProductTypeIsNotLaptopsNorSmartphones_InsuranceValueIs2000()
        {
            // arrange
            const float expectedInsuranceValue = 2000;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = 11,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_SalesPriceIs2000AndProductTypeIsLaptops_InsuranceValueIs2500()
        {
            // arrange
            const float expectedInsuranceValue = 2500;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = 12,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        [Fact]
        public void CalculateInsurance_SalesPriceIs2000AndProductTypeIsSmartphones_InsuranceValueIs2500()
        {
            // arrange
            const float expectedInsuranceValue = 2500;
            var dto = new HomeController.InsuranceDto
            {
                ProductId = 13,
            };
            var sut = new HomeController();

            // act
            var result = sut.CalculateInsurance(dto);

            // assert
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }

        #endregion


        [Fact]
        public void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            const float expectedInsuranceValue = 1000;

            var dto = new HomeController.InsuranceDto
            {
                ProductId = 1,
            };
            var sut = new HomeController();

            var result = sut.CalculateInsurance(dto);

            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: result.InsuranceValue
            );
        }
    }

    public class ControllerTestFixture : IDisposable
    {
        private readonly IHost _host;

        public ControllerTestFixture()
        {
            _host = new HostBuilder()
                   .ConfigureWebHostDefaults(
                        b => b.UseUrls("http://localhost:5002")
                              .UseStartup<ControllerTestStartup>()
                    )
                   .Build();

            _host.Start();
        }

        public void Dispose() => _host.Dispose();
    }

    public class ControllerTestStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/{id:int}",
                        context =>
                        {
                            int productId = int.Parse((string)context.Request.RouteValues["id"]);
                            var product = GetProducts().FirstOrDefault(u => u.id == productId);
                            var response = JsonConvert.SerializeObject(product) as string;
                            return context.Response.WriteAsync(response);
                        }
                    );
                    ep.MapGet(
                        "product_types",
                        context =>
                        {
                            var productTypes = new[]
                            {
                                new
                                {
                                    id = 1,
                                    name = "Test type 1",
                                    canBeInsured = true
                                },
                                new
                                {
                                    id = 2,
                                    name = "Test type 2",
                                    canBeInsured = false
                                },
                                new
                                {
                                    id = 22,
                                    name = "Laptops",
                                    canBeInsured = false
                                },
                                new
                                {
                                    id = 222,
                                    name = "Smartphones",
                                    canBeInsured = false
                                },
                                new
                                {
                                    id = 3,
                                    name = "Laptops",
                                    canBeInsured = true
                                },
                                new
                                {
                                    id = 4,
                                    name = "Smartphones",
                                    canBeInsured = true
                                }
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productTypes));
                        }
                    );
                }
            );
        }

        public List<dynamic> GetProducts()
        {
            return new List<dynamic>()
            {
                new
                {
                    id = 1,
                    name = "Test Product",
                    productTypeId = 1,
                    salesPrice = 750
                },
                new
                {
                    id = 2,
                    name = "Test Product",
                    productTypeId = 2,
                    salesPrice = 1000
                },
                new
                {
                    id = 3,
                    name = "Test Product",
                    productTypeId = 22,
                    salesPrice = 1000
                },
                new
                {
                    id = 4,
                    name = "Test Product",
                    productTypeId = 222,
                    salesPrice = 1000
                },
                new
                {
                    id = 5,
                    name = "Test Product",
                    productTypeId = 1,
                    salesPrice = 0
                },
                new
                {
                    id = 6,
                    name = "Test Product",
                    productTypeId = 3,
                    salesPrice = 0
                },
                new
                {
                    id = 7,
                    name = "Test Product",
                    productTypeId = 4,
                    salesPrice = 0
                },
                new
                {
                    id = 8,
                    name = "Test Product",
                    productTypeId = 1,
                    salesPrice = 500
                },
                new
                {
                    id = 9,
                    name = "Test Product",
                    productTypeId = 3,
                    salesPrice = 500
                },
                new
                {
                    id = 10,
                    name = "Test Product",
                    productTypeId = 4,
                    salesPrice = 500
                },
                new
                {
                    id = 11,
                    name = "Test Product",
                    productTypeId = 1,
                    salesPrice = 2000
                },
                new
                {
                    id = 12,
                    name = "Test Product",
                    productTypeId = 3,
                    salesPrice = 2000
                },
                new
                {
                    id = 13,
                    name = "Test Product",
                    productTypeId = 4,
                    salesPrice = 2000
                }
            };
        }
    }
}