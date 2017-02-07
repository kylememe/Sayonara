using Microsoft.EntityFrameworkCore;

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
