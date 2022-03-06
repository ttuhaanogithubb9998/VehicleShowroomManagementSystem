using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VehicleShowroomManagementSystem.Models;
using VehicleShowroomManagementSystem.Data;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class HomeController : CheckCookiesController
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger,VehicleShowroomManagementSystemContext context):base( context )
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Ch_Cookie();


            return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
