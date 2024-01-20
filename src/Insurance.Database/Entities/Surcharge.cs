using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Insurance.Database.Entities
{
    public class Surcharge : IEntityTypeConfiguration<Surcharge>
    {
        public int ProductTypeId { get; set; }
        public int PercentRate { get; set; }


        public void Configure(EntityTypeBuilder<Surcharge> builder)
        {
            builder.HasKey(u => u.ProductTypeId);
        }
    }
}
