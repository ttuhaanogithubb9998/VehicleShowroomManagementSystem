using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeesController : CheckAdminController
    {
        
        

        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeesController(VehicleShowroomManagementSystemContext context, IWebHostEnvironment webHostEnvironment):base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Employees
        public async Task<IActionResult> Index()
        {
           Employee employee =  CheckAdmin();
            if(employee == null)
            {
                return RedirectToAction("Login", "Home");
            }
            
            var vehicleShowroomManagementSystemContext = _context.Employees.Include(e => e.Branch);

            return View(await vehicleShowroomManagementSystemContext.ToListAsync());

        }

        // GET: Admin/Employees/Details/5
        public IActionResult Details()
        {
            if (CheckAdmin() == null)
            {
                return NotFound();
            }
            return View(CheckAdmin());
        }

        // GET: Admin/Employees/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address");
            return View();
        }

        // POST: Admin/Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Position,BranchId,Account,Password,FullName,Avatar,AvatarFile,Email,Address,PhoneNumber,Status")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                //check Employees
                bool check = _context.Employees.Any(e => e.Id == employee.Id || e.Account == employee.Account);
                if (!check)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();

                    //upload file
                    int id = _context.Employees.FirstOrDefault(m => m.Account == employee.Account).Id;
                    if (employee.AvatarFile != null)
                    {
                        var fileName = id.ToString() + Path.GetExtension(employee.AvatarFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "avatar", "employee");
                        var filePath = Path.Combine(uploadPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            employee.AvatarFile.CopyTo(fs);
                            fs.Flush();
                            employee.Avatar = fileName;
                            employee.Id = id;
                        };
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Remove(employee);
                        await _context.SaveChangesAsync();
                        ViewBag.msgFile = "Image cannot be blank!";
                        ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address");
                        return View(employee);
                    }
                }
                else
                {
                    ViewBag.msgCheck = "Employee already exists!";
                    ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address");
                    return View(employee);
                }
                ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address");
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Admin/Employees/Edit/5
        public IActionResult Edit()
        {
            Employee employee = CheckAdmin();
            if (employee == null)
            {
                return RedirectToAction("Login", "Home");
            }

           
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address", employee.BranchId);
            return View(employee);
        }

        // POST: Admin/Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Position,BranchId,Account,Password,FullName,Avatar,AvatarFile,Email,Address,PhoneNumber,Status")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
  
                    if (employee.AvatarFile != null)
                    {
                        var fileName = id.ToString() + Path.GetExtension(employee.AvatarFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "avatar", "employee");
                        var filePath = Path.Combine(uploadPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            employee.AvatarFile.CopyTo(fs);
                            fs.Flush();
                            employee.Avatar = fileName;
                            employee.Id = id;
                        };
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {  
                        ViewBag.msgFile = "Image cannot be blank!";
                        ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address");
                        return View(employee);
                    }

                    _context.Update(employee);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));

            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address", employee.BranchId);
            return View(employee);
        }

        // GET: Admin/Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Admin/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // POST: Admin/Emloyees/Login
        


        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
