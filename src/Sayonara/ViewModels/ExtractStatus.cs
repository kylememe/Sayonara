using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sayonara.ViewModels
{
    public class ExtractStatus
    {
			public string PublicID { get; set; }
			public DateTime? CompletionDate { get; set; }
			public string Status { get; set; }
			public int TotalCount { get; set; }
			public int CurrentCount { get; set; }
    }
}
