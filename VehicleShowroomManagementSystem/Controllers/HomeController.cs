using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VehicleShowroomManagementSystem.Models;
using VehicleShowroomManagementSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class HomeController : CheckCookiesController
    {


        public HomeController(VehicleShowroomManagementSystemContext context) : base(context)
        {
        }
        private void DataViewList()
        {
            ViewBag.vehicleTypes = _context.VehicleTypes.ToList();
            ViewBag.manufacturers = _context.Manufacturers.ToList();
            ViewBag.branches = _context.Branches.ToList();
            ViewBag.employee = _context.Employees.FirstOrDefault();
            ViewBag.featuredVehicles = _context.Products.Include(p => p.InvoiceDetails).OrderByDescending(p => p.InvoiceDetails.Sum(i => i.Quantity)).FirstOrDefault();
        }

        public IActionResult Index()
        {
            var customer = Ch_Cookie();



            var carts = _context.Products.Include(p => p.ProductImages).ToList();

            ViewBag.employees = _context.Employees.ToList();
            ViewBag.manufacturers = _context.Manufacturers.ToList();
            ViewBag.vehicleTypes = _context.VehicleTypes.ToList();
            ViewBag.styleHeader = "transparent-header";

            return View(carts);
        }

        public IActionResult Contact()
        {
            Ch_Cookie();
            return View();
        }

        public IActionResult AboutUs()
        {
            Ch_Cookie();
            return View();
        }

        public async Task<IActionResult> Search(string str = "")
        {

            Ch_Cookie();

            var products = await _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.Name.Contains(str)).ToListAsync();

            DataViewList();
            ViewBag.nameList = "Cars List";

            return View(products);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
