using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Infrastructure;
using TTS.Shared.Models.Role;

namespace TTS.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly OperationHelper _operationHelper;
        private readonly IdentityErrorHelper _errorHelper;
        private readonly AppDbContext _context;
        
        public RoleService(RoleManager<IdentityRole<Guid>> roleManager, IMapper mapper, OperationHelper operationHelper, IdentityErrorHelper errorHelper, AppDbContext context, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _operationHelper = operationHelper;
            _errorHelper = errorHelper;
            _context = context;
            _userManager = userManager;
        }
        
        public async Task<OperationStatus<T>> CreateAsync<T>(T item)
        {
            var dto = item as RoleDto;
            var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(dto.Name));
            if(!result.Succeeded) _operationHelper.InternalServerError<T>(_errorHelper.ErrorMessage(result));
            await _context.SaveChangesAsync();
            return  _operationHelper.OK<T>("Role created successfully");

        }

        public async Task<OperationStatus<T>> DeleteByIdAsync<T>(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return _operationHelper.NotFound<T>($"Role {id} doesn't exist");
            var result = await _roleManager.DeleteAsync(role);
            if(!result.Succeeded) _operationHelper.InternalServerError<T>(_errorHelper.ErrorMessage(result));
            await _context.SaveChangesAsync();
            return  _operationHelper.OK<T>("Role deleted successfully");
        }

        public async Task<OperationStatus<T>> UpdateAsync<T>(T item)
        {
            var dto = item as RoleDto;
            var role = await _roleManager.FindByIdAsync(dto.Id.ToString());
            role.Name = dto.Name;
            var result = await _roleManager.UpdateAsync(role);
            if(!result.Succeeded) _operationHelper.InternalServerError<T>(_errorHelper.ErrorMessage(result));
            await _context.SaveChangesAsync();
            return  _operationHelper.OK<T>("Role updated successfully");
        }

        public async Task<OperationStatus<T>> GetAsync<T>(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            var dto = _mapper.Map<T>(role);
            return _operationHelper.OK(dto, "Role returned successfully");
        }

        public async Task<OperationStatus<List<T>>> GetAllAsync<T>()
        {
            var roles = _roleManager.Roles.Select(x => _mapper.Map<T>(x)).AsEnumerable();
            return _operationHelper.OK(roles.ToList(), "Success");
        }

        public async Task<OperationStatus<IEnumerable<string>>> GetByUserAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            return _operationHelper.OK(roles.AsEnumerable(), "Success");
        }

        public async Task<OperationStatus<T>> AssignRolesAsync<T>(T item)
        {
            var dto = item as RoleAssignDto;
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            var result = await _userManager.AddToRolesAsync(user, dto.Roles);
            if(!result.Succeeded) _operationHelper.InternalServerError<T>(_errorHelper.ErrorMessage(result));
            await _context.SaveChangesAsync();
            return  _operationHelper.OK<T>("Roles assigned successfully");
        }

        public async Task<OperationStatus<T>> RemoveRolesAsync<T>(T item)
        {
            var dto = item as RoleRemoveDto;
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            var result = await _userManager.RemoveFromRolesAsync(user, dto.Roles);
            if(!result.Succeeded) _operationHelper.InternalServerError<T>(_errorHelper.ErrorMessage(result));
            await _context.SaveChangesAsync();
            return  _operationHelper.OK<T>("Roles removed successfully");
        }
    }
}