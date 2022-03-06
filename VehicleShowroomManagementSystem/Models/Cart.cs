using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace VehicleShowroomManagementSystem.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [DisplayName("Sản phẩm")]
        public int? ProductId { get; set; }

        [DisplayName("Sản phẩm")]
        public Product Product { get; set; }

        [DisplayName("Khách hàng")]
        public int? CustomerId { get; set; }

        [DisplayName("Khách hàng")]
        public Customer Customer { get; set; }

        [DisplayName("Số lượng")]
        [Required(ErrorMessage = "{0} không được bỏ trống")]
        [DefaultValue(1)]
        public int Quantity { get; set; } = 1;
    }
}
