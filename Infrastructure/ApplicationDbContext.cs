using Domain.Entities;
using Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<FinancingLead> FinancingLeads {  get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Apply configurations defined in Infrastructure (like FinancingLeadConfiguration)
            //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.ApplyConfiguration(new FinancingLeadConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
