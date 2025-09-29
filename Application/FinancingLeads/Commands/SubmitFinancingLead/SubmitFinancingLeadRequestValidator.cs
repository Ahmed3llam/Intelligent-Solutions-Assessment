using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Commands.SubmitFinancingLead
{
    public class SubmitFinancingLeadRequestValidator : AbstractValidator<SubmitFinancingLeadRequest>
    {
        public SubmitFinancingLeadRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(254);

            RuleFor(x => x.PhoneE164)
                .NotEmpty().WithMessage("Phone is required")
                .Matches(@"^\+\d{7,15}$").WithMessage("Phone must start with + and contain 7–15 digits")
                .MaximumLength(16);

            RuleFor(x => x.TypeOfActivity)
                .NotEmpty().WithMessage("Type of activity is required")
                .MaximumLength(100);

            RuleFor(x => x.CommercialRegisterType)
                .NotEmpty().WithMessage("Commercial register type is required")
                .MaximumLength(50);

            RuleFor(x => x.AnnualIncome)
                .GreaterThanOrEqualTo(0).WithMessage("Annual income cannot be negative")
                .LessThanOrEqualTo(1_000_000_000).WithMessage("Annual income exceeds allowed maximum");

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters");

        }
    }
}
