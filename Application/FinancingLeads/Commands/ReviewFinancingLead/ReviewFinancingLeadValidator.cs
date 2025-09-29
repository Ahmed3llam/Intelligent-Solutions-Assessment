using Application.DTOs;
using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Commands.ReviewFinancingLead
{
    public class ReviewFinancingLeadValidator : AbstractValidator<ReviewRequest>
    {
        public ReviewFinancingLeadValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty().WithMessage("ID is required");

            RuleFor(x => x.Status)
                .IsInEnum()
                .NotEqual(LeadReviewStatus.Pending).WithMessage("Status should be Accepted or Rejected");

            RuleFor(x => x.Reason)
                .MaximumLength(250);
        }
    }
}
