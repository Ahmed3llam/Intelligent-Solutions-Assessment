using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Queries.GetFinancingLeadById
{
    public class GetFinancingLeadByIdQuery : IRequest<FinancingLeadDto>
    {
        public Guid ID{ get; }

        public GetFinancingLeadByIdQuery(Guid id)
        {
            ID = id;
        }
    }
}
