
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace VehicleShowroomManagementSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        [DisplayName("Tên")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public string Name { get; set; }


        [DisplayName("Loại phương tiện")]
        public int VehicleTypeId { get; set; }

        [DisplayName("Loại phương tiện")]
        public VehicleType VehicleType { get; set; }

        [DisplayName("Hãng sản xuất")]
        public int ManufacturerId { get; set; }

        [DisplayName("Hãng sản xuất")]
        public Manufacturer Manufacturer { get; set; }

        [DisplayName("Mã định danh")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public string SerialNumber { get; set; }

        [DisplayName("Giá bán")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public int Price { get; set; }

        [DisplayName("Mô tả chi tiết")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public string Description { get; set; }

        [DisplayName("Trạng thái")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;


        public List<Warehouse> Warehouses { get; set; }

        public List<Cart> Carts { get; set; }

        public List<InvoiceDetail> InvoiceDetails { get; set; }

        public List<ProductImage> ProductImages { get; set; }
    }
}
