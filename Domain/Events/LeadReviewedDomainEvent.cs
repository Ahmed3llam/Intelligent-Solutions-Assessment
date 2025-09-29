using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class LeadReviewedDomainEvent
    {
        public Guid LeadId { get; }
        public string PhoneE164 { get; }
        public LeadReviewStatus Status { get; }
        public DateTime ReviewedAt { get; }

        public LeadReviewedDomainEvent(Guid leadId,  string phoneE164, LeadReviewStatus status, DateTime reviewedAt)
        {
            LeadId = leadId;
            PhoneE164 = phoneE164;
            Status = status;
            ReviewedAt = reviewedAt;
        }
    }
}
