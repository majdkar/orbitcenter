using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Shared.Constants.Clients
{
    public static class PaymentStatus
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Paid", "Paid" },
            {"UnPaid", "UnPaid" }
        };
    }
    public enum PaymentStatusEnum
    {
        Paid,
        UnPaid
    }
}
