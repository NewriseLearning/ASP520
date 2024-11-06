using System.ComponentModel.DataAnnotations;

namespace Newrise.Shared.Models {
	public class NewParticipant : Participant {
		public const string PasswordFormat = @"(?=.*[A-Z])^(?=.*[a-z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*#?&]{8,16}$";

		[Required(ErrorMessage = "{0} is required.")]
		[StringLength(16, ErrorMessage = "{0} cannot be more than {1} characters.")]
		[RegularExpression(PasswordFormat, ErrorMessage = "{0} is not valid.")]
		public string Password { get; set; }

		[Display(Name = "Confirmation password")]
		[Required(ErrorMessage = "{0} is required.")]
		[StringLength(16, ErrorMessage = "{0} cannot be more than {1} characters.")]
		[RegularExpression(PasswordFormat, ErrorMessage = "{0} is not valid.")]
		[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
