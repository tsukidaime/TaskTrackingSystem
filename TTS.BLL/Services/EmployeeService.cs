using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Infrastructure;
using TTS.Shared.Models.Employee;

namespace TTS.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<User> _userManager;
        private readonly OperationHelper _operationHelper;
        private readonly AppDbContext _context;
        private readonly IdentityErrorHelper _errorHelper;
        private readonly IMapper _mapper;

        public EmployeeService(UserManager<User> userManager, OperationHelper operationHelper, AppDbContext context, IdentityErrorHelper errorHelper, IMapper mapper)
        {
            _userManager = userManager;
            _operationHelper = operationHelper;
            _context = context;
            _errorHelper = errorHelper;
            _mapper = mapper;
        }

        public async Task<OperationStatus<IEnumerable<T>>> GetAsync<T>(Guid id)
        {
            var manager = await _userManager.FindByIdAsync(id.ToString());
            var models = manager.Employees.Select(x => _mapper.Map<T>(x));
            return _operationHelper.OK(models, "Employees returned successfully");
        }

        public async Task<OperationStatus<T>> CreateAsync<T>(T item)
        {
            if (!(item is EmployeeDto dto))
                return _operationHelper.BadRequest<T>("EmployeeDto is null");
            var employee = await _userManager.FindByIdAsync(dto.EmployeeId.ToString());
            employee.ManagerId = dto.UserId;
            var result = await _userManager.UpdateAsync(employee);
            if (!result.Succeeded) _operationHelper.InternalServerError<T>(_errorHelper.ErrorMessage(result));
            await _context.SaveChangesAsync();
            return _operationHelper.OK<T>("Employee added successfully");
        }
    }
}