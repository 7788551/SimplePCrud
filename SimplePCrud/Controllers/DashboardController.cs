using Microsoft.AspNetCore.Mvc;

namespace SimplePCrud.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult AdminDashboard()
        {
            string? roleId = HttpContext.Session.GetString("RoleId");

            if(roleId == null || roleId != "1")
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }
        public IActionResult UserDashboard()
        {
            string? roleId = HttpContext.Session.GetString("RoleId");

            if (roleId == null || roleId != "2")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }


    }
}
