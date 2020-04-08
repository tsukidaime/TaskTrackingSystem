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
            var roles = new string[]
                {"Owner", "Admin", "Manager", "Employee", "Moderator"};

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
            var users = new User[]
            {
                new User
                {
                    FirstName = "XXXX",
                    LastName = "XXXX",
                    Email = "xxxx@example.com",
                    NormalizedEmail = "XXXX@EXAMPLE.COM",
                    UserName = "Owner",
                    NormalizedUserName = "OWNER",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new User()
                {
                    FirstName = "XXXX",
                    LastName = "XXXX",
                    Email = "xxx1@example.com",
                    NormalizedEmail = "XXX1@EXAMPLE.COM",
                    UserName = "Admin",
                    NormalizedUserName = "ADMIN",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new User()
                {
                    FirstName = "XXXX",
                    LastName = "XXXX",
                    Email = "xxx2@example.com",
                    NormalizedEmail = "XXX2@EXAMPLE.COM",
                    UserName = "Moderator",
                    NormalizedUserName = "MODERATOR",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new User()
                {
                    FirstName = "XXXX",
                    LastName = "XXXX",
                    Email = "mng@example.com",
                    NormalizedEmail = "MNG@EXAMPLE.COM",
                    UserName = "Manager",
                    NormalizedUserName = "MANAGER",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new User()
                {
                    FirstName = "XXXX",
                    LastName = "XXXX",
                    Email = "emp1@example.com",
                    NormalizedEmail = "EMP1@EXAMPLE.COM",
                    UserName = "emp1",
                    NormalizedUserName = "EMP1",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new User()
                {
                    FirstName = "XXXX",
                    LastName = "XXXX",
                    Email = "emp2@example.com",
                    NormalizedEmail = "EMP2@EXAMPLE.COM",
                    UserName = "emp2",
                    NormalizedUserName = "EMP2",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
                new User()
                {
                    FirstName = "XXXX",
                    LastName = "XXXX",
                    Email = "emp3@example.com",
                    NormalizedEmail = "EMP3@EXAMPLE.COM",
                    UserName = "emp3",
                    NormalizedUserName = "EMP3",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                },
            };
            var userList = new List<User>();
            foreach (var user in users)
            {
                if (_userManager.Users.Any(u => u.UserName == user.UserName)) continue;
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(user, "123");
                user.PasswordHash = hashed;
                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded) continue;
                var createdUser = await _userManager.FindByNameAsync(user.UserName);
                userList.Add(createdUser);
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            var manager = userList.Find(x => x.UserName == "Manager");
            await _userManager.AddToRoleAsync(userList.Find(x=>x.UserName == "Owner"), "Owner");
            await _userManager.AddToRoleAsync(userList.Find(x=>x.UserName == "Admin"), "Admin");
            await _userManager.AddToRoleAsync(userList.Find(x=>x.UserName == "Moderator"), "Moderator");
            await _userManager.AddToRoleAsync(manager, "Manager");

            for (var i = 1; i <= 3; i++)
            {
                var emp = userList.Find(x => x.UserName == $"emp{i}");
                emp.ManagerId = manager.Id;
                emp.Manager = manager;
                await _userManager.UpdateAsync(emp);
            }
        }

    }
}
