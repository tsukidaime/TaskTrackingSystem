using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Models.User;

namespace TTS.BLL
{
    public class UserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly string _defaultPass = "qwe";

        public UserService(UserManager<User> userManager, AppDbContext context, IWebHostEnvironment appEnvironment, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _appEnvironment = appEnvironment;
            _mapper = mapper;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public async Task<List<Job>> GetJobs(Guid id)
        {
            var jobs = await _context.UserJobs.Where(x => x.UserId == id)
                .Select(x => x.Job).ToListAsync();
            return jobs;
        }
        public async Task<List<User>> GetSubordinates(Guid managerId)
        {
            var users = await _context.Users.Where(x => x.ManagerId == managerId).ToListAsync();
            return users;
        }

        public async Task AddSubordinate(Guid managerId, List<Guid> subordinates)
        {
            foreach (var id in subordinates)
            {
                var user = await GetUser(id);
                user.ManagerId = managerId;
                try
                {
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<IdentityResult> AddUser(UserCreateModel model)
        {
            var user = _mapper.Map<User>(model);
            user.UserName = model.Email;
            var result = await _userManager.CreateAsync(user, _defaultPass);
            if (result.Succeeded) await _context.SaveChangesAsync();
            return result;
        }

        public async Task<User> GetUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user;
        }
        public async Task<User> GetUser(ClaimsPrincipal principal)
        {
            var user = await _userManager.GetUserAsync(principal);
            return user;
        }

        public async Task<List<string>> GetUserRoles(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<IdentityResult> AddRolesAsync(User user, IEnumerable<string> roles)
        {
            var res = await _userManager.AddToRolesAsync(user, roles);
            return res;
        }
        
        public async Task<IdentityResult> RemoveRolesAsync(User user, IEnumerable<string> roles)
        {
            var res = await _userManager.RemoveFromRolesAsync(user, roles);
            return res;
        }

        public async Task<IdentityResult> EditUser(UserEditModel model)
        {
            var user = await _context.Users.FindAsync(model.Id);
            try
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.BirthDate = model.BirthDate;
                user.Position = model.Position;
                user.SecondName = model.SecondName;
                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded) await _context.SaveChangesAsync();
                return result;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(model.Id))
                    return IdentityResult.Failed();
                else
                    throw;
            }
        }

        public async Task<IdentityResult> DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return IdentityResult.Failed();
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) await _context.SaveChangesAsync();
            return result;
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

    }
}