using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TTS.BLL;
using TTS.Shared.Models.User;

namespace TTS.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public UsersController(ILogger<HomeController> logger, UserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }
        
        [ResponseCache(CacheProfileName = "Default")]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetUsers();
            var models = (from user in users select _mapper.Map<UserModel>(user)).ToList();
            return View(models);
        }

        [ResponseCache(CacheProfileName = "Default"),HttpGet]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUser((Guid) id);
            if (user == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<UserDetailsModel>(user));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _userService.AddUser(model);
            if (result.Succeeded) return RedirectToAction(nameof(Index));
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUser((Guid)id);
            if (user == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<UserEditModel>(user));
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _userService.EditUser(model);
            if (result.Succeeded) return RedirectToAction(nameof(Index));
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userService.GetUser((Guid) id);
            if (user == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<UserModel>(user));
        }

        [HttpPost, ActionName("Delete"),
         ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var result = await _userService.DeleteUser(id);
            if (result.Succeeded) return RedirectToAction(nameof(Index));
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
    }
}