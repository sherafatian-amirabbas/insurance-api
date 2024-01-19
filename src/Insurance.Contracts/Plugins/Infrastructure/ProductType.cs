namespace Insurance.Contracts.Plugins.Infrastructure
{
    public record ProductType()
    {
        public const string LAPTOPS = "Laptops";
        public const string SMARTPHONES = "Smartphones";

        public int Id { get; set; } = default;
        public string Name { get; set; } = string.Empty;
        public bool CanBeInsured { get; set; } = default;
    }
}
