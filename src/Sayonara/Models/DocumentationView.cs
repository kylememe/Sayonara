using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sayonara.Models
{
    public class DocumentationView
    {
			[DatabaseGenerated(DatabaseGeneratedOption.None)]
			public int ID { get; set; }

			public int FacilityID { get; set; }
			
			public string Name { get; set; }

			public bool MedicalRecordCopy { get; set; }

			public int recActive { get; set; }

			public Facility Facility { get; set; }
			
    }
}
