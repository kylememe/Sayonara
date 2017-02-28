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

			public ICollection<DocumentationView> DocumentationViews { get; set; }
    }
}
