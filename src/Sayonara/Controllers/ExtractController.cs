using Microsoft.AspNetCore.Mvc;
using Sayonara.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Sayonara.Controllers
{
  public class ExtractController : Controller
  {
    // GET: /<controller>/
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Add()
    {
      return View();
    }

		public IActionResult Save(Extract extract)
		{
			return View("Index");
		}
      
  }
}
