using Application.Common.Interfaces.RepositoryInterfaces;
using Application.DTOs;
using Application.FinancingLeads.Queries.GetFinancingLeadById;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Queries.GetFinancingLeads
{
    public class GetFinancingLeadsHandler : IRequestHandler<GetFinancingLeadsQuery, PagedResultDTO>
    {
        private readonly IFinancingLeadRepository _repository;
        public GetFinancingLeadsHandler(IFinancingLeadRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedResultDTO> Handle(GetFinancingLeadsQuery request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetLeadsAsync(request.Data);

            var items = result.Leads.Select(x => new FinancingLeadDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                PhoneE164 = x.PhoneE164,
                PreferredContactMethod = x.PreferredContactMethod.ToString(),
                TypeOfActivity = x.TypeOfActivity,
                CommercialRegisterType = x.CommercialRegisterType,
                AnnualIncome = x.AnnualIncome,
                Notes = x.Notes,
                CreatedAt = x.CreatedAt,
                ReviewedAt = x.ReviewedAt,
                Status = x.Status.ToString(),
                ReviewReason = x.ReviewReason
            }).ToList();

            PagedResultDTO pagedResult = new PagedResultDTO();
            pagedResult.Items = items;
            pagedResult.TotalPages = result.TotalPages;
            return pagedResult;
        }
    }
}
