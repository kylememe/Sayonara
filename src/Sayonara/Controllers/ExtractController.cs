using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Sayonara.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Sayonara.Controllers
{
  public class ExtractController : Controller
  {
		private Sayonara.Data.SayonaraContext _sayonaraContext;
		public ExtractController(Sayonara.Data.SayonaraContext context)
		{
			_sayonaraContext = context;
		}
    // GET: /<controller>/
    public async Task<IActionResult> Index()
    {
			return View(await _sayonaraContext.Extracts.ToListAsync());
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
