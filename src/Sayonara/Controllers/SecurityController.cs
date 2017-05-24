﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http.Authentication;

namespace Sayonara.Controllers
{
    public class SecurityController : Controller
    {
			[HttpGet]
			public async Task Login()
			{
				if (HttpContext.User == null || !HttpContext.User.Identity.IsAuthenticated)
					await HttpContext.Authentication.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties { RedirectUri = "/" });
			}

			[HttpGet]
			public async Task LogOff()
			{
				if (HttpContext.User.Identity.IsAuthenticated)
				{
					await HttpContext.Authentication.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
					await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				}
			}

			[HttpGet]
			public async Task EndSession()
			{
				// If AAD sends a single sign-out message to the app, end the user's session, but don't redirect to AAD for sign out.
				await HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
	}
}
