using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public class InnerSalesEquipment : InnerSalesBase
    {
        public List<InnerSalesEquipmentStatement> Equipments { get; set; } 
            = new List<InnerSalesEquipmentStatement>();
    }
    
    public class InnerSalesEquipmentStatement
    {
        public int ItemCode { get; set; }
        public string Item { get; set; }
        public decimal Amount { get; set; }
    }
}
