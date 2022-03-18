using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleShowroomManagementSystem.Data;

namespace VehicleShowroomManagementSystem.Areas.Admin.Controllers
{
    public class SearchController : Controller
    {
        public readonly VehicleShowroomManagementSystemContext _context;
        public SearchController(VehicleShowroomManagementSystemContext context)
        {
           _context = context;
        }

    }
}
