namespace SeminarHub.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SeminarHub.Models;
    using System.Diagnostics;

    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            //If the current User is logged in, he is redirected to the "All Seminars" page
            if (User.Identity != null && User != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("All", "Seminar");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}