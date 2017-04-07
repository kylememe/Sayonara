using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Sayonara.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Sayonara.Controllers
{
	public class ExtractController : Controller
	{		
		private Sayonara.Data.SayonaraContext _sayonaraContext;
		private ILogger _logger;
		public ExtractController(Sayonara.Data.SayonaraContext context, ILogger<ExtractController> logger)
		{
			_sayonaraContext = context;
			_logger = logger;
		}

		public IActionResult Add()
		{
			return View();
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
			_logger.LogInformation("Trying to get next CSV Extract with scheduled date of " + scheduledDate.ToString());
			var nextExtract = await _sayonaraContext.Extracts
				.Where(e => e.Format == ExtractType.CSV && e.ExtractionDate <= scheduledDate && e.Status == Extract.NotStartedStatus)
				.OrderBy(e => e.ExtractionDate)
				.FirstOrDefaultAsync();

			var newPassword = new Utilities.PasswordGenerator();

			if (nextExtract != null)
			{
				return Ok(new
				{
					PublicID = nextExtract.PublicID,
					FacilityID = nextExtract.FacilityID,
					password = nextExtract.Password
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
			_logger.LogInformation("Trying to get next PDF Extract with scheduled date of " + scheduledDate.ToString());
			var nextExtract = await _sayonaraContext.Extracts
				.Where(e => e.Format == ExtractType.PDF && e.ExtractionDate <= scheduledDate && e.Status == Extract.NotStartedStatus)
				.OrderBy(e => e.ExtractionDate)
				.FirstOrDefaultAsync();			

			if (nextExtract != null)
			{
				return Ok(new
				{
					PublicID = nextExtract.PublicID,
					FacilityID = nextExtract.FacilityID,
					password = nextExtract.Password,
					DocumentationViewID = nextExtract.DocumentationViewID
				});
			}
			else
			{
				return StatusCode(404);
			}
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
					Status = Extract.NotStartedStatus,
					TotalCount = 0,
					CurrentCount = 0,
					Password = Utilities.PasswordGenerator.Generate(12)
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

		[HttpPost]		
		[Route("api/Extract/Delete")]
		public async Task<IActionResult> Delete(string PublicID)
		{
			if (String.IsNullOrEmpty(PublicID))
				return StatusCode(404);

			var extract = await _sayonaraContext.Extracts
				.AsNoTracking()
				.SingleOrDefaultAsync(e => e.PublicID.Equals(new System.Guid(PublicID)));

			if (extract != null)
			{
				_sayonaraContext.Extracts.Remove(extract);
				await _sayonaraContext.SaveChangesAsync();
				return Ok();
			}
			else
			{
				return StatusCode(404);
			}
		}

		[HttpPut]
		[Route("api/Extract/Status")]
		public async Task<IActionResult> Status([FromBody]Sayonara.ViewModels.ExtractStatus dto)
		{
			if (String.IsNullOrEmpty(dto.PublicID))
				return StatusCode(404);			
			
			var extract = await _sayonaraContext.Extracts.SingleOrDefaultAsync(e => e.PublicID.Equals(new System.Guid(dto.PublicID)));

			if (extract != null)
			{
				if (extract.CompletionDate == null)
				{
					extract.CurrentCount = dto.CurrentCount;
					extract.TotalCount = dto.TotalCount;
					extract.Status = dto.Status;
					extract.CompletionDate = dto.CompletionDate;						
					await _sayonaraContext.SaveChangesAsync();
				}
				return Ok();				
			}
			else
			{
				return StatusCode(404);
			}			
		}

	}
}
