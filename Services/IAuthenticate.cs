using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace AlunoWeb.Services
{
        public interface IAuthenticate
    {
        Task<IdentityUser> Authenticate(string email, string password);
                Task<IdentityResult> RegisterUser(string email, string password); 
        Task Logout();
    }
}