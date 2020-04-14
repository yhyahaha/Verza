using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IInnerSalesSlip
    {
        string StaffOrEquipment { get; set; }
        string DivisionCode { get; set; }
        string DivisionName{get;set;}
        string ProjectCode { get; set; }
        string ClientName { get; set; }
    }
}
