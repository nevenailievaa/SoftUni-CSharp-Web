namespace Library.Controllers
{
    using Library.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            //If the current user is logged in
            if (User != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("All", "Book");
            }

            //If the current user is NOT logged in
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}