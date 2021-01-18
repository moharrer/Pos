using Inventory.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.Domain
{
    class ProductMapping : IEntityTypeConfiguration<Inventory.Domain.Product>
    {
        public void Configure(EntityTypeBuilder<Inventory.Domain.Product> builder)
        {
            builder.ToTable("Invoice", InventoryContext.DEFAULT_SCHEMA);

            builder.HasKey(a => a.Key);

            builder.Property(a => a.Id).IsRequired();
            builder.Property(a => a.Name).IsRequired();
            builder.Property(a => a.Quantity).IsRequired();

        }
    }

}
