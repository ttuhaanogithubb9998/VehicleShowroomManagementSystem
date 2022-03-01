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
    public class VehicleTypesController : Controller
    {
        private readonly VehicleShowroomManagementSystemContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public VehicleTypesController(VehicleShowroomManagementSystemContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: VehicleTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.VehicleTypes.ToListAsync());
        }

        // GET: VehicleTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // GET: VehicleTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Image,Status,ImageFile")] VehicleType vehicleType)
        {
            if (ModelState.IsValid)
            {
                bool check = _context.VehicleTypes.Any(vt => vt.Name == vehicleType.Name);
                if (!check)
                {
                    _context.Add(vehicleType);
                    await _context.SaveChangesAsync();

                    int id = _context.VehicleTypes.FirstOrDefault(v => v.Name == vehicleType.Name).Id;

                    if (vehicleType.ImageFile != null)
                    {
                        var fileName = id.ToString() + Path.GetExtension(vehicleType.ImageFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.ContentRootPath, "image", "logo", "vehicleType");
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            vehicleType.ImageFile.CopyTo(fs);
                            fs.Flush();
                            vehicleType.Image = fileName;
                            vehicleType.Id = id;
                        }
                    }
                    else
                    {
                        _context.Remove(vehicleType);
                        await _context.SaveChangesAsync();
                        ViewBag.msgFile = "Hãy chọn hình ảnh";

                        return View(vehicleType);
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.msgCheck = "Tên đã tồn tại!";
                    return View(vehicleType);
                }
            }
            return View(vehicleType);
        }

        // GET: VehicleTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType == null)
            {
                return NotFound();
            }
            return View(vehicleType);
        }

        // POST: VehicleTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Image,Status")] VehicleType vehicleType)
        {
            if (id != vehicleType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicleType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleTypeExists(vehicleType.Id))
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
            return View(vehicleType);
        }

        // GET: VehicleTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicleType = await _context.VehicleTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleType == null)
            {
                return NotFound();
            }

            return View(vehicleType);
        }

        // POST: VehicleTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleTypeExists(int id)
        {
            return _context.VehicleTypes.Any(e => e.Id == id);
        }
    }
}
