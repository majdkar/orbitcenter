using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Shared.Constants.Clients
{
    public static class ClientTypes
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Person", "Person" },
            {"Company", "Company" },
            //{"Teacher", "Teacher" },
            //{"Employee", "Employee" },
            {"Student", "Student" },
        };
    }
    public enum ClientTypesEnum
    {
        Person,
        Company,
        //Teacher,
        //Employee,
        Student
    }

    public enum CompanyActivityCodeEnum
    { 
        client,
        supplier,
        recovery
    }
}
