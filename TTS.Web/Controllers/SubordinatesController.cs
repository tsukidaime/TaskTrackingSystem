using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TTS.BLL;
using TTS.Shared.Models.Subordinates;
using TTS.Shared.Models.User;

namespace TTS.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class SubordinatesController : Controller
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public SubordinatesController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Add(Guid id)
        {
            var manager = await _userService.GetUser(id);
            var users = await _userService.GetUsers();
            users.Remove(manager);
            var model = new SubordinateAddModel()
            {
                AllUsers = (from user in users.Except(manager.Subordinates) select _mapper.Map<UserModel>(user)).ToList(),
                UserSubs = (from user in manager.Subordinates select _mapper.Map<UserModel>(user)).ToList()
            };
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(Guid id, SubordinateAddModel model)
        {
            if (!ModelState.IsValid) return View(model);
            await _userService.AddSubordinate(id, model.Subordinates);
            return RedirectToAction("Index","Users");
        }
    }
}