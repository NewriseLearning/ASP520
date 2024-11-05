using Microsoft.EntityFrameworkCore;
using Newrise.Shared.Models;
using static MudBlazor.CategoryTypes;

namespace Newrise.Services {
	public class EventDataService {
		readonly IDbContextFactory<NewriseDbContext> _dcFactory;
		readonly ILogger<EventDataService> _logger;

		public EventDataService(
			IDbContextFactory<NewriseDbContext> dcFactory,
			ILogger<EventDataService> logger) {
			_dcFactory = dcFactory;
			_logger = logger;
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
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				return await dc.Events.ToListAsync();
			}
		}
	}
}
