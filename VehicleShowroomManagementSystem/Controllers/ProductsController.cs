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

        public ProductsController(VehicleShowroomManagementSystemContext context) : base(context)
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
            Ch_Cookie();
            var cars = await _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.Warehouses.Sum(w => w.Stock) > 0).ToListAsync();

            DataViewList();
            ViewBag.nameList = "All Cars";

            return View(cars);
        }



        //Get Products/Search/
        public async Task<IActionResult> Search(int ManufacturerId, int VehicleTypeId)
        {
            Ch_Cookie();

            var products = await _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.Manufacturer.Id == ManufacturerId && p.VehicleType.Id == VehicleTypeId).ToListAsync();

            DataViewList();
            ViewBag.nameList = "Cars List";


            return View(products);
        }

        //Get Products/Filter/
        public async Task<IActionResult> Filter(int BranchId, int VehicleTypeId, int PriceId, int ManufacturerId)
        {
            Ch_Cookie();



            var products = _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.Manufacturer.Id == ManufacturerId && p.VehicleType.Id == VehicleTypeId && p.Warehouses.Where(w => w.Branch.Id == BranchId).Any(w => w.Stock > 0));


            List<Product> _products = new List<Product>();

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

        public async Task<IActionResult> VehicleType(int Id)
        {
            Ch_Cookie();

            if (Id == 0)
            {
                return RedirectToAction("AllCars", "Products");
            }

            var products = await _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.VehicleTypeId == Id).ToListAsync();

            DataViewList();
            ViewBag.nameList = "Cars List";

            return View(products);
        }

        //get Products/Manufacture

        public async Task<IActionResult> Manufacturer(int Id)
        {
            Ch_Cookie();

            if (Id == 0)
            {
                return RedirectToAction("AllCars", "Products");
            }

            var products = await _context.Products.Include(p => p.ProductImages).Include(p => p.Manufacturer).Include(p => p.VehicleType).Include(p => p.Warehouses).ThenInclude(w => w.Branch).Where(p => p.ManufacturerId == Id).ToListAsync();

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
                .Include(p => p.VehicleType).Include(p => p.ProductImages).Include(p => p.Warehouses)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        public async Task<IActionResult> Order(int? Id)
        {
            Ch_Cookie();

            if (Id == null)
            {
                return RedirectToAction("index", "Home");
            }

            var product = await _context.Products.Include(p => p.ProductImages).Where(p => p.Id == Id).FirstOrDefaultAsync();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Pay(int productId, string ShippingAddress, string ShippingPhone)
        {
            Customer customer = Ch_Cookie();

            var product = _context.Products.Include(p => p.Warehouses).FirstOrDefault(p => p.Id == productId);

            // check inventory
            if (product.Warehouses.Sum(w => w.Stock) > 0)
            {
                var wh = product.Warehouses.OrderByDescending(w => w.Stock).FirstOrDefault() ;
                wh.Stock--;
                _context.Update(wh);
                await _context.SaveChangesAsync();

                // create invoice
                Invoice invoice = new Invoice();
                DateTime timmeNow = DateTime.Now;

                invoice.ShippingAddress = ShippingAddress;
                invoice.ShippingPhone = ShippingPhone;
                invoice.ContractNumber = ((customer.Account + timmeNow).GetHashCode()*-1).ToString();
                invoice.CustomerId = customer.Id;
                invoice.Date = timmeNow;

                _context.Add(invoice);
                _context.SaveChanges();

                int invoiceId = _context.Invoices.FirstOrDefault(i => i.ContractNumber == invoice.ContractNumber).Id;
                invoice.Id = invoiceId;

                //create invoice details for each product

                InvoiceDetail invoiceDetail = new InvoiceDetail();
                invoiceDetail.ProductId = product.Id;
                invoiceDetail.InvoiceId = invoiceId;
                invoiceDetail.Quantity = 1;
                invoiceDetail.UnitPrice = product.Price;

                invoice.TotalPrice = invoiceDetail.UnitPrice;

                _context.Add(invoiceDetail);
                _context.Update(invoice);

                await _context.SaveChangesAsync();

            }
            else
            {
                ViewBag.msg = "Some products in stock are not enough or out of stock!";
                return View();
            }




            return RedirectToAction("Invoices", "Customers");
        }



        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
