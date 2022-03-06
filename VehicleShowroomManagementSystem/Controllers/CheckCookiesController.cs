using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var customer = _context.Customers.FirstOrDefault(c => c.Account == account);

            ViewBag.customer = customer;

            return customer;
        }
    }
}
