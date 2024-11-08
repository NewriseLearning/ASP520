using Microsoft.AspNetCore.Mvc;
using Newrise.Services;
using Newrise.Shared.Models;

namespace Newrise.Controllers {
	[Route("api/offices")]
	[ApiController]
	public class OfficeController : ControllerBase {
		readonly OfficeListProvider _officeListProvider;
		public OfficeController(OfficeListProvider officeListProvider) {
			_officeListProvider = officeListProvider;
		}

		[HttpGet]
		public ApiResult<List<Office>> GetList() {
			try {
				var offices = _officeListProvider.GetList();
				return new ApiResult<List<Office>>(offices);
			}
			catch (Exception ex) {
				return new ApiResult<List<Office>>(
					$"Unable to return office list. {ex.Message}");
			}
		}
	}
}
