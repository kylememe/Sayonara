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
				
		public async Task<IActionResult> Index()
		{
			return View(await _sayonaraContext.Extracts.Include(e => e.Facility).ToListAsync());
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Save([Bind("Format", "FacilityID", "ExtractionDate")]Extract extract)
		{
			_sayonaraContext.Extracts.Add(new Extract
			{
				FacilityID = extract.FacilityID,
				ExtractionDate = extract.ExtractionDate,
				Format = extract.Format
			});

			_sayonaraContext.SaveChanges();
			return View("Index", _sayonaraContext.Extracts);
		}

	}
}
