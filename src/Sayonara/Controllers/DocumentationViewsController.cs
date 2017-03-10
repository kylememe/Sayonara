using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayonara.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Sayonara.Controllers
{
	[Route("api/[controller]")]
	public class DocumentationViewsController : Controller
	{
		private Sayonara.Data.SayonaraContext _sayonaraContext;

		public DocumentationViewsController(Sayonara.Data.SayonaraContext context)
		{
			_sayonaraContext = context;
		}

		// GET: api/values
		[HttpGet]
		public async Task<IActionResult> Get(int facilityID)
		{
			var facility = await _sayonaraContext.Facilities.Include("DocumentationViews")
				.Where(f => f.ID == facilityID)
				.SingleAsync();						

			return Ok(facility.DocumentationViews.Select(view => new { view.ID, view.MedicalRecordCopy, view.Name }));
		}

	}
}
