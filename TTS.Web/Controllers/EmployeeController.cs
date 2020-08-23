using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Linq;
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
        public async Task<IActionResult> Add(Guid id)
        {
            var result = await _employeeService.GetToAddAsync<UserDto>(id);
            //TODO Ensure OK
            var model = new EmployeeDto()
            {
                Employees = result.Value.ToList()
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