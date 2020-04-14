using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public class InnerSalesBase
    {
        public string StaffOrEquipment { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string ProjectCode { get; set; }
        public string ClientName { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
