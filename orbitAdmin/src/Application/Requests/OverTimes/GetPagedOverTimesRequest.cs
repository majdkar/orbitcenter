using System;
namespace SchoolV01.Application.Requests.Services

{
    public class GetPagedOverTimesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}