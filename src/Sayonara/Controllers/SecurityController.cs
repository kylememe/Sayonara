using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sayonara.Controllers
{
    public class SecurityController : Controller
    {
		public IActionResult Login()
		{
			return View();
		}

		public IActionResult Logout()
		{			
			return View();
		}
	}
}
