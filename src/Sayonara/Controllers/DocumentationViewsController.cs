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

			return Ok(facility.DocumentationViews.Select(view => new { view.ID, view.MedicalRecordCopy, view.Name }));
		}

		[HttpPost]
		[Route("api/DocumentationViews/Seed")]
		public async Task<IActionResult> Seed([FromBody] ICollection<DocumentationView> views)
		{
			_logger.LogInformation("Views Count" + views.Count);

			await _sayonaraContext.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE DocumentationViews");

			foreach (var view in views)
			{
				_sayonaraContext.DocumentationViews.Add(new DocumentationView
				{				 
					ID = view.ID,
					FacilityID = view.FacilityID,
					Name = view.Name,
					MedicalRecordCopy = view.MedicalRecordCopy					
				});
			}

			await _sayonaraContext.SaveChangesAsync();

			return Ok();
		}

	}
}
