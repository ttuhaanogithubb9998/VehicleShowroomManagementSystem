using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace VehicleShowroomManagementSystem.Models
{
    public class VehicleType
    {
        public int Id { get; set; }

        [DisplayName("Loại phương tiện")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public string Name { get; set; }

        [DisplayName("Logo")]
        public string Image { set; get; }

        [NotMapped]
        [DisplayName("Logo")]
        public IFormFile ImageFile { get; set; }

        [DisplayName("Trạng thái")]
        public bool Status { get; set; }

        public List<Product> Products { get; set; }

        public List<Manufacturer> Manufacturers { get; set; }
    }
}
