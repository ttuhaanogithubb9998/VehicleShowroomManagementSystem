using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Data
{
    public class VehicleShowroomManagementSystemContext: DbContext
    {
        public VehicleShowroomManagementSystemContext(DbContextOptions<VehicleShowroomManagementSystemContext> options) : base(options) { }

        public DbSet<Branch> Branches { set; get; }

        public DbSet<Cart> Carts { set; get; }

        public DbSet<Customer> Customers { set; get; }

        public DbSet<Employee> Employees { set; get; }

        public DbSet<Invoice> Invoices { set; get; }

        public DbSet<InvoiceDetail> InvoiceDetails { set; get; }

        public DbSet<Manufacturer> Manufacturers { set; get; }

        public DbSet<Product> Products { set; get; }

        public DbSet<ProductImage> ProductImages { set; get; }

        public DbSet<VehicleType> VehicleTypes { set; get; }

        public DbSet<Warehouse> Warehouses { set; get; }

    }
}
