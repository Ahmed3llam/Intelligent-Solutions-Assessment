using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;
using Domain.Events;

namespace Domain.Entities
{
    public class FinancingLead
    {
        public Guid Id { get; private set; }

        #region Contact
        public string? Name { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneE164 { get; private set; }
        public PreferredContactMethod? PreferredContactMethod { get; private set; }
        #endregion

        #region Business
        public string? TypeOfActivity { get; private set; }
        public string? CommercialRegisterType { get; private set; }
        public decimal? AnnualIncome { get; private set; }
        #endregion

        public string? Notes { get; private set; }

        public DateTime CreatedAt { get; private set; }

        #region Review
        public DateTime? ReviewedAt { get; private set; }
        public LeadReviewStatus Status { get; private set; } = LeadReviewStatus.Pending;
        public string? ReviewReason { get; private set; }
        #endregion

        public byte[] RowVersion { get; private set; }


        private readonly List<object> _events = new List<object>();
        public IReadOnlyCollection<object> Events => _events.AsReadOnly();


        public void ClearEvents() => _events.Clear();

        private FinancingLead() { }

        public static FinancingLead Create(
            string? name,
            string? email,
            string? phoneE164,
            string? typeOfActivity,
            string? commercialRegisterType,
            PreferredContactMethod? preferredContactMethod = null,
            decimal? annualIncome = null,
            string? notes = null)
        {
            var lead = new FinancingLead
            {
                Id = Guid.NewGuid(),
                Name = name?.Trim(),
                Email = email?.Trim().ToLowerInvariant(),
                PhoneE164 = phoneE164?.Trim(),
                TypeOfActivity = typeOfActivity?.Trim(),
                CommercialRegisterType = commercialRegisterType?.Trim(),
                PreferredContactMethod = preferredContactMethod,
                AnnualIncome = annualIncome,
                Notes = notes?.Trim(),
                CreatedAt = DateTime.UtcNow,
                Status = LeadReviewStatus.Pending
            };
            return lead;
        }

        public void Review(LeadReviewStatus newStatus, string? reason)
        {
            if (newStatus == Status) return;

            Status = newStatus;
            ReviewReason = reason?.Trim();
            ReviewedAt = DateTime.UtcNow;

            if (newStatus == LeadReviewStatus.Accepted)
            {
                var evt = new LeadReviewedDomainEvent(Id, PhoneE164, Status, ReviewedAt.Value);
                _events.Add(evt);
            }
        }
    }
}
