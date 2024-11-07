using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newrise.Services;
using Newrise.Shared.Models;

namespace Newrise.Controllers {
	[ApiController]
	[Authorize, Route("api/events")]
	public class EventController : ControllerBase {
		private readonly EventDataService _eventDataService;
		private readonly ILogger<EventController> _logger;

		public EventController(
			EventDataService eventDataService,
			ILogger<EventController> logger) {
			_eventDataService = eventDataService;
			_logger = logger;
		}

		[HttpGet, AllowAnonymous]
		public async Task<ApiResult<List<Event>>> GetEvents() {
			try {
				var value = await _eventDataService.GetEventsAsync();
				return new ApiResult<List<Event>>(value);
			}
			catch (Exception ex) {
				return new ApiResult<List<Event>>(ex.Message);
			}
		}

		[HttpGet, Route("{id}")]
		public async Task<ApiResult<Event>> GetEvent(string id) {
			try {
				var value = await _eventDataService.GetEventAsync(id);
				return new ApiResult<Event>(value);
			}
			catch (Exception ex) {
				return new ApiResult<Event>(ex.Message);
			}
		}

		[HttpPut, Authorize(Roles = "admin")]
		public async Task<ApiResult<Event>> AddEvent([FromBody] Event item) {
			try {
				await _eventDataService.AddEventAsync(item);
				var value = await _eventDataService.GetEventAsync(item.Id);
				_logger.LogInformation($"Event {item.Id} added.");
				return new ApiResult<Event>(value);
			}
			catch (Exception ex) {
				return new ApiResult<Event>(ex.Message);
			}
		}

		[HttpPost, Authorize(Roles = "admin")]
		public async Task<ApiResult<Event>> UpdateEvent([FromBody] Event item) {
			try {
				await _eventDataService.UpdateEventAsync(item);
				var value = await _eventDataService.GetEventAsync(item.Id);
				_logger.LogInformation($"Event {item.Id} updated.");
				return new ApiResult<Event>(value);
			}
			catch (Exception ex) {
				return new ApiResult<Event>(ex.Message);
			}
		}

		[HttpDelete, Route("{id}"), Authorize(Roles = "admin")]
		public async Task<ApiResult<Event>> RemoveEvent(string id) {
			try {
				var value = await _eventDataService.GetEventAsync(id);
				await _eventDataService.RemoveEventAsync(id);
				_logger.LogInformation($"Event {id} removed.");
				return new ApiResult<Event>(value);
			}
			catch (Exception ex) {
				return new ApiResult<Event>(ex.Message);
			}
		}

		[HttpGet, Route("{eventId}/participants/{participantId}")]
		public async Task<ApiResult<bool>> CheckEventHasParticipant(string eventId, string participantId) {
			try {
				var value = await _eventDataService.HasParticipantAsync(eventId, participantId);
				return new ApiResult<bool>(value);
			}
			catch (Exception ex) {
				return new ApiResult<bool>(ex.Message);
			}
		}

		[HttpPut, Route("{eventId}/participants/{participantId}")]
		public async Task<ApiResult<bool>> AddParticipantToEvent(string eventId, string participantId) {
			try {
				await _eventDataService.AddParticipantAsync(eventId, participantId);
				_logger.LogInformation($"Participant {participantId} added to event {eventId}.");
				return new ApiResult<bool>(true);
			}
			catch (Exception ex) {
				return new ApiResult<bool>(ex.Message);
			}
		}

		[HttpDelete, Route("{eventId}/participants/{participantId}")]
		public async Task<ApiResult<bool>> RemoveParticipantFromEvent(string eventId, string participantId) {
			try {
				await _eventDataService.RemoveParticipantAsync(eventId, participantId);
				_logger.LogInformation($"Participant {participantId} removed from event {eventId}.");
				return new ApiResult<bool>(true);
			}
			catch (Exception ex) {
				return new ApiResult<bool>(ex.Message);
			}
		}
	}
}
