using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;
using Microsoft.AspNetCore.Http;


namespace VehicleShowroomManagementSystem.Areas.Admin.Controllers

{
    [Area("Admin")]
    public class CheckAdminController : Controller
    {


        public readonly VehicleShowroomManagementSystemContext _context;

        public CheckAdminController(VehicleShowroomManagementSystemContext context)
        {
            _context = context;
        }



        public Employee CheckAdmin()
        {
            string admin = HttpContext.Session.GetString("Account");

            var employee = _context.Employees.FirstOrDefault(e => e.Account == admin && e.Position == "Admin");
            ViewBag.employee = employee;

            return employee;
        }



       
    }
}
