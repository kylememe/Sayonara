using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sayonara.Models
{
    public class Facility
    {

			[DatabaseGenerated(DatabaseGeneratedOption.None)]
			public int ID { get; set; }

			public string Name { get; set; }

			public string Alias { get; set; }

			//Properties for address
			public string ContactName { get; set; }
			public string Address1 { get; set; }
			public string Address2 { get; set; }
			public string City { get; set; }
			public string State { get; set; }
			public string ZipCode { get; set; }

			public ICollection<DocumentationView> DocumentationViews { get; set; }
    }
}
