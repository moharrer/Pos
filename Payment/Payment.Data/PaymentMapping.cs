using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Domain
{
    class PaymentMapping : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable(nameof(Payment), PaymentContext.DEFAULT_SCHEMA);
            
            builder.HasKey(a => a.Key);
            
            builder.Property(a => a.Id).IsRequired();
            builder.Property(a => a.InvoiceId).IsRequired();
            builder.Property(a => a.Amount).IsRequired();
        }
    }
}
