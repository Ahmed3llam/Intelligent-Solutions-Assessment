using MediatR;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Commands.SubmitFinancingLead
{
    public class SubmitFinancingLeadCommand : IRequest<Guid>
    {
        public SubmitFinancingLeadRequest SubmitFinancingLead { get; set; }
        public SubmitFinancingLeadCommand(SubmitFinancingLeadRequest submitFinancingLead)
        {
            SubmitFinancingLead = submitFinancingLead;
        }
    }
}
