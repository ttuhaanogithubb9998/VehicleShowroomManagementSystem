using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomersController : CheckAdminController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;



        public CustomersController(VehicleShowroomManagementSystemContext context, IWebHostEnvironment webHostEnvironment) : base(context)
        {
            _webHostEnvironment = webHostEnvironment;
            
        }

        // GET: Admin/Customers
        public async Task<IActionResult> Index()
        {
            Employee employee = CheckAdmin();
            if (employee == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View(await _context.Customers.ToListAsync());
        }

        // GET: Admin/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Account,Password,FullName,Address,PhoneNumber,Email,Avatar,AvatarFile,Status")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                bool check = _context.Customers.Any(m => m.Account == customer.Account);
                if (!check)
                {
                    _context.Add(customer);
                    _context.SaveChanges();

                    //upload file
                    int id = _context.Customers.FirstOrDefault(m => m.Account == customer.Account).Id;
                    if (customer.AvatarFile != null)
                    {
                        var fileName = id.ToString() + Path.GetExtension(customer.AvatarFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image","avatar","Customer");
                        var filePath = Path.Combine(uploadPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            customer.AvatarFile.CopyTo(fs);
                            fs.Flush();
                            customer.Avatar = fileName;
                            customer.Id = id;
                        };
                        _context.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Remove(customer);
                        await _context.SaveChangesAsync();
                        ViewBag.msgFile = "File cannot be blank!";
                        return View(customer);
                    }
                }
                else
                {
                    ViewBag.msgCheck = "Account already exists!";
                    return View(customer);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Admin/Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Admin/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Account,Password,FullName,Address,PhoneNumber,Email,Avatar,Status")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Admin/Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
