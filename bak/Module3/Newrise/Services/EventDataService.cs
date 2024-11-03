using Microsoft.EntityFrameworkCore;
using Newrise.Shared.Models;

namespace Newrise.Services {
	public class EventDataService {
		private readonly IDbContextFactory<NewriseDbContext> _dcFactory;

		public EventDataService(IDbContextFactory<NewriseDbContext> dcFactory) {
			_dcFactory = dcFactory;
		}

		public async Task AddEventAsync(Event item) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				if (await dc.Events.FindAsync(item.Id) != null)
					throw new Exception($"Event with ID '{item.Id}' already exists.");
				await dc.Events.AddAsync(item);
				await dc.SaveChangesAsync();
			}
		}

		public async Task RemoveEventAsync(string id) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var item = await dc.Events.FindAsync(id);
				if (item != null) {
					dc.Events.Remove(item);
					await dc.SaveChangesAsync();
				}
			}
		}

		public async Task UpdateEventAsync(Event item) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var entry = dc.Events.Attach(item);
				entry.State = EntityState.Modified;
				await dc.SaveChangesAsync();
			}
		}

		public async Task<List<Event>> GetEventsAsync() {
			using (var dc = await _dcFactory.CreateDbContextAsync())
				return await dc.Events.ToListAsync();
		}
	}
}
