using Microsoft.Data.Entity;

namespace Sayonara.Models
{	

	public class SayonaraContext : DbContext
  {
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			
    }
    
		public DbSet<Extract> Extracts { get; set; }		
	}
}
