using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace TTS.Shared.Infrastructure
{
    public class IdentityErrorHelper
    {
        public string ErrorMessage(IdentityResult result){
            var errorMessage = new StringBuilder();
            foreach (var error in (from error in result.Errors select $"{error.Code} - {error.Description}\n"))
                errorMessage.Append(error);
            return errorMessage.ToString();
        }
    }
}