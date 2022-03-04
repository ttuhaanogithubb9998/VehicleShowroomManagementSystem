using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class CustomersController : Controller
    {
        private readonly VehicleShowroomManagementSystemContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public CustomersController(VehicleShowroomManagementSystemContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }


        // GET: Customers/Login
        [HttpPost]
        public JsonResult Login(string account, string password)
        {
            try
            {
                Customer Customer = _context.Customers.FirstOrDefault(c => c.Account == account & c.Password == password);
                if (Customer != null)
                {

                    return Json(new { code = 200, customer = Customer, msg = "Logged in successfully" });
                }

                return Json(new { code = 200, msg = "Login failed" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "error" + ex.Message });

            }
        }


        // GET: Customers/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Customers/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Account,Password,FullName,Address,PhoneNumber,Email,Avatar,Status,AvatarFile")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                bool check = _context.Customers.Any(c => c.Account == customer.Account || c.PhoneNumber == customer.PhoneNumber || c.Email == customer.Email);
                if (!check)
                {

                    _context.Add(customer);
                    _context.SaveChanges();
                    int id = _context.Customers.FirstOrDefault(c => c.Account == customer.Account).Id;
                    customer.Id = id;

                    if (customer.AvatarFile != null)
                    {

                        string fileName = id.ToString() + Path.GetExtension(customer.AvatarFile.FileName);
                        string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "avatar", "customer");
                        string filePath = Path.Combine(uploadPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            customer.AvatarFile.CopyTo(fs);
                            customer.Avatar = fileName;
                        }
                        _context.Update(customer);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        ViewBag.msgFile = "Hình ảnh không được bỏ trống!";
                        _context.Remove(customer);
                        await _context.SaveChangesAsync();
                        return View(customer);
                    }
                }
                else
                {
                    _context.Remove(customer);
                    await _context.SaveChangesAsync();
                    ViewBag.msgCheck = "Tên tài khoản, SDT, Email đã tồn tại!";
                    return View(customer);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
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

        // POST: Customers/Edit/5
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


        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
