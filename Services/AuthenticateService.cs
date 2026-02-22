using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;



namespace AlunoWeb.Services
{
	public class AuthenticateService : IAuthenticate
	{
        		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;

		public AuthenticateService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
		}

		public async Task<IdentityUser> Authenticate(string email, string password)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user != null)
			{
				var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
				if (result.Succeeded)
				{
					return user;
				}
			}
			return null;
		}

		public async Task<IdentityResult> RegisterUser(string email, string password)
		{
			var appUser = new IdentityUser { UserName = email, Email = email };
			var result = await _userManager.CreateAsync(appUser, password);
			if (result.Succeeded)
			{
				await _signInManager.SignInAsync(appUser, false);
				//return true;
			}
			return result;
		}

		public async Task Logout()
		{
			await _signInManager.SignOutAsync();
		}
	}
}