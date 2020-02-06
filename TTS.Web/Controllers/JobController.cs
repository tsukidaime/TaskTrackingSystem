using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.Shared.Models.Job;
using TTS.Shared.Models.Status;
using TTS.Shared.Models.Todo;
using TTS.Shared.Models.User;

namespace TTS.Web.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private readonly IJobService _jobService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IStatusService _statusService;
        private readonly IEmployeeService _employeeService;
        private readonly ITodoService _todoService;
        
        public JobController(IJobService jobService, IUserService userService, IMapper mapper, IStatusService statusService, IEmployeeService employeeService, ITodoService todoService, IEmailSender emailSender)
        {
            _jobService = jobService;
            _userService = userService;
            _mapper = mapper;
            _statusService = statusService;
            _employeeService = employeeService;
            _todoService = todoService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _jobService.GetByUser<JobDto>(HttpContext.User);
            //TODO ensure OK
            var jobs = result.Value;
            foreach (var job in jobs)
            {
                var status = await _statusService.GetAsync<StatusDto>(job.StatusId);
                job.Status = status.Value;
            }
            var models = jobs;
            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return BadRequest();
            var result = await _jobService.GetAsync<JobDetailsDto>((Guid) id);
            //TODO Ensure ok
            var job = result.Value;
            var status = await _statusService.GetAsync<StatusDto>(job.StatusId);
            job.Status = status.Value;
            var users = await _userService.GetByJobAsync<UserDto>(job.Id);
            job.Users = users.Value.ToList();
            var todos = await _todoService.GetByJobAsync<TodoDto>(job.Id);
            job.Todos = todos.Value.ToList();
            var model = job;
            return View(model);
        }


        public async Task<IActionResult> Create()
        {
            var user = await _userService.GetAsync<UserDto>(HttpContext.User);
            var users = await _employeeService.GetAsync<UserDto>(user.Value.Id);
            var statuses = await _statusService.GetAllAsync<StatusDto>();
            ViewBag.Statuses = statuses.Value;
            ViewBag.Users = users.Value;
            return View(new JobCreateDto());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var job = await _jobService.CreateAsync(dto);
            //TODO ensure ok
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return BadRequest();
            var job = await _jobService.GetAsync<JobEditDto>((Guid) id);
            //TODO ensure ok
            var model = job.Value;
            var users = await _userService.GetByJobAsync<UserDto>(model.Id);
            var statuses = await _statusService.GetAllAsync<StatusDto>();
            ViewBag.Statuses = statuses.Value;
            ViewBag.Users = users.Value;
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

        
        [HttpGet,Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var result = await _jobService.GetAsync<JobDto>((Guid) id);
            //TODO ensure ok
            var model = result.Value;
            return View(model);
        }

        
        [HttpPost, ActionName("Delete"),Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _jobService.DeleteByIdAsync<JobDto>(id);
            //TODO Ensure ok
            return RedirectToAction(nameof(Index));
        }

        
    }
}
