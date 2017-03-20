using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sayonara.Models;
using Microsoft.Extensions.Logging;

namespace Sayonara.Controllers
{
	[Route("api/[controller]")]
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
		public async Task<IActionResult> Get(string query)
		{
			var facilities = await _sayonaraContext.Facilities
				.Where(f => f.Name.StartsWith(query))
				.ToAsyncEnumerable()
				.ToArray();

			return Ok(facilities);
		}

		public async Task<IActionResult> Seed(ICollection<Facility> facilities)
		{

			_logger.LogInformation("Facility Count" + facilities.Count);

			await _sayonaraContext.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE Facilities");			

			foreach(var facility in facilities)
			{
				_sayonaraContext.Facilities.Add(new Facility
				{
					ID = facility.ID,
					Alias = facility.Alias,
					Name = facility.Name
				});
			}

			await _sayonaraContext.SaveChangesAsync();

			return Ok();
		}
		
	}
}
