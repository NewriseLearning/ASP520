using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newrise.Services;
using Newrise.Shared.Models;

namespace Newrise.Controllers {
	[Route("api/events")]
	[ApiController]
	public class EventController : ControllerBase {
		private readonly EventDataService _eventDataService;
		private readonly ILogger<EventController> _logger;

		public EventController(
			EventDataService eventDataService,
			ILogger<EventController> logger) {
			_eventDataService = eventDataService;
			_logger = logger;
		}

		[HttpGet]
		public async Task<ApiResult<List<Event>>> GetEvents() {
			try {
				var value = await _eventDataService.GetEventsAsync();
				return new ApiResult<List<Event>>(value);
			} catch (Exception ex) {
				return new ApiResult<List<Event>>(ex.Message);
			}
		}

		[HttpGet, Route("{id}")]
		public async Task<ApiResult<Event>> GetEvent(string id) {
			try {
				var value = await _eventDataService.GetEventAsync(id);
				return new ApiResult<Event>(value);
			} catch (Exception ex) {
				return new ApiResult<Event>(ex.Message);
			}
		}

		[HttpPut]
		public async Task<ApiResult<Event>> AddEvent([FromBody] Event item) {
			try {
				await _eventDataService.AddEventAsync(item);
				var value = await _eventDataService.GetEventAsync(item.Id);
				return new ApiResult<Event>(value);
			} catch (Exception ex) {
				return new ApiResult<Event>(ex.Message);
			}
		}

		[HttpPost]
		public async Task<ApiResult<Event>> UpdateEvent([FromBody] Event item) {
			try {
				await _eventDataService.UpdateEventAsync(item);
				var value = await _eventDataService.GetEventAsync(item.Id);
				return new ApiResult<Event>(value);
			} catch (Exception ex) {
				return new ApiResult<Event>(ex.Message);
			}
		}

		[HttpDelete]
		public async Task<ApiResult<Event>> RemoveEventAsync(string id) {
			try {
				var value = await _eventDataService.GetEventAsync(id);
				await _eventDataService.RemoveEventAsync(id);
				return new ApiResult<Event>(value);
			} catch (Exception ex) {
				return new ApiResult<Event>(ex.Message);
			}
		}

		//public async Task<ApiResult<bool>> HasParticipantAsync(string eventId, string participantId) {
		//	try {

		//	} catch (Exception ex) {
		//		return new ApiResult<bool>(ex.Message);
		//	}
		//}
	}
}


		//public async Task AddParticipantAsync(string eventId, string participantId);
		//public async Task<Event> GetEvent(string id) {
		//	try {

		//	} catch (Exception ex) {
		//		return new ApiResult<>(ex.Message);

		//	}
		//}
//		public async Task RemoveParticipantAsync(string eventId, string participantId);
//		public async Task<Event> GetEvent(string id) {
//			try {

//			} catch (Exception ex) {
//				return new ApiResult<>(ex.Message);

//			}
//		}

//	}
//}
