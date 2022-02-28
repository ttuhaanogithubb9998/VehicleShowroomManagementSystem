using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class BarnchesController : Controller
    {
        private readonly VehicleShowroomManagementSystemContext _context;

        public BarnchesController(VehicleShowroomManagementSystemContext context)
        {
            _context = context;
        }

        // GET: Barnches
        public async Task<IActionResult> Index()
        {
            return View(await _context.Barnchs.ToListAsync());
        }

        // GET: Barnches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barnch = await _context.Barnchs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barnch == null)
            {
                return NotFound();
            }

            return View(barnch);
        }

        // GET: Barnches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Barnches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Location,Name,PhoneNumber,Address,Status")] Barnch barnch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(barnch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(barnch);
        }

        // GET: Barnches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barnch = await _context.Barnchs.FindAsync(id);
            if (barnch == null)
            {
                return NotFound();
            }
            return View(barnch);
        }

        // POST: Barnches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location,Name,PhoneNumber,Address,Status")] Barnch barnch)
        {
            if (id != barnch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(barnch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BarnchExists(barnch.Id))
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
            return View(barnch);
        }

        // GET: Barnches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barnch = await _context.Barnchs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barnch == null)
            {
                return NotFound();
            }

            return View(barnch);
        }

        // POST: Barnches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var barnch = await _context.Barnchs.FindAsync(id);
            _context.Barnchs.Remove(barnch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BarnchExists(int id)
        {
            return _context.Barnchs.Any(e => e.Id == id);
        }
    }
}
