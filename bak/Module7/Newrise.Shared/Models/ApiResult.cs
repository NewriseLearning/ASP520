namespace Newrise.Shared.Models {
	public class ApiResult<T> {
		public bool Success { get; set; }
		public string Error { get; set; }
		public T Value { get; set; }
		public ApiResult() { }  // used by JSON deserialization
		public ApiResult(string error) { Error = error; }
		public ApiResult(T value) { Value = value; Success = true; }
	}
}
