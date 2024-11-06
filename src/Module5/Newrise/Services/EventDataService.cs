using Microsoft.EntityFrameworkCore;
using Newrise.Shared.Models;
using System.Data;
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

		public async Task<Event> GetEventAsync(string id) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				return await dc.Events.FindAsync(id);
			}
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

		public async Task<bool> HasParticipantAsync(string eventId, string participantId) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var eventItem = await dc.Events.Include(e => e.Participants).SingleOrDefaultAsync(
				e => e.Id == eventId);
				if (eventItem != null) return eventItem.Participants.SingleOrDefault(
				p => p.Id == participantId) != null;
				return false;
			}
		}

		public async Task AddParticipantAsync(string eventId, string participantId) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var transaction = await dc.Database.BeginTransactionAsync(
				IsolationLevel.RepeatableRead);
				try {
					var eventItem = await dc.Events.Include(e => e.Participants)
					.SingleOrDefaultAsync(e => e.Id == eventId);
					if (eventItem == null) throw new Exception($"Event '{eventId}' does not exist.");
					if (eventItem.Participants.SingleOrDefault(p => p.Id == participantId) != null)
						throw new Exception($"User '{participantId}' is already a participant of event '{eventId}.");
				if (eventItem.RemainingSeats == 0)
						throw new Exception($"Event '{eventId}' is full.");
					var participant = await dc.Participants.FindAsync(participantId);
					if (participant == null) throw new Exception(
					$"Participant '{participantId}' does not exist.");
					eventItem.Participants.Add(participant);
					eventItem.AllocatedSeats++;
					await dc.SaveChangesAsync();
					await transaction.CommitAsync();
				}
				catch (Exception) { await transaction.RollbackAsync(); throw; }
			}
		}

		public async Task RemoveParticipantAsync(string eventId, string participantId) {
			using (var dc = await _dcFactory.CreateDbContextAsync()) {
				var transaction = await dc.Database.BeginTransactionAsync(
				IsolationLevel.RepeatableRead);
				try {
					var eventItem = await dc.Events.Include(e => e.Participants)
					.SingleOrDefaultAsync(e => e.Id == eventId);
					if (eventItem == null) throw new Exception($"Event '{eventId}' does not exist.");
					var participant = eventItem.Participants.SingleOrDefault(
					p => p.Id == participantId);
					if (participant == null) throw new Exception(
					$"User '{participantId}' is not a participant of event '{eventId}.");
					eventItem.Participants.Remove(participant);
					eventItem.AllocatedSeats--;
					await dc.SaveChangesAsync();
					await transaction.CommitAsync();
				}
				catch (Exception) {
					await transaction.RollbackAsync();
					throw;
				}
			}
		}
	}
}
