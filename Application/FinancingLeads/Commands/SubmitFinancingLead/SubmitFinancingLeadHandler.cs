using Application.Common.Interfaces.RepositoryInterfaces;
using Application.DTOs;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Commands.SubmitFinancingLead
{
    internal class SubmitFinancingLeadHandler : IRequestHandler<SubmitFinancingLeadCommand, Guid>
    {
        private readonly IFinancingLeadRepository _repository;
        public SubmitFinancingLeadHandler(IFinancingLeadRepository repository)
        {
            _repository = repository;
        }
        public async Task<Guid> Handle(SubmitFinancingLeadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var lead = FinancingLead.Create(
                request.SubmitFinancingLead.Name,
                request.SubmitFinancingLead.Email,
                request.SubmitFinancingLead.PhoneE164,
                request.SubmitFinancingLead.TypeOfActivity,
                request.SubmitFinancingLead.CommercialRegisterType,
                request.SubmitFinancingLead.PreferredContactMethod,
                request.SubmitFinancingLead.AnnualIncome,
                request.SubmitFinancingLead.Notes);

                await _repository.AddAsync(lead);
                await _repository.SaveChangesAsync();
                return lead.Id;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
