namespace Insurance.Contracts.Plugins.Infrastructure
{
    public record ProductComplete(int ProductId)
    {
        public Product? Product { get; set; } = null!;
        public ProductType? ProductType { get; set; } = null!;
    }
}
