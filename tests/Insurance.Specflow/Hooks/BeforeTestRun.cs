using BoDi;
using Insurance.Specflow.Mock;
using Insurance.Specflow.Support;

namespace Insurance.SpecFlow.Hooks
{
    [Binding]
    public static class BeforeTestRun
    {
        [BeforeTestRun]
        public static void Execute(IObjectContainer container)
        {
            // mock is registered to the container for scenarios
            var mockDataApiProxy = new MockDataApiProxy();
            container.RegisterInstanceAs(mockDataApiProxy);

            // also we apply it to the Insurance.Api assembly
            container.RegisterInstanceAs(new ApplicationFactory(mockDataApiProxy));
        }
    }
}
