using Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FinancingLeads.Commands.ReviewFinancingLead
{
    public class ReviewFinancingLeadCommand : IRequest<ReviewDTO>
    {
        public ReviewRequest Request { get; set; }
        public ReviewFinancingLeadCommand(ReviewRequest request)
        {
            Request = request;
        }
    }
}
