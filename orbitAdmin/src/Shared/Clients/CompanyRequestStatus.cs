﻿
using System.Collections.Generic;


namespace SchoolV01.Shared.Constants.Clients
{
    public static class CompanyRequestStatus
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Pending", "Pending" },
            {"Accepted", "Accepted" },
            {"Refused", "Refused" }
        };
    }

    public enum CompanyRequestStatusEnum
    {
        Pending,
        Accepted,
        Refused
    }
}
