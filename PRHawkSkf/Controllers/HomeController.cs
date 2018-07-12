
using System.Web.Mvc;


namespace PRHawkSkf.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "PRHawk SKF is my take on the ServiceNow 'take home coding test' Web application.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Contact: Scott K. Fraley via CyberCoders.";

			return View();
		}
	}
}
