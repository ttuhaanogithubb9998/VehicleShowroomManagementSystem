using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace VehicleShowroomManagementSystem.Models
{
    public class Manufacturer
    {
        public int Id { get; set; }

        [DisplayName("Tên hãng sản xuất")]
        [Required(ErrorMessage ="{0} không được bỏ trống!")]
        public string Name { get; set; }

        [DisplayName("Logo")]
        [DefaultValue("default.png")]
        public string Image { set; get; } = "default.png";

        [NotMapped]
        [DisplayName("Logo")]
        public IFormFile ImageFile { get; set; }

        [DisplayName("Trạng thái")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;
    }
}
