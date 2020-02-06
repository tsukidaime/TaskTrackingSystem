using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTS.BLL.Services.Abstract;
using TTS.Shared.Models.Todo;

namespace TTS.Web.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> List(Guid id)
        {
            var result = await _todoService.GetByJobAsync<TodoDto>(id);
            //TODO ensure OK
            var models = result.Value;
            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TodoDto value)
        {
            if (ModelState == null || !ModelState.IsValid) return BadRequest();
            var result = await _todoService.CreateAsync(value);
            //TODO Ensure OK
            return Json(result.Value);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] TodoDto value)
        {
            if (!ModelState.IsValid) return BadRequest();
            var result = await _todoService.UpdateAsync(value);
            //TODO Ensure ok
            return Json(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _todoService.DeleteByIdAsync<TodoDto>(id);
            //TODO Ensure OK
            return Ok();
        }
    }

}