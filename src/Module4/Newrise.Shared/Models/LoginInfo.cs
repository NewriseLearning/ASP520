using System.ComponentModel.DataAnnotations;

namespace Newrise.Shared.Models {
	public class LoginInfo {
		[Display(Name = "ID/Email")]
		[Required(ErrorMessage = "{0} is required.")]
		[StringLength(254, ErrorMessage = "{0} cannot be more than {1} characters.")]
		public string UserID { get; set; }
		[Required(ErrorMessage = "{0} is required.")]
		[StringLength(16, ErrorMessage = "{0} cannot be more than {1} characters.")]
		[RegularExpression(NewParticipant.PasswordFormat, ErrorMessage = "{0} is invalid.")]
		public string Password {
			get; set;
		}
	}
}
