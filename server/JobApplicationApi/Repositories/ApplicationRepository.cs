﻿using JobApplicationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JobApplicationApi.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDBContext _context;

        public ApplicationRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobApplication>> GetAll(int page, int size)
        {
            /* https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/sort-filter-page?view=aspnetcore-9.0
             * Use a switch statement to order by different columns
             */
            return await _context.JobApplication
                .OrderByDescending(item => item.dateApplied)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();
        }

        public async Task<JobApplication> Get(int id)
        {
            return await _context.JobApplication.FindAsync(id);
        }

        public async Task Post(JobApplication jobApplication)
        {
            _context.JobApplication.Add(jobApplication);
            await _context.SaveChangesAsync();
        }

        public async Task Put(int id, JobApplication jobApplication)
        {
            _context.Entry(jobApplication).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        // Helper function used to count total elements in paginated data
        public Task<int> Count()
        {
            return _context.JobApplication.CountAsync();
        }

        public bool Exists(int id)
        {
            return _context.JobApplication.Any(e => e.id == id);
        }
    }
}
