using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleShowroomManagementSystem.Areas.Admin.Controllers
{
    public class CheckAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
