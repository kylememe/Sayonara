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
			return View(await _sayonaraContext.Extracts
				.Include(e => e.Facility)
				.Include(v => v.DocumentationView)
				.OrderByDescending(v => v.ExtractionDate).
				ToListAsync());
		}

		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Save([Bind("Format", "FacilityID", "ExtractionDate", "DocumentationViewID")]Extract extract)
		{
			if (ModelState.IsValid)
			{
				_sayonaraContext.Extracts.Add(new Extract
				{
					PublicID = System.Guid.NewGuid(),
					FacilityID = extract.FacilityID,
					ExtractionDate = extract.ExtractionDate,
					Format = extract.Format,
					DocumentationViewID = extract.DocumentationViewID,
					Status = "Waiting to start...",
					TotalCount = 0,
					CurrentCount = 0
				});

				_sayonaraContext.SaveChanges();

				return View("Index", _sayonaraContext.Extracts
					.Include(e => e.Facility)
					.ThenInclude(f => f.DocumentationViews)
					.OrderByDescending(e => e.ExtractionDate)
					.ToList());
			}
			else
			{
				if (extract.FacilityID == 0)
				{
					ViewBag.ErrorMessage = "A Facility needs to be chosen";
					return View("Add");
				}
				if (extract.ExtractionDate <= System.DateTime.Now)
				{
					ViewBag.ErrorMessage = "Extraction date needs to be chosen and in the future.";
					return View("Add");
				}

				ViewBag.ErrorMessage = "An unknown problem with your selections occured.";
				return View("Add");

			}
			
		}

	}
}
