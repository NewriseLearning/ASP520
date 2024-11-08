using Newrise.Shared.Models;
using Newrise.Shared.Services;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Newrise.Client.Services {
	public class EventDataClient : IEventDataService {
		private const string _route = "api/events";
		private readonly HttpClient _client;
		private readonly JsonSerializerOptions _json_options;
		public EventDataClient(HttpClient client) {
			_client = client; _json_options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true
			};
		}

		public async Task<Event> GetEventAsync(string id) {
			var response = await _client.GetAsync($"{_route}/{id}");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<Event>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}

		public async Task<Event> AddEventAsync(Event item) {
			var response = await _client.PutAsJsonAsync(_route, item);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new Exception("You are not allowed to add an event.");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<Event>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}

		public async Task<Event> UpdateEventAsync(Event item) {
			var response = await _client.PostAsJsonAsync(_route, item);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new Exception("You are not allowed to add an event.");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<Event>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}


		public async Task<Event> RemoveEventAsync(string id) {
			var response = await _client.DeleteAsync($"{_route}/{id}");
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new Exception("You are not allowed to remove an event.");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<Event>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}

		public async Task<List<Event>> GetEventsAsync() {
			var response = await _client.GetAsync(_route);
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<List<Event>>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}

		public async Task<bool> HasParticipantAsync(string eventId, string participantId) {
			var response = await _client.GetAsync($"{_route}/{eventId}/participants/{participantId}");
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new Exception("You are not allowed to check event participant.");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<bool>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}

		public async Task<bool> AddParticipantAsync(string eventId, string participantId) {
			var response = await _client.PutAsJsonAsync(
				$"{_route}/{eventId}/participants/{participantId}", string.Empty);
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new Exception("You are not allowed to add event participant.");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<bool>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}

		public async Task<bool> RemoveParticipantAsync(string eventId, string participantId) {
			var response = await _client.DeleteAsync($"{_route}/{eventId}/participants/{participantId}");
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new Exception("You are not allowed to remove event participant.");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<bool>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}
	}
}
