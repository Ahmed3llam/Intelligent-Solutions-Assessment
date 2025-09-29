using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PagedResultDTO
    {
        public List<FinancingLeadDto>? Items { get; set; }
        public int TotalPages { get; set; }
    }
}
