using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TTS.BLL.Services.Abstract;
using TTS.DAL;
using TTS.DAL.Entities;
using TTS.Shared.Infrastructure;
using TTS.Shared.Models.Status;

namespace TTS.BLL.Services
{
    public class StatusService : IStatusService
    {
        private readonly OperationHelper _operationHelper;
        private readonly AppDbContext _context;
        private readonly IdentityErrorHelper _errorHelper;
        private readonly IMapper _mapper;

        public StatusService(AppDbContext context, OperationHelper operationHelper, IdentityErrorHelper errorHelper, IMapper mapper)
        {
            _context = context;
            _operationHelper = operationHelper;
            _errorHelper = errorHelper;
            _mapper = mapper;
        }

        public async Task<OperationStatus<T>> CreateAsync<T>(T item)
        {
            var dto = item as StatusDto;
            if (dto == null) return _operationHelper.BadRequest<T>($"StatusDto  is null");
            var status = new Status()
            {
                Name = dto.Name,
                Color = dto.Color
            };
            try
            {
                await _context.Statuses.AddAsync(status);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) {return _operationHelper.InternalServerError<T>(e.Message);}
            return _operationHelper.OK<T>("Status created successfully");
        }

        public async Task<OperationStatus<T>> DeleteByIdAsync<T>(Guid id)
        {
            var status = await _context.Statuses.FindAsync(id);
            try
            {
                _context.Statuses.Remove(status);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return _operationHelper.InternalServerError<T>(e.Message);
            }

            return _operationHelper.OK<T>($"Status {status.Id} removed successfully");
        }

        public async Task<OperationStatus<T>> UpdateAsync<T>(T item)
        {
            var dto = item as StatusDto;
            var status = await _context.Statuses.FindAsync(dto.Id);
            if(status == null) _operationHelper.BadRequest<T>($"Status {dto.Id} doesn't exist");
            status.Name = dto.Name;
            status.Color = dto.Color;
            try
            {
                _context.Update(status);
                await _context.SaveChangesAsync();
                return _operationHelper.OK<T>($"Status {status.Id} updated successfully");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return !StatusExists(status.Id) ? _operationHelper.NotFound<T>($"Status {dto.Id} isn't exist") 
                    : _operationHelper.InternalServerError<T>(e.Message);
            }
        }
        
        private bool StatusExists(Guid id)
        {
            return _context.Statuses.Any(e => e.Id == id);
        }

        public async Task<OperationStatus<T>> GetAsync<T>(Guid id)
        {
            var status = await _context.Statuses
                .FirstOrDefaultAsync(m => m.Id == id);
            var dto = _mapper.Map<T>(status);
            return _operationHelper.OK(dto, "Status returned successfully");
        }

        public async Task<OperationStatus<List<T>>> GetAllAsync<T>()
        {
            var statuses = await _context.Statuses.Select(x => _mapper.Map<T>(x)).ToListAsync();
            return _operationHelper.OK(statuses, "Success");
        }
    }
}