using Microsoft.Data.Entity;

namespace Sayonara.Models
{	

	public class SayonaraContext : DbContext
  {
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Extract>(b =>
			{
				b.Key(e => e.ID);
				b.Property(e => e.ID).ForSqlServer().UseIdentity();
			});
    }
    
		public DbSet<Extract> Extracts { get; set; }		
	}
}
