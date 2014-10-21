using System.Web.Mvc;

namespace LiveChat.App.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult AccessDenied()
        {
            return View();
        }
    }
}
