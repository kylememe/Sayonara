using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Sayonara.Models
{
	public enum ExtractType
	{
		CSV,
		PDF
	}

	public class Extract : IValidatableObject
	{
		public static string NotStartedStatus = "Waiting to Start...";

		public int ID { get; set; }
		public Guid PublicID { get; set; }
		public ExtractType Format { get; set; }
		public int FacilityID { get; set; }
		public int? DocumentationViewID { get; set; }
		[Required]
		[DataType(DataType.Date)]
		public DateTime ExtractionDate { get; set; }
		public DateTime? CompletionDate { get; set; }
		public DateTime? ShippedDate { get; set; }
		public DateTime? ReceivedDate { get; set; }
		public string CreatedBy { get; set; }
		public int CurrentCount { get; set; }
		public int TotalCount { get; set; }
		public string Status { get; set; }
		public string FilePath { get; set; }
		public string Password { get; set; }

		//Properties for address
		public string ContactName { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }

		public Facility Facility { get; set; }

		public DocumentationView DocumentationView { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var results = new List<ValidationResult>();
			if (ExtractionDate < DateTime.Today.AddDays(-2)) //-2 because I think the azure website deployed is an an unknow timezone
			{
				var msg = new ValidationResult("Extraction date must be greater than or equal to Today");
				results.Add(msg);

			}
			return results;
		}
	}
}