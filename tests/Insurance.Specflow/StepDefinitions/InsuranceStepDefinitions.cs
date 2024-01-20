using Insurance.Specflow.Mock;
using Insurance.SpecFlow.Drivers;
using Insurance.Specflow.Utils.Extensions;
using Insurance.Contracts.Application.Models;

namespace Insurance.SpecFlow.StepDefinitions
{
    [Binding]
    public class InsuranceStepDefinitions
    {
        List<int> productIds = new();
        OrderInsuranceResult? orderInsuranceResult;

        private readonly InsuranceDriver insuranceDriver;
        private readonly MockDataApiProxy mockDataApiProxy;


        public InsuranceStepDefinitions(InsuranceDriver insuranceDriver,
            MockDataApiProxy mockDataApiProxy)
        {
            this.insuranceDriver = insuranceDriver;
            this.mockDataApiProxy = mockDataApiProxy;
        }


        #region Background

        [Given(@"the following Products were available")]
        public void GivenTheFollowingProductsWereAvailable(Table table)
        {
            var products = table.Rows
                .Select(u => u.ToProduct())
                .ToList();

            mockDataApiProxy.SetProducts(products);
        }


        [Given(@"the following ProductTypes were available")]
        public void GivenTheFollowingProductTypesWereAvailable(Table table)
        {
            var productTypes = table.Rows
                .Select(u => u.ToProductType())
                .ToList();

            mockDataApiProxy.SetProductTypes(productTypes);
        }

        #endregion


        [Given(@"the following products were available in the order")]
        public void GivenTheFollowingProductsWereAvailableInTheOrder(Table table)
        {
            productIds = table.Rows.Select(u => Convert.ToInt32(u["Id"])).ToList();
        }


        [When(@"insurance is calculated for the order")]
        public async Task WhenInsuranceIsCalculatedForTheOrder()
        {
            orderInsuranceResult = await insuranceDriver.CalculateOrderInsuranceAsync(productIds);
        }


        [Then(@"the insurance value is (.*)")]
        public void ThenTheInsuranceValueIs(int insuranceValue)
        {
            Assert.NotNull(orderInsuranceResult);
            Assert.Equal(insuranceValue, orderInsuranceResult!.InsuranceValue);
        }

    }
}
