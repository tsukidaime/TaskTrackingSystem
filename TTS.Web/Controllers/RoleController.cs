using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using TTS.BLL;
using TTS.BLL.Services;
using TTS.BLL.Services.Abstract;
using TTS.Shared.Models.User;
using TTS.Shared.Models.Role;

namespace TTS.Web.Controllers
{
    //[Authorize(Roles = "admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        
        public RoleController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _roleService.GetAllAsync<RoleDto>();
            //TODO ensure ok
            var models = result.Value;
            return View(models);
        }
        
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RoleDto dto)
        {
            if (!ModelState.IsValid) return View(dto.Name);
            var result = await _roleService.CreateAsync(dto);
            //TODO Ensure OK
            return Redirect(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return BadRequest();
            var result = await _roleService.GetAsync<RoleDto>((Guid)id);
            //TODO ensure ok
            var model = result.Value;
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _roleService.UpdateAsync(dto);
            //TODO Ensure ok
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Remove(Guid? id)
        {
            if (id == null) return BadRequest();
            var userRoles = await _roleService.GetByUserAsync((Guid)id);
            //TODO Ensure ok
            var model = new RoleRemoveDto()
            {
                UserId = (Guid)id
            };
            ViewBag.Roles = userRoles.Value;
            return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Guid id, RoleRemoveDto dto)
        {
            var result = await _roleService.RemoveRolesAsync(dto);
            //Ensure ok
            return RedirectToAction("Index","User");
        }
        
        [HttpGet]
        public async Task<IActionResult> Assign(Guid? id)
        {
            if (id == null) return BadRequest();
            var userRoles = await _roleService.GetByUserAsync((Guid)id);
            var allRoles = await _roleService.GetAllAsync<RoleDto>();
            //TODO Ensure ok
            var model = new RoleAssignDto()
            {
                UserId = (Guid)id
            };
            ViewBag.Roles = allRoles.Value.Select(x => x.Name).ToList().Except(userRoles.Value);
            return View(model);
        }

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(Guid id, RoleAssignDto dto)
        {
            var result = await _roleService.AssignRolesAsync(dto);
            //Ensure ok
            return RedirectToAction("Index","User");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            var result = await _roleService.GetAsync<RoleDto>((Guid)id);
            //TODO ensure ok
            var model = result.Value;
            return View(model);
        }


        [HttpPost, ActionName("Delete"),ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _roleService.DeleteByIdAsync<RoleDto>(id);
            //TODO Ensure OK
            return RedirectToAction("Index");
        }
    }
}