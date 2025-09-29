using Application.Common.Interfaces.RepositoryInterfaces;
using Application.DTOs;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Queries.GetFinancingLeadById
{
    internal class GetFinancingLeadByIdHandler : IRequestHandler<GetFinancingLeadByIdQuery, FinancingLeadDto>
    {
        private readonly IFinancingLeadRepository _repository;
        public GetFinancingLeadByIdHandler(IFinancingLeadRepository repository)
        {
            _repository = repository;
        }
        public async Task<FinancingLeadDto> Handle(GetFinancingLeadByIdQuery request, CancellationToken cancellationToken)
        {
            FinancingLead financing = await _repository.GetByIdAsync(request.ID);
            if (financing != null)
            {
                FinancingLeadDto financingLead = new FinancingLeadDto()
                {
                    Id = financing.Id,
                    Name = financing.Name,
                    Email = financing.Email,
                    PhoneE164 = financing.PhoneE164,
                    PreferredContactMethod = financing.PreferredContactMethod.ToString(),
                    TypeOfActivity = financing.TypeOfActivity,
                    CommercialRegisterType = financing.CommercialRegisterType,
                    AnnualIncome = financing.AnnualIncome,
                    Notes = financing.Notes,
                    CreatedAt = financing.CreatedAt,
                    Status = financing.Status.ToString(),
                    ReviewedAt = financing.ReviewedAt,
                    ReviewReason = financing.ReviewReason,
                };

                return financingLead;
            }
            return null;
        }
    }
}
