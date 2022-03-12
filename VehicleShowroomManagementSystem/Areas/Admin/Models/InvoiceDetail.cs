using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using VehicleShowroomManagementSystem.Models;

namespace VehicleShowroomManagementSystem.Areas.Admin.Models
{
    public class InvoiceDetail
    {
        public int Id { get; set; }

        [DisplayName("Sản phẩm")]
        public int? ProductId { get; set; }

        [DisplayName("Sản phẩm")]
        public Product Product { get; set; }

        public int? InvoiceId { get; set; }

        [DisplayName("Hóa đơn")]
        public Invoice Invoice { get; set; }

        [DisplayName("Số lượng")]
        public int Quantity { get; set; }

        [DisplayName("Đơn giá")]
        public int UnitPrice { get; set; }
    }
}
