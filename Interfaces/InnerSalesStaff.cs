using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public class InnerSalesStaff : InnerSalesBase
    {
        public List<InnerSalesStaffStatement> Staffs { get; set; } 
            = new List<InnerSalesStaffStatement>();
    }

    public class InnerSalesStaffStatement
    {
        public int ItemCode { get; set; }
        public string Item { get; set; }
        public int StaffCode { get; set; }
        public string StaffName { get; set; }
        public decimal Amount { get; set; }
    }
}
