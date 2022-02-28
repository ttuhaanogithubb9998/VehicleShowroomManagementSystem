using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleShowroomManagementSystem.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [DisplayName("Vị trí")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public string Location { get; set; }

        [DisplayName("Tên chi nhánh")]
        [Required(ErrorMessage ="{0} không được bỏ trống!")]
        public string Name { get; set; }

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage ="{0} không được bỏ trống!")]
        [RegularExpression("0\\d{9}", ErrorMessage = "STD không hợp lệ!")]
        public string PhoneNumber { get; set; }

        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage ="{0} không được bỏ trống!")]
        public string Address { get; set; }


        [DisplayName("Trạng thái hoạt động")]
        public bool Status { get; set; }

        public List<Employee> Employees { get; set; }

        public List<Warehouse> Warehouses { get; set; }
    }
}
