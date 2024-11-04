using Microsoft.EntityFrameworkCore;
using Newrise.Shared.Models;

namespace Newrise.Services {
	public class NewriseDbContext : DbContext {
		public DbSet<Event> Events { get; set; }
		public DbSet<Participant> Participants { get; set; }
		public NewriseDbContext(
			DbContextOptions<NewriseDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Participant>().HasAlternateKey(p => p.Email);
			modelBuilder.Entity<Event>().Property("Fee").HasColumnType("decimal").HasPrecision(7, 2);
		}
	}
}
