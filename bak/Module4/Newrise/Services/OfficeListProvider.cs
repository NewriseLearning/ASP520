using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Newrise.Shared.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Newrise.Services {
	public class OfficeListProvider {

		private readonly IWebHostEnvironment _environment;
		private readonly ILogger<OfficeListProvider> _logger;
		private readonly IMemoryCache _cache;
		private IChangeToken _token;


		public const string DirectoryName = "Content";
		public const string FileName = "offices.json";

		private string _path;

		public OfficeListProvider(
			IMemoryCache cache,
			IWebHostEnvironment environment,
			ILogger<OfficeListProvider> logger) {
			_cache = cache;
			_environment = environment;
			_logger = logger;

			_path = Path.Combine(
				_environment.ContentRootPath,
				DirectoryName, FileName);
		}

		IChangeToken GetToken() {
			_token = _environment.ContentRootFileProvider.Watch(
				Path.Combine(DirectoryName, FileName));
			return _token;
		}

		public List<Office> GetList() {
			var offices = _cache.Get<List<Office>>(FileName);
			if (offices == null) {
				offices = JsonSerializer.Deserialize<List<Office>>(File.ReadAllText(_path));
				_logger.LogInformation($"Office list loaded from '{_path}'.");
				_cache.Set(FileName, offices, GetToken());
			}	return offices;
		}

		public void Remove() {
			_cache.Remove(FileName);
		}

	}
}
