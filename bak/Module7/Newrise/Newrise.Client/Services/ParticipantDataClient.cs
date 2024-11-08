using Newrise.Shared.Models;
using Newrise.Shared.Services;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

namespace Newrise.Client.Services {
	public class ParticipantDataClient : IParticipantDataService {
		private const string _route = "api/participants";
		private readonly HttpClient _client;
		private readonly JsonSerializerOptions _json_options;
		public ParticipantDataClient(HttpClient client) {
			_client = client; _json_options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true
			};
		}

		public Task<UserSession> SignInAsync(string userId, string password) {
			throw new NotImplementedException();
		}

		public async Task<Participant> AddParticipantAsync(NewParticipant item) {
			var response = await _client.PutAsJsonAsync(_route, item);
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<Participant>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}

		public Task<bool> UpdatePhotoAsync(string id, byte[] photo) {
			throw new NotImplementedException();
		}

		public async Task<Participant> GetParticipantAsync(string id) {
			var response = await _client.GetAsync($"{_route}/{id}");
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new Exception("You are not allowed to access the participant.");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<Participant>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}

		public async Task<List<Event>> GetParticipantEventsAsync(string id) {
			var response = await _client.GetAsync($"{_route}/{id}/events");
			if (response.StatusCode == HttpStatusCode.Unauthorized)
				throw new Exception("You are not allowed to access participant events.");
			if (response.StatusCode != HttpStatusCode.OK)
				throw new Exception($"Network or server error ({response.StatusCode})");
			var content = await response.Content.ReadAsStringAsync();
			var result = JsonSerializer.Deserialize<ApiResult<List<Event>>>(content, _json_options);
			if (!result.Success) throw new Exception(result.Error);
			return result.Value;
		}

		public Task<Participant> GetCurrentParticipantAsync() {
			throw new NotImplementedException();
		}

		public Task<List<Event>> GetCurrentParticipantEventsAsync() {
			throw new NotImplementedException();
		}


	}
}
