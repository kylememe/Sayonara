using Microsoft.EntityFrameworkCore;
using Sayonara.Models;

namespace Sayonara.Data
{	
	public class SayonaraContext : DbContext
  {
		public SayonaraContext(DbContextOptions<SayonaraContext> options) : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			
			//modelBuilder.Entity<Extract>().HasOne<>
    }    
		public DbSet<Extract> Extracts { get; set; }		

		public DbSet<Facility> Facilities { get; set; }

		public DbSet<DocumentationView> DocumentationViews { get; set; }
	}
}
