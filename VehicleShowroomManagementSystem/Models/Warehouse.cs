using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleShowroomManagementSystem.Models
{
    public class Warehouse
    {
        public int Id { get; set; }

        [DisplayName("Chi nhánh")]
        public int BranchId { get; set; }

        [DisplayName("Chi nhánh")]
        public Branch Branch { get; set; }

        public int ProductId { get; set; }

        [DisplayName("Sản phẩm")]
        public Product Product { get; set; }

        [DisplayName("Số lượng tồn kho")]
        public int Stock { get; set; }

    }
}
