﻿using System;
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
using Microsoft.AspNetCore.Http;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class CustomersController : CheckCookiesController
    {

        private readonly IWebHostEnvironment _webHostEnvironment;


        public CustomersController(VehicleShowroomManagementSystemContext context, IWebHostEnvironment webHostEnvironment):base(context)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Customers
        public  IActionResult Index()
        {
            var customer = Ch_Cookie();
            if (customer == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(customer);
        }


        // Customers/Login
        [HttpPost]
        public JsonResult Login(string account, string password)
        {
            try
            {
                Customer customer = _context.Customers.FirstOrDefault(c => c.Account == account & c.Password == password);
                if (customer != null)
                {
                    HttpContext.Response.Cookies.Append("Customer", customer.Account, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7)
                    });
                    return Json(new { code = 200, customer = customer, msg = "Logged in successfully" });
                }

                return Json(new { code = 300, msg = "Login failed" });

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "error" + ex.Message });

            }
        }



        // POST: Customers/Register
        [HttpPost]
        public JsonResult Register([Bind("Id,Account,Password,FullName,Address,PhoneNumber,Email,Avatar,Status,AvatarFile")] Customer customer)
        {
            try
            {
                bool check = _context.Customers.Any(c => c.Account == customer.Account || c.PhoneNumber == customer.PhoneNumber || c.Email == customer.Email);
                if (!check)
                {

                    _context.Add(customer);
                    _context.SaveChanges();
                    int id = _context.Customers.FirstOrDefault(c => c.Account == customer.Account).Id;
                    customer.Id = id;


                    string fileName = customer.Id.ToString() + Path.GetExtension(customer.AvatarFile.FileName);
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "avatar", "customer");
                    string filePath = Path.Combine(uploadPath, fileName);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        customer.AvatarFile.CopyTo(fs);
                        customer.Avatar = fileName;
                    }
                    _context.Update(customer);
                    _context.SaveChanges();

                    HttpContext.Response.Cookies.Append("Customer", customer.Account, new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(7),
                    });


                    return Json(new
                    {
                        code = 200,
                        msg = "Register successfully",
                        customer = customer
                    });
                }
                else
                {
                    return Json(new
                    {
                        code = 300,
                        msg = "Register failed"
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "error" + ex.Message });

            }


        }

        public IActionResult Logout()
        {

            HttpContext.Response.Cookies.Delete("Customer");

            return RedirectToAction("index", "Home");
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
