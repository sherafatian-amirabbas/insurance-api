using Insurance.Contracts.Plugins.Infrastructure;

namespace Insurance.Specflow.Utils.Extensions
{
    public static class TableRowExtensions
    {
        public static Product ToProduct(this TableRow tableRow)
        {
            return new Product()
            {
                Id = Convert.ToInt32(tableRow["Id"]),
                Name = tableRow["Name"],
                SalesPrice = Convert.ToInt32(tableRow["SalesPrice"]),
                ProductTypeId = Convert.ToInt32(tableRow["ProductTypeId"])
            };
        }

        public static ProductType ToProductType(this TableRow tableRow)
        {
            return new ProductType()
            {
                Id = Convert.ToInt32(tableRow["Id"]),
                Name = tableRow["Name"],
                CanBeInsured = Convert.ToBoolean(tableRow["CanBeInsured"])
            };
        }
    }
}
