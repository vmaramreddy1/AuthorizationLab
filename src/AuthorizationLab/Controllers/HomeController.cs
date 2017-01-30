using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizationLab.Controllers
{
	[Authorize(Policy = "BuildingEntry")]
	public class HomeController : Controller 
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
