using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;

namespace Sayonara.Models
{
    public class Facility
    {
			public int ID { get; set; }

			public string Name { get; set; }

			public string Alias { get; set; }

			public ICollection<DocumentationView> DocumentationViews { get; set; }
    }
}
