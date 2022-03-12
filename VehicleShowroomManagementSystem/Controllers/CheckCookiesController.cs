using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class CheckCookiesController : Controller
    {
        public readonly VehicleShowroomManagementSystemContext _context;

        public CheckCookiesController(VehicleShowroomManagementSystemContext context)
        {
            _context = context;
        }

        public Customer Ch_Cookie()
        {
            string account  = HttpContext.Request.Cookies["Customer"];
            var customer = _context.Customers.Include(c=>c.Carts).ThenInclude(c=>c.Product).ThenInclude(p=>p.ProductImages).FirstOrDefault(c => c.Account == account);

            ViewBag.customer = customer;

            ViewBag.manufacturers = _context.Manufacturers.ToList();
            ViewBag.vehicleTypes = _context.VehicleTypes.ToList();

            return customer;
        }
    }
}
