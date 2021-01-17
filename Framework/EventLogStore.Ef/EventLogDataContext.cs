using EventBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventLogStore.Ef
{
    public class EventLogDataContext:DbContext
    {
        public EventLogDataContext(DbContextOptions<EventLogDataContext> options) : base(options)
        {

        }

        public DbSet<EventLog> EventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EventLog>(ConfigureEventLog);
        }

        void ConfigureEventLog(EntityTypeBuilder<EventLog> builder)
        {
            builder.ToTable("EventLog");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();

        }
    }
}
