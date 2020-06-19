using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Models.Employee;

namespace TTS.BLL.Utils
{
    public static class SeedData
    {
        public static async Task Initialize(UserManager<User> _userManager, RoleManager<IdentityRole<Guid>> _roleManager)
        {
            var roles = new [] {"admin", "manager", "employee"};

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
            var users = new User[]
            {
                new User()
                {
                    Email = "admin@tts.com",
                    UserName = "admin@tts.com"
                },
                new User()
                {
                    Email = "manager@tts.com",
                    UserName = "manager@tts.com"
                },
                new User()
                {
                    Email = "emp1@tts.com",
                    UserName = "emp1@tts.com"
                },
                new User()
                {
                    Email = "emp2@tts.com",
                    UserName = "emp2@tts.com"
                },
                new User()
                {
                    Email = "emp3@tts.com",
                    UserName = "emp3@tts.com"
                },
            };
            var userList = new List<User>();
            foreach (var user in users)
            {
                if (_userManager.Users.Any(u => u.UserName == user.UserName)) continue;
                var result = await _userManager.CreateAsync(user,"abc123");
                if (!result.Succeeded) continue;
                var createdUser = await _userManager.FindByNameAsync(user.UserName);
                userList.Add(createdUser);
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            var manager = userList.Find(x => x.UserName == "manager@tts.com");
            await _userManager.AddToRoleAsync(userList.Find(x=>x.UserName == "admin@tts.com"), "Admin");
            await _userManager.AddToRoleAsync(manager, "Manager");

            for (var i = 1; i <= 3; i++)
            {
                var emp = userList.Find(x => x.UserName == $"emp{i}@tts.com");
                emp.ManagerId = manager.Id;
                emp.Manager = manager;
                await _userManager.UpdateAsync(emp);
            }
        }

    }
}
