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
    public class ManufacturersController : Controller
    {
        private readonly VehicleShowroomManagementSystemContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManufacturersController(VehicleShowroomManagementSystemContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: Manufacturers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Manufacturers.ToListAsync());
        }

        // GET: Manufacturers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }

        // GET: Manufacturers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manufacturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,ImageFile,Status")] Manufacturer manufacturer)
        {

            if (ModelState.IsValid)
            {
                //check manufaturer
                bool check = _context.Manufacturers.Any(m => m.Name == manufacturer.Name);
                if (!check)
                {
                    _context.Add(manufacturer);
                    _context.SaveChanges();

                    // upload image file
                    int id = _context.Manufacturers.FirstOrDefault(m => m.Name == manufacturer.Name).Id;
                    if (manufacturer.ImageFile != null)
                    {
                        var fileName = id.ToString() + Path.GetExtension(manufacturer.ImageFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "logo", "manufacturer");
                        var filePath = Path.Combine(uploadPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            manufacturer.ImageFile.CopyTo(fs);
                            fs.Flush();
                            manufacturer.Image = fileName;
                            manufacturer.Id = id;

                        }
                        _context.Update(manufacturer);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.Remove(manufacturer);
                        await _context.SaveChangesAsync();
                        ViewBag.msgFile = "Hình ảnh không được bỏ trống!";
                        return View(manufacturer);
                    }

                }
                else
                {
                    ViewBag.msgCheck = "Tên hãng đã tồn tại!";
                    return View(manufacturer);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(manufacturer);
        }

        // GET: Manufacturers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers.FindAsync(id);
            if (manufacturer == null)
            {
                return NotFound();
            }
            return View(manufacturer);
        }

        // POST: Manufacturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Status")] Manufacturer manufacturer)
        {
            if (id != manufacturer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manufacturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManufacturerExists(manufacturer.Id))
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
            return View(manufacturer);
        }

        // GET: Manufacturers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var manufacturer = await _context.Manufacturers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturer);
        }

        // POST: Manufacturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var manufacturer = await _context.Manufacturers.FindAsync(id);
            _context.Manufacturers.Remove(manufacturer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ManufacturerExists(int id)
        {
            return _context.Manufacturers.Any(e => e.Id == id);
        }
    }
}
