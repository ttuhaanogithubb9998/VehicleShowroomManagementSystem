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
            if(!CheckAdmin())
            {
                return RedirectToAction("Login");
            }
            if (HttpContext.Session.GetString("Account") != null)
            {
                ViewBag.FullName = HttpContext.Session.GetString("Account");
            }
            return View();
            
        }
        public IActionResult Login()
        {
            if(!CheckAdmin())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
                
            }   
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
        public bool CheckAdmin()
        {
            string admin = HttpContext.Session.GetString("Account");

            bool check = _context.Employees.Any(e => e.Account == admin && e.Position == "Admin");

            return check;
        }
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("Account");

            return RedirectToAction("Login");
        }
    }
}
