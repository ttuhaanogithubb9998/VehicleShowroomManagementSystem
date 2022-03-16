using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleShowroomManagementSystem.Data;

namespace VehicleShowroomManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly VehicleShowroomManagementSystemContext _context;

        public HomeController(VehicleShowroomManagementSystemContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("Account") != null)
            {
                ViewBag.FullName = HttpContext.Session.GetString("Account");
            }

            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        
        public IActionResult Login (string account,string password)
        {
            int count = _context.Employees.Count(a => a.Account == account && a.Password == password);
            if (count > 0)
            {
                HttpContext.Session.SetString("Account", account);

                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.Msg = "Login Failed! Try Again!";
                return View();
            }

        }
    }
}
