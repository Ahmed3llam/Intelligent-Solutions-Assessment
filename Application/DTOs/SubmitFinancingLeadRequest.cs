using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class SubmitFinancingLeadRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneE164 { get; set; }
        public PreferredContactMethod? PreferredContactMethod { get; set; }
        public string? TypeOfActivity { get; set; }
        public string? CommercialRegisterType { get; set; }
        public decimal? AnnualIncome { get; set; }
        public string? Notes { get; set; }
    }
}
