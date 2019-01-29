using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASRWebMVC.Interface;
using ASRWebMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ASRWebMVC.Controllers
{
	[Route("auth")]
	public class AuthController : Controller
    {
		private IUserService _userService;

		public AuthController(IUserService userService)
		{
			_userService = userService;
		}
		[Route("signin")]
		public IActionResult SignIn()
		{
			return View(new SignInModel());
		}

		[Route("signin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SignIn(SignInModel model, string returnUrl = null)
		{
			if (ModelState.IsValid)
			{
				User user;
				if (await _userService.ValidateCredentials(model.Email, out user))
				{
					await SignInUser(user.Email);
					if (returnUrl != null)
					{
						return Redirect(returnUrl);
					}
					return RedirectToAction("Landing", "Home");
				}
			}
			return View(model);
		}

		private async Task SignInUser(string email)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, email),
				new Claim(ClaimTypes.Email, email),
				new Claim("name", email)
			};
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", null);
			var principal = new ClaimsPrincipal(claimsIdentity);
			await HttpContext.SignInAsync(principal);
		}

		public async Task<IActionResult> SignOut()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

		[Route("signup")]
		public IActionResult SignUp()
		{
			return View(new SignUpModel());
		}

		[Route("signup")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> SignUp(SignUpModel model)
		{
			if (ModelState.IsValid)
			{
				if (await _userService.AddUser(model))
				{
					await SignInUser(model.Email);
					return RedirectToAction("Landing", "Home");
				}
				ModelState.AddModelError("Error", "Could not add user. Username already in use...");
			}
			return View(model);
		}
	}
}