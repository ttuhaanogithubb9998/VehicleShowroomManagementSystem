using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleShowroomManagementSystem.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [DisplayName("Sản phẩm")]
        public Product Product { get; set; }

        [DisplayName("Đường dẫn")]
        [DefaultValue("default.png")]
        public string Path { get; set; } = "default.png";

        [NotMapped]
        [DisplayName("Ảnh")]
        public IFormFile PathFile { get; set; }
    }
}
