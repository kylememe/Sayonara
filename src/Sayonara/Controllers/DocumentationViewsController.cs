using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayonara.Models;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Sayonara.Controllers
{	
	public class DocumentationViewsController : Controller
	{
		private Sayonara.Data.SayonaraContext _sayonaraContext;
		private ILogger _logger;

		public DocumentationViewsController(Sayonara.Data.SayonaraContext context, ILogger<DocumentationViewsController> logger)
		{
			_sayonaraContext = context;
			_logger = logger;
		}

		// GET: api/values
		[HttpGet]
		[Route("api/DocumentationViews")]
		public async Task<IActionResult> Get(int facilityID)
		{
			var facility = await _sayonaraContext.Facilities.Include("DocumentationViews")
				.Where(f => f.ID == facilityID)
				.SingleAsync();						

			return Ok(facility.DocumentationViews
				.Where(view => view.recActive == 1)
				.Select(view => new { view.ID, view.MedicalRecordCopy, view.Name })
				);

		}

		[HttpPost]
		[Route("api/DocumentationViews/Seed")]
		public async Task<IActionResult> Seed([FromBody] ICollection<DocumentationView> views)
		{
			_logger.LogInformation("In DocumentationViews Seed with " + views.Count + " views");
			foreach (var view in views)
			{
				var currentView = await _sayonaraContext.DocumentationViews.Where(v => v.ID == view.ID).FirstOrDefaultAsync();
				if (currentView != null)
				{
					currentView.Name = view.Name;
					currentView.MedicalRecordCopy = view.MedicalRecordCopy;
					currentView.recActive = view.recActive;
					_sayonaraContext.DocumentationViews.Update(currentView);
				}
				else
				{
					_sayonaraContext.DocumentationViews.Add(new DocumentationView
					{
						ID = view.ID,
						FacilityID = view.FacilityID,
						Name = view.Name,
						MedicalRecordCopy = view.MedicalRecordCopy,
						recActive = view.recActive
				});
				}
			}

			var success = false;
			try
			{
				await _sayonaraContext.SaveChangesAsync();
				success = true;
			}
			catch(Exception ex)
			{
				_logger.LogError("DocumentationView Seed failed with message: " + ex.Message);
			}

			if (success)
				return Ok();
			else
				return StatusCode(500);
		}
	}
}
