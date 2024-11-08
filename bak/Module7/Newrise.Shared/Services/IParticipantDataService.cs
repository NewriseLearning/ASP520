using Newrise.Shared.Models;

namespace Newrise.Shared.Services {
	public interface IParticipantDataService {
		Task<UserSession> SignInAsync(string userId, string password);
		Task<Participant> AddParticipantAsync(NewParticipant participant);
		Task<bool> UpdatePhotoAsync(string id, byte[] photo);
		Task<Participant> GetParticipantAsync(string id);
		Task<List<Event>> GetParticipantEventsAsync(string id);
		Task<Participant> GetCurrentParticipantAsync();
		Task<List<Event>> GetCurrentParticipantEventsAsync();
	}
}
