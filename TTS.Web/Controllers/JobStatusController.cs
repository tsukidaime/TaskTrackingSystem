using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TTS.DAL;
using TTS.DAL.Entities;

namespace TTS.Web
{
    [Authorize(Roles = "admin")]
    public class JobStatusController : Controller
    {
        private readonly AppDbContext _context;

        public JobStatusController(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobStatuses.ToListAsync());
        }

        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobStatus = await _context.JobStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobStatus == null)
            {
                return NotFound();
            }

            return View(jobStatus);
        }

        
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Color")] JobStatus jobStatus)
        {
            if (ModelState == null || !ModelState.IsValid) return View(jobStatus);
            _context.Add(new JobStatus()
            {
                Name = jobStatus.Name,
                Color = jobStatus.Color
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobStatus = await _context.JobStatuses.FindAsync(id);
            if (jobStatus == null)
            {
                return NotFound();
            }
            return View(jobStatus);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,[Bind("Name,Color,Id"),FromForm] JobStatus jobStatus)
        {
            if (!ModelState.IsValid) return View(jobStatus);
            var status = await _context.FindAsync<JobStatus>(id);
            status.Name = jobStatus.Name;
            status.Color = jobStatus.Color;
            try
            {
                _context.Update(status);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobStatusExists(jobStatus.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobStatus = await _context.JobStatuses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobStatus == null)
            {
                return NotFound();
            }

            return View(jobStatus);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var jobStatus = await _context.JobStatuses.FindAsync(id);
            _context.JobStatuses.Remove(jobStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobStatusExists(Guid id)
        {
            return _context.JobStatuses.Any(e => e.Id == id);
        }
    }
}
