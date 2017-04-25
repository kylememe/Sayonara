using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayonara.Models;
using Sayonara.Utilities;
using Microsoft.Extensions.Logging;

namespace Sayonara.Controllers
{	
	public class FacilitiesController : Controller
	{
		private Sayonara.Data.SayonaraContext _sayonaraContext;
		private ILogger _logger;

		public FacilitiesController(Sayonara.Data.SayonaraContext context, ILogger<FacilitiesController> logger)
		{
			_sayonaraContext = context;
			_logger = logger;
		}

		// GET: api/values
		[HttpGet]
		[Route("api/Facilities")]
		public async Task<IActionResult> Get(string query)
		{
			if (query.StartsWith("@"))
			{				
				var facilities = await _sayonaraContext.Facilities
					.Where(f => f.ID == Convert.ToInt32(query.Substring(1)))
					.Select(f => new { Name = f.Alias + " (FacID: " + f.ID + ")", ID = f.ID, Alias = f.Alias })
					.ToAsyncEnumerable()
					.ToArray();

				return Ok(facilities);
			}
			else
			{
				var facilities = await _sayonaraContext.Facilities
					.Where(f => f.Alias.StartsWith(query))
					.Select(f => new { Name = f.Alias + " (FacID: " + f.ID + ")", ID = f.ID, Alias = f.Alias })
					.ToAsyncEnumerable()
					.ToArray();

				return Ok(facilities);
			}
		}

		[HttpPost]
		[Route("api/Facilities/Seed")]		
		public async Task<IActionResult> Seed([FromBody]ICollection<Facility> facilities)
		{
			_logger.LogInformation("In Facilities Seed with " + facilities.Count + " facilities");
			foreach(var facility in facilities)
			{
				var currentFacility = await _sayonaraContext.Facilities.Where(f => f.ID == facility.ID).FirstOrDefaultAsync();
				if(currentFacility != null)
				{					
					currentFacility.Name = facility.Name;
					currentFacility.Alias = facility.Alias;
					_sayonaraContext.Facilities.Update(currentFacility);										
				}
				else
				{					
					_sayonaraContext.Facilities.Add(new Facility
					{
						ID = facility.ID,
						Alias = facility.Alias,
						Name = facility.Name
					});
				}				
			}

			await _sayonaraContext.SaveChangesAsync();

			return Ok();
		}
		
	}
}
