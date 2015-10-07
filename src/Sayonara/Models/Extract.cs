using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using System.Collections.Generic;
using System.Linq; 

namespace Sayonara.Models
{
  public enum ExtractType
  {
    CSV,
    PDF
  }

  public class SayonaraContext : DbContext
  {
    public DbSet<Extract> Extracts { get; set; }

    /*
    protected override void OnConfiguring(DbContextOptions builder)
    {
      builder.UseSqlServer(@"Server=(localdb)\v11.0;Database=Sayonara;Trusted_Connection=True;");
    }
    */

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Extract>();
		}
	}

    public class Extract
    {
      public int ID { get; set; }
      public ExtractType Format { get; set; }
      public int FacilityID { get; set; }
      public int DocumentationViewID { get; set; }
      public DateTime ExtractionDate { get; set; }
      public DateTime CompletionDate { get; set; }
      public DateTime ShippedDate { get; set; }
      public DateTime ReceivedDate { get; set; }
      public string CreatedBy { get; set; }
      public int CurrentCount { get; set; }
      public int TotalCount { get; set; }
      public string Status { get; set; }
      public string FilePath { get; set; }


    }
}