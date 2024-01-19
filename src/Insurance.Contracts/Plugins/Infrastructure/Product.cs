namespace Insurance.Contracts.Plugins.Infrastructure
{
    public record Product()
    {
        public int Id { get; set; } = default;
        public string Name { get; set; } = string.Empty;
        public float SalesPrice { get; set; } = default;
        public int ProductTypeId { get; set; } = default;
    }
}
