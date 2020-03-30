using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Infrastructure;
using TTS.Shared.Models.Job;
using TTS.Shared.Models.Status;

namespace TTS.BLL.Services
{
    public class JobService : IJobService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly OperationHelper _operationHelper;
        private readonly IEmailSender _emailSender;

        public JobService(AppDbContext context, IMapper mapper, OperationHelper operationHelper,
            UserManager<User> userManager, IEmailSender emailSender)
        {
            _context = context;
            _mapper = mapper;
            _operationHelper = operationHelper;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<OperationStatus<T>> CreateAsync<T>(T item)
        {
            var dto = item as JobCreateDto;
            if (dto == null) return _operationHelper.BadRequest<T>("JobCreateDto is null");
            var job = new Job()
            {
                Name = dto.Name,
                Deadline = dto.Deadline,
                Description = dto.Description,
                StatusId = dto.StatusId
            };
            try
            {
                await _context.Jobs.AddAsync(job);
                foreach (var userId in dto.SelectedUsers)
                {
                    var user = await _userManager.FindByIdAsync(userId.ToString());
                    _context.UserJobs.Add(new UserJob()
                    {
                        UserId = userId,
                        JobId = job.Id
                    });
                    await _emailSender.SendEmailAsync(user.Email, "New task for you",
                        $"<p>Hello! You have a new job!</p> <p>{job.Name} \n {job.Description}</p>");
                }

                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                return _operationHelper.InternalServerError<T>(e.Message);
            }
            return _operationHelper.OK(_mapper.Map<T>(job),"Job created successfully");
        }

        public async Task<OperationStatus<T>> DeleteByIdAsync<T>(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            try
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return _operationHelper.InternalServerError<T>(e.Message);
            }

            return _operationHelper.OK<T>($"Job {job.Id} removed successfully");
        }

        public async Task<OperationStatus<T>> UpdateAsync<T>(T item)
        {
            var dto = item as JobEditDto;
            var job = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == dto.Id);
            if(job == null) _operationHelper.BadRequest<T>($"Job {dto.Id} doesn't exist");
            job.Name = dto.Name;
            job.Description = dto.Description;
            job.Deadline = dto.Deadline;
            job.StatusId = dto.JobStatusId;
            try
            {
                _context.Update(job);
                await _context.SaveChangesAsync();
                return _operationHelper.OK<T>($"Job {job.Id} updated successfully");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return !JobExists(job) ? _operationHelper.NotFound<T>($"Job {dto.Id} isn't exist") 
                    : _operationHelper.InternalServerError<T>(e.Message);
            }
        }

        private bool JobExists(Job job)
        {
            return _context.Jobs.Contains(job);
        }

        public async Task<OperationStatus<T>> GetAsync<T>(Guid id)
        {
            var job = await _context.Jobs
                .Include(j => j.Status)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (job == null) return _operationHelper.NotFound<T>($"Job {id} isn't exist");
            var model = _mapper.Map<T>(job);
            return _operationHelper.OK(model,$"Job {job.Id} returned successfully");
        }

        public async Task<OperationStatus<List<T>>> GetAllAsync<T>()
        {
            var jobs = await _context.Jobs
                .Include(j => j.Status)
                .Select(x => _mapper.Map<T>(x))
                .ToListAsync();
            return _operationHelper.OK(jobs,"Jobs are returned successfully");
        }

        public async Task<OperationStatus<List<T>>> GetByUser<T>(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            var jobs = _context.UserJobs.Where(x => x.UserId == user.Id)
                .Select(x => _mapper.Map<T>(x.Job)).ToList();
            return _operationHelper.OK(jobs,"Jobs are returned successfully");
        }
    }
}