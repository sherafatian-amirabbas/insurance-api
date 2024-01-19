using System.Net;
using Insurance.Contracts.Application.Exceptions;
using Insurance.Contracts.Application.Interfaces;
using Insurance.Contracts.Application.Models;
using Insurance.Contracts.Plugins.Infrastructure;


namespace Insurance.Application
{
    public class InsuranceService : IInsuranceService
    {
        private readonly IDataApiProxy dataApiProxy;


        public InsuranceService(IDataApiProxy dataApiProxy)
        {
            this.dataApiProxy = dataApiProxy;
        }


        #region Public Methods

        /// <summary>
        /// Calculates insurance
        /// </summary>
        public async Task<ProductInsurance> CalculateInsuranceAsync(int productId)
        {
            var product = await dataApiProxy.GetProductAsync(productId);
            if (product == null)
                throw new BusinessException($"Product couldn't be found! ProductId: {productId}",
                    HttpStatusCode.BadRequest);

            var productType = await dataApiProxy.GetProductTypeAsync(product.ProductTypeId);
            if (productType == null)
                throw new TechnicalException($"ProductType couldn't be found! ProductTypeId: {product.ProductTypeId}");

            float insurance = 0f;
            if (productType.CanBeInsured)
            {
                if (product.SalesPrice >= 500)
                    insurance = product.SalesPrice < 2000 ? 1000 : 2000;

                if (productType.Name == ProductType.LAPTOPS || productType.Name == ProductType.SMARTPHONES)
                    insurance += 500;
            }

            return new ProductInsurance(productId, insurance);
        }

        #endregion
    }
}
