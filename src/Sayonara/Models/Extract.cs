using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sayonara.Models
{
	public enum ExtractType
	{
		CSV,
		PDF
	}

	public class Extract
	{
		public int ID { get; set; }
		public ExtractType Format { get; set; }
		public int FacilityID { get; set; }
		public int? DocumentationViewID { get; set; }
		public DateTime ExtractionDate { get; set; }
		public DateTime CompletionDate { get; set; }
		public DateTime ShippedDate { get; set; }
		public DateTime ReceivedDate { get; set; }
		public string CreatedBy { get; set; }
		public int CurrentCount { get; set; }
		public int TotalCount { get; set; }
		public string Status { get; set; }
		public string FilePath { get; set; }

		public Facility Facility { get; set; }

		public DocumentationView DocumentationView { get; set; }
				
	}
}