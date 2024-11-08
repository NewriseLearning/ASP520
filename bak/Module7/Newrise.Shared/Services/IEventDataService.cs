using Newrise.Shared.Models;

namespace Newrise.Shared.Services {
	public interface IEventDataService {
		Task<Event> GetEventAsync(string id);
		Task<Event> AddEventAsync(Event item);
		Task<Event> RemoveEventAsync(string id);
		Task<Event> UpdateEventAsync(Event item);
		Task<List<Event>> GetEventsAsync();
		Task<bool> HasParticipantAsync(string eventId, string participantId);
		Task<bool> AddParticipantAsync(string eventId, string participantId);
		Task<bool> RemoveParticipantAsync(string eventId, string participantId);
	}
}
