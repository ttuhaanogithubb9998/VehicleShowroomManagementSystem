using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WarehousesController : CheckAdminController
    {
       

        public WarehousesController(VehicleShowroomManagementSystemContext context) : base(context)
        {
            
        }

        // GET: Admin/Warehouses
        public async Task<IActionResult> Index()
        {
            Employee employee = CheckAdmin();
            if (employee == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var vehicleShowroomManagementSystemContext = _context.Warehouses.Include(w => w.Branch).Include(w => w.Product);
            return View(await vehicleShowroomManagementSystemContext.ToListAsync());
        }

        // GET: Admin/Warehouses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .Include(w => w.Branch)
                .Include(w => w.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // GET: Admin/Warehouses/Create
        public IActionResult Create()
        {
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description");
            return View();
        }

        // POST: Admin/Warehouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BranchId,ProductId,Stock")] Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address", warehouse.BranchId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", warehouse.ProductId);
            return View(warehouse);
        }

        // GET: Admin/Warehouses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address", warehouse.BranchId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", warehouse.ProductId);
            return View(warehouse);
        }

        // POST: Admin/Warehouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BranchId,ProductId,Stock")] Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(warehouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WarehouseExists(warehouse.Id))
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
            ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Address", warehouse.BranchId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Description", warehouse.ProductId);
            return View(warehouse);
        }

        // GET: Admin/Warehouses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .Include(w => w.Branch)
                .Include(w => w.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                return NotFound();
            }

            return View(warehouse);
        }

        // POST: Admin/Warehouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.Id == id);
        }
    }
}
