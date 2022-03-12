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
    public class ProductsController : CheckCookiesController
    {

        public ProductsController(VehicleShowroomManagementSystemContext context):base(context)
        {
        }

        private void DataViewList()
        {
            ViewBag.vehicleTypes = _context.VehicleTypes.ToList();
            ViewBag.manufacturers = _context.Manufacturers.ToList();
            ViewBag.branches = _context.Branches.ToList();
            ViewBag.employee = _context.Employees.FirstOrDefault();
            ViewBag.featuredVehicles = _context.Products.Include(p => p.InvoiceDetails).OrderByDescending(p => p.InvoiceDetails.Sum(i => i.Quantity)).FirstOrDefault();
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var vehicleShowroomManagementSystemContext = _context.Products.Include(p => p.Manufacturer).Include(p => p.VehicleType);
            return View(await vehicleShowroomManagementSystemContext.ToListAsync());
        }
        
        public async Task<IActionResult> AllCars()
        {
            var cars = await _context.Products.Include(p=>p.ProductImages).Include(p=>p.Manufacturer).Include(p=>p.VehicleType).Include(p=>p.Warehouses).ThenInclude(w=>w.Branch).Where(p=>p.Warehouses.Sum(w=>w.Stock)>0).ToListAsync();

            DataViewList();
            ViewBag.nameList = "All Cars";

            return View(cars);
        }



        //Get Products/Search/
        public async Task<IActionResult> Search (int ManufacturerId,int VehicleTypeId)
        {
            var products = await _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.Manufacturer.Id == ManufacturerId && p.VehicleType.Id == VehicleTypeId).ToListAsync();

            DataViewList();
            ViewBag.nameList = "Cars List";


            return View(products);
        }

        //Get Products/Filter/
        public async Task<IActionResult> Filter (int BranchId, int VehicleTypeId,int PriceId,int ManufacturerId)
        {



            var products = _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.Manufacturer.Id == ManufacturerId && p.VehicleType.Id == VehicleTypeId && p.Warehouses.Where(w => w.Branch.Id == BranchId).Any(w=>w.Stock>0));


            List<Product> _products =new List<Product>();

            switch (PriceId)
            {
                case 1:
                    _products = await products.Where(p => p.Price >= 10000 && p.Price <= 15000).ToListAsync();
                    break;
                case 2:
                    _products = await products.Where(p => p.Price >= 15000 && p.Price <= 25000).ToListAsync();
                    break;
                case 3:
                    _products = await products.Where(p => p.Price >= 25000 && p.Price <= 35000).ToListAsync();
                    break;
                case 4:
                    _products = await products.Where(p => p.Price >= 35000 && p.Price <= 55000).ToListAsync();
                    break;
                case 5:
                    _products = await products.Where(p => p.Price >= 55000 && p.Price <= 100000).ToListAsync();
                    break;
                default:
                    ViewBag.msgFilter = "Not found";
                    break;
            }

            DataViewList();
            ViewBag.nameList = "Cars List";


            return View(_products);
        }


        //get Products/VehicleTypes

        public async Task<IActionResult> VehicleTypes (int Id)
        {
            if(Id == 0)
            {
                return RedirectToAction("AllCars", "Products"); 
            }

            var products = await  _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.VehicleTypeId == Id).ToListAsync();

            DataViewList();
            ViewBag.nameList = "Cars List";

            return View(products);
        }

        //get Products/Manufacture

        public async Task<IActionResult> Manufacturer(int Id)
        {
            if(Id == 0)
            {
                return RedirectToAction("AllCars", "Products"); 
            }

            var products = await  _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.ManufacturerId == Id).ToListAsync();

            DataViewList();
            ViewBag.nameList = "Cars List";

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            Ch_Cookie();

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.VehicleType).Include(p=>p.ProductImages).Include(p=>p.Warehouses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name");
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VehicleTypeId,ManufacturerId,SerialNumber,Price,Description,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", product.ManufacturerId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", product.VehicleTypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", product.ManufacturerId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", product.VehicleTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VehicleTypeId,ManufacturerId,SerialNumber,Price,Description,Status")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["ManufacturerId"] = new SelectList(_context.Manufacturers, "Id", "Name", product.ManufacturerId);
            ViewData["VehicleTypeId"] = new SelectList(_context.VehicleTypes, "Id", "Name", product.VehicleTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.VehicleType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
