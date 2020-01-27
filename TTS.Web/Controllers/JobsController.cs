using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TTS.BLL;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Models.User;
using TTS.Shared.Models.Job;

namespace TTS.Web
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly JobService _jobService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        
        public JobsController(JobService jobService, UserService userService, IMapper mapper, AppDbContext context)
        {
            _jobService = jobService;
            _userService = userService;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUser(HttpContext.User);
            var models = (from job in await _userService.GetJobs(user.Id)
                select _mapper.Map<JobModel>(job)).ToList();
            return View(models);
        }
        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _jobService.Get((Guid) id);
            if (job == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<JobDetailsModel>(job);

            return View(model);
        }

        
        public async Task<IActionResult> Create(Guid ?id)
        {
            if (id == null) return NotFound();
            var users = (from user in await _userService.GetSubordinates((Guid) id)
                select _mapper.Map<UserModel>(user));
            var jobStatuses = _context.JobStatuses.ToList();
            ViewBag.JobStatuses = jobStatuses;
            ViewBag.Users = users;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _jobService.Add(model);
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();
            var job = await _jobService.Get((Guid) id);
            if (job == null) return NotFound();
            var jobStatuses = _context.JobStatuses.ToList();
            ViewBag.JobStatuses = jobStatuses;
            var model = _mapper.Map<JobEditModel>(job);
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobEditModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _jobService.Edit(model);
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _jobService.Get((Guid)id);
            if (job == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<JobModel>(job);
            return View(model);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
