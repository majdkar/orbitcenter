using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Shared.Orders
{
    public class CourseRequestStatus
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Pending", "Pending" },
            {"Approved", "Approved" },
            {"Rejected", "Rejected" },
            {"Completed", "Completed" },
        };
    }

    public enum CourseRequestStatusEnum
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Completed = 3
    }
}
