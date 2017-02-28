using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Sayonara.Controllers
{
	[Route("api/[controller]")]
	public class FacilitiesController : Controller
	{
		private Sayonara.Data.SayonaraContext _sayonaraContext;

		public FacilitiesController(Sayonara.Data.SayonaraContext context)
		{
			_sayonaraContext = context;
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
		
	}
}
