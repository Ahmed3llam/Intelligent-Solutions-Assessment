using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mapping
{
    public class FinancingLeadConfiguration : IEntityTypeConfiguration<FinancingLead>
    {
        public void Configure(EntityTypeBuilder<FinancingLead> builder)
        {
            //builder.ToTable("FinancingLeads");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.RowVersion)
                .IsRowVersion();

            // Contact
            builder.Property(l => l.Name).HasMaxLength(100).IsRequired();
            builder.Property(l => l.Email).HasMaxLength(254).IsRequired();
            builder.Property(l => l.PhoneE164).HasMaxLength(16).IsRequired();

            // Business
            builder.Property(l => l.TypeOfActivity).HasMaxLength(100).IsRequired();
            builder.Property(l => l.CommercialRegisterType).HasMaxLength(50).IsRequired();
            builder.Property(l => l.AnnualIncome).HasPrecision(18, 2);

            // Notes
            builder.Property(l => l.Notes).HasMaxLength(500);

            builder.Property(l => l.CreatedAt).IsRequired();
            builder.Property(l => l.ReviewedAt);

            // Review
            builder.Property(l => l.Status).HasConversion<string>().HasMaxLength(20);
            builder.Property(l => l.ReviewReason).HasMaxLength(250);

            // Indexes (for efficient lookup and sorting)
            //builder.HasIndex(l => l.CreatedAt).IsDescending(true).HasDatabaseName("IX_Lead_CreatedAt_Desc");
            //builder.HasIndex(l => l.PhoneE164).IsUnique();
            //builder.HasIndex(l => l.Email).IsUnique(); 
            //builder.HasIndex(l => new { l.Name, l.Email, l.PhoneE164 }).HasDatabaseName("IX_Lead_ContactComposite");

            // Ignore domain events collection from persistence
            builder.Ignore(l => l.Events);
        }
    }
}