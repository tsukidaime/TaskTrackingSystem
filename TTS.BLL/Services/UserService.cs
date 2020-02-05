using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Infrastructure;
using TTS.Shared.Models.User;

namespace TTS.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly string _defaultPass = "qwe";
        private readonly OperationHelper _operationHelper;
        private readonly IdentityErrorHelper _errorHelper;

        public UserService(UserManager<User> userManager, AppDbContext context, IWebHostEnvironment appEnvironment,
            IMapper mapper, OperationHelper operationHelper, IdentityErrorHelper errorHelper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _operationHelper = operationHelper;
            _errorHelper = errorHelper;
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public async Task<OperationStatus<T>> CreateAsync<T>(T dto)
        {
            var user = _mapper.Map<User>(dto);
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, _defaultPass);
            if (!result.Succeeded) _operationHelper.InternalServerError<T>(_errorHelper.ErrorMessage(result));
            await _context.SaveChangesAsync();
            return _operationHelper.OK<T>("User created successfully");
        }

        public async Task<OperationStatus<T>> DeleteByIdAsync<T>(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return _operationHelper.NotFound<T>($"user {id} isn't exist");
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded) _operationHelper.InternalServerError<T>(_errorHelper.ErrorMessage(result));
            await _context.SaveChangesAsync();
            return _operationHelper.OK<T>("User deleted successfully");
        }

        public async Task<OperationStatus<T>> UpdateAsync<T>(T item)
        {
            var dto = item as UserEditDto;
            var user = await _context.Users.FindAsync(dto.Id);
            try
            {
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;
                user.BirthDate = dto.BirthDate;
                user.Position = dto.Position;
                user.SecondName = dto.SecondName;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded) _operationHelper.InternalServerError<T>(_errorHelper.ErrorMessage(result));
                await _context.SaveChangesAsync();
                return _operationHelper.OK<T>("User updated successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                return !UserExists(dto.Id) ? _operationHelper.NotFound<T>($"user {dto.Id} isn't exist") 
                    : _operationHelper.InternalServerError<T>($"Database update concurrency error");
            }
        }

        public async Task<OperationStatus<T>> GetAsync<T>(Guid id)
        {
            User user;
            try
            {
                user = await _userManager.FindByIdAsync(id.ToString());
            }
            catch (Exception e)
            {
                return _operationHelper.InternalServerError<T>($"Database error");
            }
            var model = _mapper.Map<T>(user);
            return _operationHelper.OK(model,$"{user.Id} user returned successfully");
        }
        public async Task<OperationStatus<T>> GetAsync<T>(ClaimsPrincipal principal)
        {
            User user;
            try
            {
                user = await _userManager.GetUserAsync(principal);
            }
            catch (Exception e)
            {
                return _operationHelper.InternalServerError<T>($"Database error");
            }
            var model = _mapper.Map<T>(user);
            return _operationHelper.OK(model,$"{user.Id} user returned successfully");
        }

        public async Task<OperationStatus<IEnumerable<T>>> GetAllAsync<T>()
        {
            var users = _userManager.Users.Select(x=>_mapper.Map<T>(x)).AsEnumerable();
            return _operationHelper.OK(users, "List of users returned successfully");
        }
        
        public async Task<OperationStatus<IEnumerable<T>>> GetByJobAsync<T>(Job job)
        {
            var users = _context.UserJobs.Where(x => x.JobId == job.Id)
                .Select(x => x.User).Select(x=>_mapper.Map<T>(x)).AsEnumerable();
            return _operationHelper.OK(users,"Users returned successfully");
        }
    }
}