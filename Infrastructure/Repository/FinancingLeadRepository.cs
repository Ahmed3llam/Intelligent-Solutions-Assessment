using Application.Common.Interfaces.RepositoryInterfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class FinancingLeadRepository : IFinancingLeadRepository
    {
        private readonly ApplicationDbContext _context;

        public FinancingLeadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(FinancingLead lead)
        {
            _context.FinancingLeads.Add(lead);
            return Task.CompletedTask;
        }

        public async Task<FinancingLead?> GetByIdAsync(Guid id)
        {
            return await _context.FinancingLeads.FindAsync(id);
        }

        public async Task<(IList<FinancingLead> Leads, int TotalPages)> GetLeadsAsync(GetFinancingLeadsQueryDto queryDto)
        {
            IQueryable<FinancingLead> query = _context.FinancingLeads.AsNoTracking();

            var search = queryDto.Search;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchLower = search.Trim().ToLower();

                if (queryDto.SearchWith.HasValue)
                {
                    switch (queryDto.SearchWith.Value)
                    {
                        case SearchTypes.Name:
                            query = query.Where(l => l.Name.ToLower().Contains(searchLower));
                            break;

                        case SearchTypes.Email:
                            query = query.Where(l => l.Email.ToLower().Contains(searchLower));
                            break;

                        case SearchTypes.Phone:
                            query = query.Where(l => l.PhoneE164.Contains(searchLower));
                            break;
                    }
                }
                else
                {
                    query = query.Where(l =>
                        l.Name.ToLower().Contains(searchLower) ||
                        l.Email.ToLower().Contains(searchLower) ||
                        l.PhoneE164.Contains(searchLower));
                }
            }


            if (!string.IsNullOrWhiteSpace(queryDto.PhoneStartsWith))
            {
                query = query.Where(l => l.PhoneE164.StartsWith(queryDto.PhoneStartsWith.Trim()));
            }

            if (queryDto.From.HasValue)
            {
                query = query.Where(l => l.CreatedAt >= queryDto.From.Value);
            }

            if (queryDto.To.HasValue)
            {
                query = query.Where(l => l.CreatedAt <= queryDto.To.Value);
            }

            if (queryDto.Status.HasValue)
            {
                query = query.Where(l => l.Status == queryDto.Status);
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)queryDto.PageSize);

            var sortBy = queryDto.SortBy;
            var sortDir = queryDto.SortDir;
            if (sortBy?.ToLower() == "status")
            {
                query = sortDir?.ToLower() == "asc"
                    ? query.OrderBy(x => x.Status)
                    : query.OrderByDescending(x => x.Status);
            }
            else if (sortBy?.ToLower() == "name")
            {
                query = sortDir?.ToLower() == "asc"
                    ? query.OrderBy(x => x.Name)
                    : query.OrderByDescending(x => x.Name);
            }
            else
            {
                query = sortDir?.ToLower() == "asc"
                    ? query.OrderBy(x => x.CreatedAt)
                    : query.OrderByDescending(x => x.CreatedAt);
            }

            var leads = await query
                .Skip((queryDto.Page - 1) * queryDto.PageSize)
                .Take(queryDto.PageSize)
                .ToListAsync();

            return (leads, totalPages);

        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
