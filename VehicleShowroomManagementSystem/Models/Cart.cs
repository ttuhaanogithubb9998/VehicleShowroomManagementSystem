﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;


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
    }
}
