using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Models.Status;

namespace TTS.Web
{
    [Authorize(Roles = "admin")]
    public class StatusController : Controller
    {
        private readonly IStatusService _statusService;

        public StatusController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        
        public async Task<IActionResult> Index()
        {
            var result = await _statusService.GetAllAsync<StatusDto>();
            //TODO ensure OK
            var models = result.Value;
            return View(models);
        }

        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return BadRequest();
            var result = await _statusService.GetAsync<StatusDto>((Guid) id);
            //TODO Ensure ok
            var model = result.Value;
            return View(model);
        }

        
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StatusDto dto)
        {
            if (ModelState == null || !ModelState.IsValid) return View(dto);
            var result = _statusService.CreateAsync(dto);
            //TODO Ensure OK
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return BadRequest();
            var result = await _statusService.GetAsync<StatusDto>((Guid)id);
            //TODO ensure ok
            var model = result.Value;
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,StatusDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _statusService.UpdateAsync(dto);
            //TODO Ensure ok
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return BadRequest();
            var result = await _statusService.GetAsync<StatusDto>((Guid)id);
            //TODO ensure ok
            var model = result.Value;
            return View(model);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _statusService.DeleteByIdAsync<StatusDto>(id);
            //TODO Ensure OK
            return RedirectToAction("Index");
        }

        
    }
}
