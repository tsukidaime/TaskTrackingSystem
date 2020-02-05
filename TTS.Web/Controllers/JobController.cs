using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.Shared.Models.Job;
using TTS.Shared.Models.User;

namespace TTS.Web.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        
        public JobController(IJobService jobService, IUserService userService, IMapper mapper, AppDbContext context)
        {
            _jobService = jobService;
            _userService = userService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _jobService.GetByUser<JobDto>(HttpContext.User);
            //TODO ensure OK
            var models = result.Value;
            return View(models);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return BadRequest();
            
            var result = await _jobService.GetAsync<JobDetailsDto>((Guid) id);
            //TODO Ensure ok
            var model = result.Value;
            return View(model);
        }

        
        public async Task<IActionResult> Create(Guid ?id)
        {
            if (id == null) return BadRequest();
            var users = (from user in await _userService.GetByJobAsync((Guid) id)
                select _mapper.Map<UserDto>(user));
            var jobStatuses = _context.Statuses.ToList();
            ViewBag.JobStatuses = jobStatuses;
            ViewBag.Users = users;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _jobService.CreateAsync(dto);
            //TODO ensure ok
            
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return BadRequest();
            var result = await _jobService.GetAsync<JobEditDto>((Guid) id);
            //TODO ensure ok
            var model = result.Value;
            var jobStatuses = _context.Statuses.ToList();
            ViewBag.JobStatuses = jobStatuses;
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobEditDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _jobService.UpdateAsync(dto); 
            //TODO Ensure OK
            
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var result = await _jobService.GetAsync<JobDto>((Guid) id);
            //TODO ensure ok
            var model = result.Value;
            return View(model);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _jobService.DeleteByIdAsync<JobDto>(id);
            //TODO Ensure ok
            return RedirectToAction(nameof(Index));
        }

        
    }
}
