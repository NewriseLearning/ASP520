using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newrise.Services;
using Newrise.Shared.Models;

namespace Newrise.Controllers {
	[ApiController]
	[Authorize, Route("api/participants")]
	public class ParticipantController : ControllerBase {
		private readonly ParticipantDataService _participantDataService;
		private readonly ILogger<ParticipantController> _logger;

		public ParticipantController(
			ParticipantDataService participantDataService,
			ILogger<ParticipantController> logger) {
			_participantDataService = participantDataService;
			_logger = logger;
		}

		[HttpPut, AllowAnonymous]
		public async Task<ApiResult<Participant>> AddParticipant([FromBody] NewParticipant participant) {
			try {
				await _participantDataService.AddParticipantAsync(participant);
				_logger.LogInformation($"Participant {participant.Id} has been added.");
				var value = await _participantDataService.GetParticipantAsync(participant.Id);
				value.PasswordHash = null; return new ApiResult<Participant>(value);
			} catch (Exception ex) {
				return new ApiResult<Participant>(ex.Message);
			}
		}

		[HttpPost, AllowAnonymous, Route("login")]
		public async Task<ApiResult<UserSession>> Login([FromBody] LoginInfo login) {
			try {
				var value = await _participantDataService.SignInAsync(login.UserID, login.Password);
				_logger.LogInformation($"Participant {value.UserId} has logged-in.");
				value.GenerateToken(); return new ApiResult<UserSession>(value);
			} catch (Exception ex) {
				return new ApiResult<UserSession>(ex.Message);
			}
		}

		private void EnsureAdminOrSelf(string userId) {
			if (!User.IsInRole("admin") && User.Identity.Name != userId)
				throw new Exception("You are not allowed to perform this operation.");
		}

		[HttpGet, Route("{id}")]
		public async Task<ApiResult<Participant>> GetParticipant(string id) {
			try {
				EnsureAdminOrSelf(id);
				var value = await _participantDataService.GetParticipantAsync(id);
				value.PasswordHash = null; return new ApiResult<Participant>(value);
			} catch (Exception ex) {
				return new ApiResult<Participant>(ex.Message);
			}
		}

		[HttpGet, Route("{id}/events")]
		public async Task<ApiResult<List<Event>>> GetEventsForParticipant(string id) {
			try {
				EnsureAdminOrSelf(id);
				var participant = await _participantDataService.GetParticipantWithEventsAsync(id);
				var value = participant.Events.ToList(); // return events only, not the participant
				return new ApiResult<List<Event>>(value);
			}
			catch (Exception ex) {
				return new ApiResult<List<Event>>(ex.Message);
			}
		}

		[HttpPost, Route("{id}/photo")]
		public async Task<ApiResult<bool>> UpdateParticipantPhoto(string id, [FromBody] byte[] photo) {
			try {
				EnsureAdminOrSelf(id);
				await _participantDataService.UpdatePhotoAsync(id, photo);
				_logger.LogInformation($"Participant {id} updated their photo.");
				return new ApiResult<bool>(true);
			}
			catch (Exception ex) {
				return new ApiResult<bool>(ex.Message);
			}
		}
	}
}
