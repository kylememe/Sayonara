using Microsoft.Data.Entity;

namespace Sayonara.Models
{
  public class SayonaraContext : DbContext
  {
		public DbSet<Extract> Extracts { get; set; }		
	}
}
