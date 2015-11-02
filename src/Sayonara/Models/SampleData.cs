using Microsoft.Data.Entity;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Linq;

namespace Sayonara.Models
{
	public static class SampleData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			var context = serviceProvider.GetService<SayonaraContext>();
			if (context.Database.EnsureCreated())
			{
				if (!context.Extracts.Any())
				{					
					context.Extracts.AddRange(
						new Extract()
						{
							ID = 1,
              FacilityID = 1,
							ExtractionDate = Convert.ToDateTime("10/15/2015"),
							Format = ExtractType.CSV
						},
						new Extract()
						{
							ID = 2,
							FacilityID = 2032,
							ExtractionDate = Convert.ToDateTime("11/12/2015"),
							Format = ExtractType.PDF
						}							
					);
					context.SaveChanges();
				}
			}
		}
	}
}
