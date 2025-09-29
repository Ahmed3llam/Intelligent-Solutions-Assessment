using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class FinancingLeadDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public string? Email { get; set; }
        public string? PhoneE164 { get; set; }
        public string? PreferredContactMethod { get; set; }

        public string? TypeOfActivity { get; set; }
        public string? CommercialRegisterType { get; set; }
        public decimal? AnnualIncome { get; set; }

        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }
        public string? Status { get; set; }
        public string? ReviewReason { get; set; }
    }
}
