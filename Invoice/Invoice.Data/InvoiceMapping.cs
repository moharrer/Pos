using Invoice.Data;
using Invoice.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.Domain
{
    class InvoiceMapping : IEntityTypeConfiguration<Invoice.Domain.Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice.Domain.Invoice> builder)
        {
            builder.ToTable("Invoice", InvoiceContext.DEFAULT_SCHEMA);

            builder.HasKey(a => a.Key);

            builder.Property(a => a.Id).IsRequired();
            builder.Property(a => a.PaidAmount).IsRequired();
            builder.Property(a => a.TotalAmount).IsRequired();

            builder.HasMany(a => a.InvoiceItemLines).WithOne(a => a.Invoice).HasForeignKey(a => a.InvoiceId);
        }
    }

    class InvoiceItemLineMapping : IEntityTypeConfiguration<InvoiceItemLine>
    {
        public void Configure(EntityTypeBuilder<InvoiceItemLine> builder)
        {
            builder.ToTable(nameof(InvoiceItemLine), InvoiceContext.DEFAULT_SCHEMA);

            builder.HasKey(a => a.Key);

            builder.Property(a => a.Id).IsRequired();
            builder.Property(a => a.Name).IsRequired();
            builder.Property(a => a.Quantity).IsRequired();
            builder.Property(a => a.Type).IsRequired();

            builder.HasOne(a => a.Invoice);
        }
    }
}
