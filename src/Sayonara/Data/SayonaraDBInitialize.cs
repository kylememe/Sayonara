using System;
using System.Linq;
using Sayonara.Models;

namespace Sayonara.Data
{
    public static class SayonaraDBInitialize
    {
			public static void Initialize(Sayonara.Data.SayonaraContext context)
			{			
				if (!context.Facilities.Any())
				{
					context.Facilities.AddRange(
						new Facility()
						{
							ID = 1,
							Name = "Kyle's Facility",
							Alias = "Kyle's Facility",
                            ContactName = "Kyle Smith",
                            Address1 = "7152 Cherry Blossom Lane",
                            City = "Gibsonia",
                            State = "PA",
                            ZipCode = "18080"
						},
						new Facility()
						{
							ID = 2,
							Name = "Sean's Facility",
							Alias = "Sean's Facility",
                            ContactName = "Sean Smith",
                            Address1 = "1535 Avenue of the Americas",
                            City = "New York",
                            State = "NY",
                            ZipCode = "18080"
                        },
						new Facility()
						{
							ID = 3,
							Name = "Ryan's Facility",
							Alias = "Ryan's Facility",
                            ContactName = "Ryan Smith",
                            Address1 = "124 Nascar Lane",
                            City = "Charlottle",
                            State = "NC",
                            ZipCode = "18080"
                        }
					);
					context.SaveChanges();
					context.DocumentationViews.AddRange(
						new DocumentationView()
						{
							ID = 1,
							MedicalRecordCopy = true,
							FacilityID = 1,
							Name = "PA Documents",
                            recActive = 1
						},
						new DocumentationView()
						{
							ID = 2,
							MedicalRecordCopy = false,
							FacilityID = 1,
							Name = "MD Documents",
                            recActive = 1
                        },
						new DocumentationView()
						{
							ID = 3,
							MedicalRecordCopy = false,
							FacilityID = 1,
							Name = "All Documents",
                            recActive = 1
                        },
						new DocumentationView()
						{
							ID = 4,
							MedicalRecordCopy = true,
							FacilityID = 2, 
							Name = "Clincian Docs",
                            recActive = 1
                        }
					);
					context.SaveChanges();				
					context.Extracts.AddRange(
						new Extract()
						{								
							FacilityID = 2,
							ExtractionDate = Convert.ToDateTime("3/1/2017"),
							Format = ExtractType.CSV,
							PublicID = System.Guid.NewGuid(),
							Status = Sayonara.Models.Extract.NotStartedStatus
						},
						new Extract()
						{								
							FacilityID = 1,
							ExtractionDate = Convert.ToDateTime("2/23/2017"),
							Format = ExtractType.PDF,
							DocumentationViewID = 1,
							PublicID = System.Guid.NewGuid(),
							Status = Sayonara.Models.Extract.NotStartedStatus
						}
					);
					context.SaveChanges();
				}				
			}
    }
}
