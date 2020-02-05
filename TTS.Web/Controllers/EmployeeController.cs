using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TTS.BLL;
using TTS.BLL.Services;
using TTS.BLL.Services.Abstract;
using TTS.Shared.Models.Employee;
using TTS.Shared.Models.User;

namespace TTS.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmployeeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IUserService userService, IMapper mapper, IEmployeeService employeeService)
        {
            _userService = userService;
            _mapper = mapper;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> Add(Guid ?id)
        {
            if (id == null) return BadRequest();
            var employees = await _employeeService.GetAsync<UserDto>((Guid) id);
            var users = await _userService.GetAllAsync<UserDto>();
            //TODO Ensure OK
            var viewUsers = users.Value.Except(employees.Value);
            var model = new EmployeeDto()
            {
                Employees = viewUsers.ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(Guid id, EmployeeDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            dto.UserId = id;
            await _employeeService.CreateAsync(dto);
            //TODO ensure ok
            return RedirectToAction("Index","User");
        }
    }
}