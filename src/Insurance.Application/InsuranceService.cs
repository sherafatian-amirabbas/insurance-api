using System.Net;
using Insurance.Common.Extensions;
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
        private readonly ISurchargeService surchargeService;

        public InsuranceService(IDataApiProxy dataApiProxy,
            ILogger<InsuranceService> logger,
            ISurchargeService surchargeService)
        {
            this.dataApiProxy = dataApiProxy;
            this.logger = logger;
            this.surchargeService = surchargeService;
        }


        #region Public Methods

        /// <summary>
        /// Calculates insurance for a product
        /// </summary>
        public async Task<ProductInsuranceResult> CalculateProductInsuranceAsync(ProductInsurance productInsurance)
        {
            var productComplete = await dataApiProxy.GetProductCompleteAsync(productInsurance.ProductId);
            var (productIds, insuranceValue) = await CalculateInsuranceAsync(new List<ProductComplete>() { productComplete });
            return new ProductInsuranceResult(productIds.FirstOrDefault(), insuranceValue);
        }

        /// <summary>
        /// Calculates insurance for list of products
        /// </summary>
        public async Task<OrderInsuranceResult> CalculateOrderInsuranceAsync(OrderInsurance orderInsurance)
        {
            var productCompletes = await dataApiProxy.GetProductCompletesAsync(orderInsurance.ProductIds);
            var (productIds, insuranceValue) = await CalculateInsuranceAsync(productCompletes);
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

        private async Task<(List<int> productIds, float insuranceValue)> CalculateInsuranceAsync(List<ProductComplete> products)
        {
            ValidateProducts(products);


            #region list of insurance value per product

            var productInsuranceValues = products
                .Select(productComplete =>
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

            #endregion


            #region list of insurance value per product type

            var productTypeInsuranceValues = productInsuranceValues
                .GroupBy(u => new
                {
                    u.productComplete.ProductType!.Id,
                    u.productComplete.ProductType!.Name
                })
                .Select(u =>
                {
                    var insuranceValue = u.Sum(t => t.insuranceValue);
                    if (u.Key.Name == ProductType.DIGITAL_CAMERAS) // if type is 'Digital cameras' 500 added
                        insuranceValue += 500;

                    return new
                    {
                        productTypeId = u.Key.Id,
                        productTypeName = u.Key.Name,
                        insuranceValue,
                        productIds = u.Select(t => t.productComplete.ProductId).ToList()
                    };
                })
                .ToList();

            #endregion


            #region applying surcharge rates

            var productTypeIds = productTypeInsuranceValues
                .Where(u => u.insuranceValue != 0)
                .Select(u => u.productTypeId)
                .ToList();

            var surcharges = await surchargeService.GetSurchargesAsync(productTypeIds);

            var insuranceValues = productTypeInsuranceValues
                .LeftJoin(surcharges,
                    productTypeInsuranceValue => productTypeInsuranceValue.productTypeId,
                    surcharge => surcharge.ProductTypeId,
                    (productTypeInsuranceValue, surcharge) =>
                    {
                        var insuranceValue = productTypeInsuranceValue.insuranceValue;
                        if (insuranceValue != 0 && surcharge != null)
                            insuranceValue += (insuranceValue * surcharge!.PercentRate) / 100;

                        return new
                        {
                            productTypeInsuranceValue.productIds,
                            insuranceValue,
                        };
                    })
                .ToList();

            #endregion


            var productIds = insuranceValues.SelectMany(u => u.productIds).ToList();
            var totalInsuranceValue = insuranceValues.Sum(u => u.insuranceValue);

            LogProductInsuranceResult(productIds, totalInsuranceValue);

            return (productIds, totalInsuranceValue);
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
