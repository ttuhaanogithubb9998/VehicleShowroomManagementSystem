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
    public class Customer
    {
        public int Id { get; set; }

        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} từ 6-20 ký tự")]
        public string Account { get; set; }

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} từ 6 - 20 ký tự")]
        public string Password { get; set; }    

        [DisplayName("Họ và tên")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public string FullName { get; set; }

        [DisplayName("Địa chỉ")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public string Address { get; set; }

        [DisplayName("SDT")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [RegularExpression("0\\d{9}", ErrorMessage = "STD khôn hợp lệ!")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [EmailAddress(ErrorMessage = "{0} không hợp lệ!")]
        public string Email { get; set; }

        [DisplayName("Ảnh đại diện")]
        [DefaultValue("default.png")]
        public string Avatar { get; set; } = "default.png";

        [DisplayName("Ảnh đại diện")]
        [NotMapped]
        public IFormFile AvatarFile { get; set; }

        [DisplayName("Trạng thái")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;


        public List<Cart> Carts { get; set; }

        public List<Invoice> Invoices { get; set; }
    }
}
