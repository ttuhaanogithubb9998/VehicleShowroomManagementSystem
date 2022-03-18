using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;

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
            if(CheckAdmin()==null)
            {
                return RedirectToAction("Login");
            }
            return View();
            
        }
        private void DataViewList()
        {
            ViewBag.vehicleTypes = _context.VehicleTypes.ToList();
            ViewBag.manufacturers = _context.Manufacturers.ToList();
            ViewBag.branches = _context.Branches.ToList();
            ViewBag.employee = _context.Employees.FirstOrDefault();


            ViewBag.featuredVehicles = _context.Products.Include(p => p.ProductImages).Include(p => p.InvoiceDetails).OrderByDescending(p => p.InvoiceDetails.Sum(i => i.Quantity)).FirstOrDefault();
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
        public Employee CheckAdmin()
        {
            string admin = HttpContext.Session.GetString("Account");

            var employee = _context.Employees.FirstOrDefault(e => e.Account == admin && e.Position == "Admin");
            ViewBag.employee = employee;
            return employee;
        }
        public IActionResult Logout()
        {

            HttpContext.Session.Remove("Account");

            return RedirectToAction("Login");
        }
        public async Task<IActionResult> Search(string find)
        {
            CheckAdmin();
            var str = await _context.Products.Include(s => s.ProductImages).Include(s => s.VehicleType).Include(s => s.Manufacturer).Include(s => s.Carts).Include(s => s.InvoiceDetails).ThenInclude(s => s.Invoice).Where(s => s.Name.Contains(find) || s.Description.Contains(find)).ToListAsync();

            DataViewList();
            ViewBag.nameList = "Cars List";

            
            return View(str);
        } 
    }
}
