using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TTS.BLL.Services.Abstract;
using TTS.DAL.Entities;
using TTS.Shared.Infrastructure;
using TTS.Shared.Models.Auth;

namespace TTS.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;

        public AuthService(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public Task<OperationStatus<T>> Authenticate<T>(T item)
        {
            //var dto = item as AuthDto;
            //var user = _signInManager

            throw new NotImplementedException();
        }
    }
}