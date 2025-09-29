using Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Queries.GetFinancingLeads
{
    public class GetFinancingLeadsQueryValidator : AbstractValidator<GetFinancingLeadsQueryDto>
    {
        public GetFinancingLeadsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("PageSize must be between 1 and 100");

            RuleFor(x => x.SortBy)
                .Must(s => string.IsNullOrWhiteSpace(s) || new[] { "CreatedAt", "Name", "Status" }
                .Contains(s, StringComparer.OrdinalIgnoreCase))
                .WithMessage("SortBy must be 'CreatedAt', 'Name', or 'Status'.");
        }
    }
}
