using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.RepositoryInterfaces
{
    public interface IFinancingLeadRepository
    {
        Task AddAsync(FinancingLead lead);
        Task<FinancingLead?> GetByIdAsync(Guid id);
        Task<(IList<FinancingLead> Leads, int TotalPages)> GetLeadsAsync(GetFinancingLeadsQueryDto queryDto);
        Task<bool> SaveChangesAsync();
    }
}
