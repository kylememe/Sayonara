using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Sayonara.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Sayonara.Controllers
{
	public class ExtractController : Controller
	{		
		private Sayonara.Data.SayonaraContext _sayonaraContext;
		private readonly IOptions<Sayonara.Utilities.SayonaraOptions> _sayonaraOptions;
		private ILogger _logger;

		public ExtractController(Sayonara.Data.SayonaraContext context, ILogger<ExtractController> logger, IOptions<Sayonara.Utilities.SayonaraOptions> sayonaraOptions)
		{
			_sayonaraContext = context;
			_logger = logger;
			_sayonaraOptions = sayonaraOptions;
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

		public async Task<IActionResult> Detail(int id)
		{
			var extract = await _sayonaraContext.Extracts
				.Include(e => e.Facility)
				.AsNoTracking()
				.SingleOrDefaultAsync(e => e.ID == id);

			if (extract != null)
				return View(extract);
			else
				return StatusCode(404);
		}        

		[Route("api/Extract/CSV/Next")]		
		public async Task<IActionResult> NextCSVExtract(System.DateTime scheduledDate)
		{
			_logger.LogInformation("Trying to get next CSV Extract with scheduled date of " + scheduledDate.ToString());
			var nextExtract = await _sayonaraContext.Extracts
				.Where(e => e.Format == ExtractType.CSV && e.ExtractionDate <= scheduledDate && e.Status == Extract.NotStartedStatus)
				.OrderBy(e => e.ExtractionDate)
				.FirstOrDefaultAsync();			

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
		public async Task<IActionResult> Save([Bind("Format", "FacilityID", "ExtractionDate", "DocumentationViewID", "ContactName", "Address1", "Address2", "City", "State", "ZipCode")]Extract extract)
		{
			if (ModelState.IsValid)
			{
                string newPassword; 

                //Check if there's another extract. This way we should get CSV and PDF extracts to share same randomly generated password. 
                var currentExtract = await _sayonaraContext.Extracts
                .Where(e => e.FacilityID == extract.FacilityID)
                .OrderByDescending(e => e.ExtractionDate)
                .FirstOrDefaultAsync();

                if (currentExtract != null)
                    newPassword = currentExtract.Password;
                else
                    newPassword = Utilities.PasswordGenerator.Generate(20);

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
					Password = newPassword,
                    CreatedBy = User.Identity.Name,
                    ContactName = extract.ContactName,
                    Address1 = extract.Address1,
                    Address2 = extract.Address2,
                    City = extract.City,
                    State = extract.State,
                    ZipCode = extract.ZipCode
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
				if (extract.ExtractionDate <= System.DateTime.Now.AddDays(-2)) //-2 because I think the azure website deployed is an an unknow timezone
				{
					ViewBag.ErrorMessage = "Extraction date needs to be chosen and in the future!";
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
				if (extract.CompletionDate == null) //Don't want to update extracts that were already completed.
				{
					extract.CurrentCount = dto.CurrentCount;
					extract.TotalCount = dto.TotalCount;
					extract.Status = dto.Status;
					extract.CompletionDate = dto.CompletionDate;						
					await _sayonaraContext.SaveChangesAsync();

          //If email is setup, send a status email on complete (ComplettionDate.HasValue) or on failure (Status.Containts("failed")
					if((!String.IsNullOrEmpty(_sayonaraOptions.Value.SendGridAPIKey)) && ((dto.CompletionDate.HasValue) || (dto.Status.Contains("failed"))))
					{
						var extractFacility = await _sayonaraContext.Facilities.SingleOrDefaultAsync(f => f.ID == extract.FacilityID);						
						var client = new SendGridClient(_sayonaraOptions.Value.SendGridAPIKey);
						var msg = new SendGridMessage();

						msg.From = new EmailAddress("sayonarastatus@nethealth.com", _sayonaraOptions.Value.ApplicationName);
						msg.AddTo(new EmailAddress(extract.CreatedBy, "Extract Creator"));

						if (dto.CompletionDate.HasValue)
						{
							msg.Subject = extractFacility.Name + "'s extract is done";

							string path;
							if (extract.Format == ExtractType.CSV)
								path = _sayonaraOptions.Value.ExtractFolder + "\\CSV";
							else
								path = _sayonaraOptions.Value.ExtractFolder + "\\PDF";

							msg.PlainTextContent = "Extract is at " + path;
						}
						else
						{
							msg.Subject = extractFacility.Name + "'s extract did not complete";
							msg.PlainTextContent = "Extract did not succesfully complete. Check Status in Sayonara";
						}							
						
						var response = await client.SendEmailAsync(msg);
					}
				}
				return Ok();				
			}
			else
			{
				return StatusCode(404);
			}			
		}

        /********************************************************************************
         * 
         * Actions for Fedex Site Chrome integration
         * 
         * ******************************************************************************/

        [AllowAnonymous]
        [Route("api/Extract/Detail/{id:int}")]
        public async Task<IActionResult> APIDetail(int id)
        {
            var extract = await _sayonaraContext.Extracts.Where(e => e.ID == id).FirstOrDefaultAsync();

            if (extract != null)
                return Ok(new
                {
                    Contact = extract.ContactName,
                    Address1 = extract.Address1,
                    Address2 = extract.Address2,
                    City = extract.City,
                    State = extract.State,
                    ZipCode = extract.ZipCode
                });
            else
                return StatusCode(404);
        }

    }
}
