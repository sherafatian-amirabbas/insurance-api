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
            var (productIds, insuranceValue) = CalculateInsurance(new List<ProductComplete>() { productComplete });
            return new ProductInsuranceResult(productIds.FirstOrDefault(), insuranceValue);
        }

        public async Task<OrderInsuranceResult> CalculateOrderInsuranceAsync(OrderInsurance orderInsurance)
        {
            var productCompletes = await dataApiProxy.GetProductCompletesAsync(orderInsurance.ProductIds);
            var (productIds, insuranceValue) = CalculateInsurance(productCompletes);
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

        private (List<int> productIds, float insuranceValue) CalculateInsurance(List<ProductComplete> products)
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

                return new
                {
                    productComplete,
                    insuranceValue
                };
            })
            .ToList();

            var productIds = result.Select(u => u.productComplete.ProductId).ToList();
            var insuranceValue = result.Sum(u => u.insuranceValue);
            if (result.Any(u => u.productComplete.ProductType!.Name == ProductType.DIGITAL_CAMERAS))
                insuranceValue += 500;

            LogProductInsuranceResult(productIds, insuranceValue);

            return (productIds, insuranceValue);
        }

        public void LogProductInsuranceResult(List<int> productIds, float insuranceValue)
        {
            var ids = string.Join(", ", productIds);
            logger.LogInformation("CalculateInsurance - InsuranceValue: {InsuranceValue}, ProductId(s): {ProductIds}", 
                ids,
                insuranceValue);
        }

        #endregion
    }
}
