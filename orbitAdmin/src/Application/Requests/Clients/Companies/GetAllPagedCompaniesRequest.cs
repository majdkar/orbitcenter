using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolV01.Application.Requests.Clients.Companies
{
    public class GetAllPagedCompaniesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
