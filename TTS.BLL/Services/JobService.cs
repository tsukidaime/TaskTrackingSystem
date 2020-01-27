using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Models.Job;

namespace TTS.BLL
{
    public class JobService
    {
        private readonly AppDbContext _context;

        public JobService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Job>> Get() {
            var jobs = await _context.Jobs
                .Include(j => j.JobStatus)
                .ToListAsync();
            return jobs;
        }

        public async Task<Job> Get(Guid id)
        {
            var job = await _context.Jobs
                .Include(j => j.JobStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            return job;
        }

        public async Task<List<User>> GetUsersByJob(Job job)
        {
            var users = await _context.UserJobs.Where(x => x.JobId == job.Id)
                .Select(x => x.User).ToListAsync();
            return users;
        }

        public async Task Add(JobCreateModel model)
        {
            var job = new Job()
            {
                Name = model.Name,
                Deadline = model.Deadline,
                Description = model.Description,
                Progress = 0,
                JobStatusId = model.JobStatusId
            };
            _context.Jobs.Add(job);
            foreach (var userId in model.SelectedUsers)
            {
                _context.UserJobs.Add(new UserJob()
                {
                    UserId = userId,
                    JobId = job.Id
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task Edit(JobEditModel model)
        {
            try
            {
                var job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == model.Id);
                job.Name = model.Name;
                job.Description = model.Description;
                job.Deadline = model.Deadline;
                job.JobStatusId = model.JobStatusId;
                _context.Update(job);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //Log
            }
        }
        
    }
}