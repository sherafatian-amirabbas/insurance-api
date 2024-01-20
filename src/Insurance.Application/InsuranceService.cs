using System.Net;
using Insurance.Contracts.Application.Exceptions;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Application.Models;
using Insurance.Contracts.Plugins.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Insurance.Application
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IDataApiProxy dataApiProxy;
        private readonly ILogger<InsuranceService> logger;


        public InsuranceService(IDataApiProxy dataApiProxy,
            ILogger<InsuranceService> logger)
        {
            this.dataApiProxy = dataApiProxy;
            this.logger = logger;
        }


        #region Public Methods

        /// <summary>
        /// Calculates insurance
        /// </summary>
        public async Task<ProductInsuranceResult> CalculateProductInsuranceAsync(ProductInsurance productInsurance)
        {
            var productComplete = await dataApiProxy.GetProductCompleteAsync(productInsurance.ProductId);
            var result = CalculateInsurance(new List<ProductComplete>() { productComplete });
            return result.FirstOrDefault()!;
        }

        public async Task<OrderInsuranceResult> CalculateOrderInsuranceAsync(OrderInsurance orderInsurance)
        {
            var productCompletes = await dataApiProxy.GetProductCompletesAsync(orderInsurance.ProductIds);

            var insuranceValues = CalculateInsurance(productCompletes);

            var insuranceValue = insuranceValues.Sum(u => u.InsuranceValue);
            var productIds = insuranceValues.Select(u => u.ProductId).ToList();
            return new OrderInsuranceResult(productIds, insuranceValue);
        }

        #endregion


        #region Private Methods

        private void ValidateProducts(List<ProductComplete> products)
        {
            var invalidIds = products.Where(u => u.Product == null)
                .Select(u => u.ProductId)
                .ToList();
            if (invalidIds.Any())
            {
                var productIds = string.Join(", ", invalidIds);
                throw new BusinessException($"Product(s) couldn't be found! ProductId(s): {productIds}",
                    HttpStatusCode.BadRequest);
            }

            var invalidTypeIds = products.Where(u => u.ProductType == null)
                .Select(u => u.Product!.ProductTypeId)
                .ToList();
            if (invalidTypeIds.Any())
            {
                var productIds = string.Join(", ", invalidTypeIds);
                throw new TechnicalException($"ProductType(s) couldn't be found! ProductTypeId(s): {productIds}");
            }
        }

        private List<ProductInsuranceResult> CalculateInsurance(List<ProductComplete> products)
        {
            ValidateProducts(products);

            var result = products.Select(productComplete =>
            {
                float insuranceValue = 0f;
                if (productComplete.ProductType!.CanBeInsured)
                {
                    if (productComplete.Product!.SalesPrice >= 500)
                        insuranceValue = productComplete.Product!.SalesPrice < 2000 ? 1000 : 2000;

                    if (productComplete.ProductType!.Name == ProductType.LAPTOPS ||
                        productComplete.ProductType!.Name == ProductType.SMARTPHONES)
                        insuranceValue += 500;
                }

                return new ProductInsuranceResult(productComplete.Product!.Id, insuranceValue);
            })
            .ToList();

            LogProductInsuranceResult(result);

            return result;
        }

        public void LogProductInsuranceResult(List<ProductInsuranceResult> result)
        {
            var infoItems = result
                .Select(u => $"productId: {u.ProductId}, InsuranceValue: {u.InsuranceValue}")
                .ToList();
            var info = string.Join(" - ", infoItems);
            logger.LogInformation("CalculateInsurance - {info}", info);
        }

        #endregion
    }
}
