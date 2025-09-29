using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Queries.GetFinancingLeads
{
    public class GetFinancingLeadsQuery : IRequest<PagedResultDTO>
    {
        public GetFinancingLeadsQueryDto Data { get; set; }
        public GetFinancingLeadsQuery(GetFinancingLeadsQueryDto data)
        {
            Data = data;
        }
    }
}
