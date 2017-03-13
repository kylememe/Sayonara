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
		private string notStartedStatus = "Waiting to Start...";
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

		[Route("api/Extract/CSV/Next")]
		public async Task<IActionResult> NextCSVExtract(System.DateTime scheduledDate)
		{
			var nextExtract = await _sayonaraContext.Extracts
				.Where(e => e.Format == ExtractType.CSV && e.ExtractionDate <= scheduledDate && e.Status == notStartedStatus)
				.OrderBy(e => e.ExtractionDate)
				.FirstOrDefaultAsync();

			var newPassword = new Utilities.PasswordGenerator();

			if (nextExtract != null)
			{
				return Ok(new
				{
					ExtractID = nextExtract.PublicID,
					FacilityID = nextExtract.FacilityID,
					password = newPassword.Password
				});
			}
			else
			{
				return StatusCode(404);
			}				
		}

		[Route("api/Extract/PDF/Next")]
		public async Task<IActionResult> NextPDFExtract(System.DateTime scheduledDate)
		{
			var nextExtract = await _sayonaraContext.Extracts
				.Where(e => e.Format == ExtractType.PDF && e.ExtractionDate <= scheduledDate && e.Status == notStartedStatus)
				.OrderBy(e => e.ExtractionDate)
				.FirstOrDefaultAsync();

			var newPassword = new Utilities.PasswordGenerator();

			if (nextExtract != null)
			{
				return Ok(new
				{
					ExtractID = nextExtract.PublicID,
					FacilityID = nextExtract.FacilityID,
					password = newPassword.Password,
					DocumentationViewID = nextExtract.DocumentationViewID
				});
			}
			else
			{
				return StatusCode(404);
			}
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
					Status = notStartedStatus,
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

		[HttpPut]
		[Route("api/Extract/Status")]
		public async Task<IActionResult> Status(Sayonara.ViewModels.ExtractStatus status)
		{
			var extract = await _sayonaraContext.Extracts.SingleOrDefaultAsync(e => e.PublicID.Equals(status.PublicID));

			if (extract == null)
			{
				return StatusCode(404);
			}
			else
			{
				if(extract.CompletionDate == null)
				{
					extract.CurrentCount = status.CurrentCount;
					extract.TotalCount = status.TotalCount;
					extract.Status = status.Status;
					await _sayonaraContext.SaveChangesAsync();
				}

				return Ok();
			}				
		}

	}
}
