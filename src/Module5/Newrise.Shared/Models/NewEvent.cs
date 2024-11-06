namespace Newrise.Shared.Models {
	public class NewEvent : Event {
		public DateTime? FromDate {
			get { return From; }
			set {
				if (value != null) {
					From = new DateTime(
					value.Value.Year, value.Value.Month, value.Value.Day,
					From.Hour, From.Minute, From.Second
					);
				}
			}
		}
		public TimeSpan? FromTime {
			get { return From.TimeOfDay; }
			set {
				if (value != null) {
					From = new DateTime(
					From.Year, From.Month, From.Day,
					value.Value.Hours, value.Value.Minutes, value.Value.Seconds);
				}
			}
		}
	}

}
