using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VehicleShowroomManagementSystem.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        
        [DisplayName("Ngày giao dịch")]
        public DateTime Date { get; set; }

        [DisplayName("Mã hóa đơn")]
        [Required(ErrorMessage ="{0} không được bỏ trống!")]
        public string ContractNumber { get; set; }

        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [DisplayName("Địa chỉ giao hàng")]
        public string ShippingAddress { get; set; }

        [DisplayName("SDT nhận hàng")]
        public string ShippingPhone { get; set; }

        [DisplayName("Tổng tiền")]
        public int TotalPrice { get; set; }

        [DisplayName("Trạng thái")]
        [DefaultValue(true)]
        public bool Status { get; set; } = true;

        public List<InvoiceDetail>  InvoiceDetails { get; set; }
    }
}
