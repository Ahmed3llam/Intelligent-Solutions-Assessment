using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ReviewRequest
    {
        public Guid ID { get; set; }
        public LeadReviewStatus Status { get; set; } = LeadReviewStatus.Pending;
        public string? Reason { get; set; }
    }

}
