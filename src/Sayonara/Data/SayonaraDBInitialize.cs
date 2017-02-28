using System;
using System.Linq;
using Sayonara.Models;

namespace Sayonara.Data
{
    public static class SayonaraDBInitialize
    {
			public static void Initialize(Sayonara.Data.SayonaraContext context)
			{
				if (context.Database.EnsureCreated())
				{					
					if (!context.Extracts.Any())
					{
						context.Facilities.AddRange(
							new Facility()
							{
								ID = 1,
								Name = "Kyle's Facility",
								Alias = "Kyle's Facility"
							},
							new Facility()
							{
								ID = 2,
								Name = "Sean's Facility",
								Alias = "Sean's Facility"
							},
							new Facility()
							{
								ID = 3,
								Name = "Ryan's Facility",
								Alias = "Ryan's Facility"
							}
						);
						context.SaveChanges();						
						context.Extracts.AddRange(
							new Extract()
							{								
								FacilityID = 1,
								ExtractionDate = Convert.ToDateTime("3/1/2017"),
								Format = ExtractType.CSV
							},
							new Extract()
							{								
								FacilityID = 2,
								ExtractionDate = Convert.ToDateTime("2/23/2017"),
								Format = ExtractType.PDF
							}
						);
						context.SaveChanges();
					}
				}
			}
    }
}
