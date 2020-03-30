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
using TTS.Shared.Models.Todo;

namespace TTS.BLL.Services
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly OperationHelper _operationHelper;

        public TodoService(OperationHelper operationHelper, IMapper mapper, AppDbContext context)
        {
            _operationHelper = operationHelper;
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<OperationStatus<T>> CreateAsync<T>(T item)
        {
            var dto = item as TodoDto;
            if (dto == null) return _operationHelper.BadRequest<T>($"TodoDto is null");
            var todo = new Todo()
            {
                Content = dto.Content,
                JobId = dto.JobId
            };
            try
            {
                await _context.Todos.AddAsync(todo);
                await _context.SaveChangesAsync();
            }
            catch (Exception e) {return _operationHelper.InternalServerError<T>(e.Message);}
            return _operationHelper.OK(_mapper.Map<T>(todo),"Todo created successfully");
        }

        public async Task<OperationStatus<T>> DeleteByIdAsync<T>(Guid id)
        {
            var todo = await _context.Todos.FindAsync(id);
            try
            {
                _context.Todos.Remove(todo);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return _operationHelper.InternalServerError<T>(e.Message);
            }

            return _operationHelper.OK<T>($"Status {todo.Id} removed successfully");
        }

        public async Task<OperationStatus<T>> UpdateAsync<T>(T item)
        {
            var dto = item as TodoDto;
            var todo = await _context.Todos.FindAsync(dto.Id);
            if(todo == null) _operationHelper.BadRequest<T>($"Todo {dto.Id} doesn't exist");
            todo.Content = dto.Content;
            todo.Done = dto.Done;
            try
            {
                _context.Update(todo);
                await _context.SaveChangesAsync();
                return _operationHelper.OK(_mapper.Map<T>(todo),$"Todo {todo.Id} updated successfully");
            }
            catch (DbUpdateConcurrencyException e)
            {
                return !TodoExists(todo.Id) ? _operationHelper.NotFound<T>($"Todo {dto.Id} isn't exist") 
                    : _operationHelper.InternalServerError<T>(e.Message);
            }
        }

        private bool TodoExists(Guid id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
        
        public async Task<OperationStatus<T>> GetAsync<T>(Guid id)
        {
            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
            var dto = _mapper.Map<T>(todo);
            return _operationHelper.OK(dto, "Todo returned successfully");
        }

        public async Task<OperationStatus<List<T>>> GetAllAsync<T>()
        {
            var todos = await _context.Todos.Select(x => _mapper.Map<T>(x)).ToListAsync();
            return _operationHelper.OK(todos, "Success");
        }

        public async Task<OperationStatus<List<T>>> GetByJobAsync<T>(Guid id)
        {
            var job = await _context.Jobs.Include(x => x.Todos)
                .FirstOrDefaultAsync(x => x.Id == id);
            var todos = job.Todos.Select(x => _mapper.Map<T>(x)).ToList();
            return _operationHelper.OK(todos, "Success");
        }
    }
}