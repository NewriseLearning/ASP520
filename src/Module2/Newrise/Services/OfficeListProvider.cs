using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Newrise.Shared.Models;
using System.Text.Json;

namespace Newrise.Services {
	public class OfficeListProvider {
		const string FileName = @"Content\offices.json";
		private string _path;

		private readonly IHostEnvironment _environment;
		private readonly ILogger<OfficeListProvider> _logger;
		private readonly IMemoryCache _cache;

		public OfficeListProvider(
			IMemoryCache cache,
			IHostEnvironment environment,
			ILogger<OfficeListProvider> logger) {
			_cache = cache;
			_environment = environment;
			_logger = logger;

			_path = Path.Combine(
				environment.ContentRootPath,
				FileName);
		}

		private IChangeToken _token;
		public IChangeToken GetToken() {
			return _token = _environment.ContentRootFileProvider.Watch(FileName);
		}

		public void Remove() {
			_cache.Remove(FileName);
		}

		public List<Office> GetList() {
			if (_token is null) GetToken();
			else if (_token.HasChanged) { Remove(); GetToken(); }

			var offices = _cache.Get<List<Office>>(FileName);
			if (offices is null) {
				offices = JsonSerializer.Deserialize<List<Office>>(File.ReadAllText(_path));
				_logger.LogInformation($"Office list loaded from '{_path}'.");
				_cache.Set(FileName, offices, TimeSpan.FromMinutes(30));
			}
			return offices;
		}
	}
}
