using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class GetFinancingLeadsQueryDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string? SortBy { get; set; } = "CreatedAt";
        public string? SortDir { get; set; } = "desc";

        public SearchTypes? SearchWith { get; set; }
        public string? Search { get; set; }

        public string? PhoneStartsWith { get; set; }

        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public LeadReviewStatus? Status { get; set; } 
    }

}
