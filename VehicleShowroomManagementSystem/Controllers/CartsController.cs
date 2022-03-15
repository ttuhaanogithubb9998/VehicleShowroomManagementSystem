using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagementSystem.Data;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Controllers
{
    public class CartsController : CheckCookiesController
    {

        public CartsController(VehicleShowroomManagementSystemContext context) : base(context)
        {
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var vehicleShowroomManagementSystemContext = _context.Carts.Include(c => c.Customer).Include(c => c.Product);
            return View(await vehicleShowroomManagementSystemContext.ToListAsync());
        }


        //add cart

        public JsonResult Add(int Id, string Customer)
        {
            try
            {

                var carts = _context.Carts.FirstOrDefault(c => c.Customer.Account == Customer && c.Product.Id == Id);
                if (carts != null)
                {
                    carts.Quantity++;
                    _context.Update(carts);

                }
                else
                {
                    Cart c = new Cart();
                    c.CustomerId = _context.Customers.FirstOrDefault(c => c.Account == Customer).Id;
                    c.Quantity = 1;
                    c.ProductId = Id;
                    _context.Add(c);

                }


                _context.SaveChanges();

                int quantityCart = _context.Customers.Include(c => c.Carts).FirstOrDefault(c => c.Account == Customer).Carts.Count();

                return Json(new { quantityCart, code = 200, msg = "successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "error " + ex.Message });
            }
        }

        public IActionResult Remove(int Id)
        {

            var cart = _context.Carts.FirstOrDefault(c => c.Id == Id);


            _context.Remove(cart);
            _context.SaveChanges();


            return RedirectToAction("index", "Customers");
        }

        // pay carts

        public IActionResult Order(string values)
        {
            Ch_Cookie();

            string[] lId = values.Split(",");


            List<Cart> carts = new List<Cart>();

            foreach (var item in lId)
            {
                var cart = _context.Carts.Include(c => c.Product).ThenInclude(p => p.ProductImages).FirstOrDefault(c => c.Id == Convert.ToInt32(item));
                carts.Add(cart);
            }
                
            return PartialView("_ListOrder", carts);


        }

        public async Task<IActionResult> Pay(string ShippingAddress, string ShippingPhone, string listId)
        {
            Customer customer = Ch_Cookie();
            int totalPrice = 0;

            listId = listId.Substring(0, listId.Length - 1);

            string[] listCaartId = listId.Split(",");



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


            foreach (var cartId in listCaartId)
            {
                Cart cart = _context.Carts.Include(c => c.Product).ThenInclude(p => p.Warehouses).FirstOrDefault(c => c.Id == Convert.ToInt32(cartId));

                if (cart.Quantity > cart.Product.Warehouses.Sum(w => w.Stock))
                {
                    // delete generated invoices when some products are not enough

                    var listInvoiceDetails = _context.InvoiceDetails.Where(i => i.InvoiceId == invoiceId).ToList();
                    if (listInvoiceDetails != null)
                    {
                        // delete the invoice details related to the above invoice
                        foreach (var item in listInvoiceDetails)
                        {
                            _context.Remove(item);
                            _context.SaveChanges();
                        }
                    }


                    _context.Remove(invoice);
                   await _context.SaveChangesAsync();

                    ViewBag.msg = "Some products in stock are not enough or out of stock!";
                    return View();
                }
                else
                {

                    // minus the quantity in stock
                    int minus = cart.Quantity;

                    foreach (var item in cart.Product.Warehouses)
                    {
                        if (item.Stock >= minus)
                        {
                            item.Stock = item.Stock - minus;
                            _context.Update(item);
                            await _context.SaveChangesAsync();
                            break;
                        }
                        else
                        {
                            item.Stock = 0;
                            minus = minus - item.Stock;
                            _context.Update(item);
                            await _context.SaveChangesAsync();
                        }
                    }

                    //create invoice details for each product

                    InvoiceDetail invoiceDetail = new InvoiceDetail();
                    invoiceDetail.ProductId = cart.Product.Id;
                    invoiceDetail.InvoiceId = invoiceId;
                    invoiceDetail.Quantity = cart.Quantity;
                    invoiceDetail.UnitPrice = invoiceDetail.Quantity * cart.Product.Price;
                    totalPrice += invoiceDetail.UnitPrice;

                    _context.Add(invoiceDetail);
                    await _context.SaveChangesAsync();
                }

            }

            // delete selected carts
            foreach (var cId in listCaartId)
            {
                Cart cart = _context.Carts.FirstOrDefault(c => c.Id == Convert.ToInt32(cId));
                _context.Remove(cart);
                await _context.SaveChangesAsync();
            }


            invoice.TotalPrice = totalPrice;
            _context.Update(invoice);
            _context.SaveChanges();

            return RedirectToAction("Invoices", "Customers");

        }


        public JsonResult PlusQuantity(int cartId)
        {
            try
            {
                var cart = _context.Carts.FirstOrDefault(c => c.Id == cartId);
                if (cart != null)
                {
                    int productId = _context.Carts.Include(c => c.Product).FirstOrDefault(c => c.Id == cartId).Product.Id;
                    int inventoryNumber = _context.Warehouses.Where(w => w.ProductId == productId).Sum(w => w.Stock);
                    if (cart.Quantity < inventoryNumber)
                    {
                        cart.Quantity++;
                        _context.Update(cart);
                        _context.SaveChanges();

                        return Json(new { cart, code = 200, inventoryNumber, msg = "successfully" });
                    }
                    else
                    {
                        return Json(new { code = 300, msg = "max" });
                    }
                }


                return Json(new { code = 200, msg = "cart is null" });

            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "error: " + ex.Message });
            }


        }



        public JsonResult MinusQuantity(int cartId)
        {
            try
            {
                var cart = _context.Carts.FirstOrDefault(c => c.Id == cartId);
                if (cart != null)
                {
                    if (cart.Quantity > 1)
                    {
                        int productId = _context.Carts.Include(c => c.Product).FirstOrDefault(c => c.Id == cartId).Product.Id;
                        int inventoryNumber = _context.Warehouses.Where(w => w.ProductId == productId).Sum(w => w.Stock);

                        cart.Quantity--;
                        _context.Update(cart);
                        _context.SaveChanges();

                        return Json(new { cart, code = 200, inventoryNumber, msg = "successfully" });
                    }
                    else
                    {
                        _context.Remove(cart);
                        _context.SaveChanges();


                        return Json(new { code = 300, msg = "remove" });
                    }
                }


                return Json(new { code = 200, msg = "cart is null" });

            }
            catch (Exception ex)
            {

                return Json(new { code = 500, msg = "error: " + ex.Message });
            }


        }



        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Customer)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }



        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
    }
}
