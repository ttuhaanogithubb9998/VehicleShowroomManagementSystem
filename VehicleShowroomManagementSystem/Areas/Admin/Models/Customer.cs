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
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} Please enter your account!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} từ 6-20 ký tự")]
        public string Account { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0} từ 6 - 20 ký tự")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [RegularExpression("0\\d{9}", ErrorMessage = "STD khôn hợp lệ!")]
        public string PhoneNumber { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} không được bỏ trống!")]
        [EmailAddress(ErrorMessage = "{0} không hợp lệ!")]
        public string Email { get; set; }

        [DefaultValue("default.png")]
        public string Avatar { get; set; } = "default.png";

        [NotMapped]
        public IFormFile AvatarFile { get; set; }

        [DefaultValue(true)]
        public bool Status { get; set; } = true;


        public List<Cart> Carts { get; set; }

        public List<Invoice> Invoices { get; set; }
    }
}
