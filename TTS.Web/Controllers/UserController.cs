using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TTS.BLL;
using TTS.BLL.Services;
using TTS.BLL.Services.Abstract;
using TTS.Shared.Models.User;

namespace TTS.Web.Controllers
{  
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }
        
        [ResponseCache(CacheProfileName = "Default")]
        public async Task<IActionResult> Index()
        {
            var operationStatus = await _userService.GetAllAsync<UserDto>();
            //TODO добавить сюда ensure ok
            return View(operationStatus.Value);
        }

        [ResponseCache(CacheProfileName = "Default"),HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return BadRequest();
            var result = await _userService.GetAsync<UserDetailsDto>((Guid) id);
            //TODO Ensure OK

            return View(_mapper.Map<UserDetailsDto>(result.Value));
        }

        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _userService.CreateAsync(dto);
            //TODO Ensure OK

            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return BadRequest();
            var result = await _userService.GetAsync<UserEditDto>((Guid)id);
            //TODO Ensure OK
            return View(_mapper.Map<UserEditDto>(result.Value));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _userService.UpdateAsync(dto);
            //TODO ensure OK
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();

            var result = await _userService.GetAsync<UserDto>((Guid)id);
            //TODO Ensure OK

            return View(_mapper.Map<UserDto>(result));
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _userService.DeleteByIdAsync<UserDto>(id);
            //TODO ensure OK
            return RedirectToAction(nameof(Index));
        }
    }
}