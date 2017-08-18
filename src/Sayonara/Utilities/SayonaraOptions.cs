using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sayonara.Utilities
{
	public class SayonaraOptions
	{
		public string ApplicationName { get; set; }
    public string ExtractFolder { get; set; }
		public string ExtractLetterPath { get; set; }
		public string SMTPServer { get; set; }
		public string SMTPPort { get; set; }
		public string SMTPUsername { get; set; }
		public string SMTPPassword { get; set; }
	}
}
