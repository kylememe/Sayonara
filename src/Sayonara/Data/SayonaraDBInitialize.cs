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
						context.Extracts.AddRange(
							new Extract()
							{								
								FacilityID = 1,
								ExtractionDate = Convert.ToDateTime("3/1/2017"),
								Format = ExtractType.CSV
							},
							new Extract()
							{								
								FacilityID = 2032,
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
