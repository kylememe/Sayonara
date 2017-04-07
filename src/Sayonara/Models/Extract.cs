﻿using System;
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

		public Facility Facility { get; set; }

		public DocumentationView DocumentationView { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			var results = new List<ValidationResult>();
			if (ExtractionDate < DateTime.Today)
			{
				var msg = new ValidationResult("Expire date must be greater than or equal to Today");
				results.Add(msg);

			}
			return results;
		}
	}
}