using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : CheckAdminController
    {

        public HomeController(VehicleShowroomManagementSystemContext context):base(context)
        {
        }
        public IActionResult Index()
        {
            if(CheckAdmin()==null)
            {
                return RedirectToAction("Login");
            }
            return View();
            
        }
        public IActionResult Login()
        {
            if(CheckAdmin()==null)
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
       
    }
}
