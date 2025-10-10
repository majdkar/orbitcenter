
namespace SchoolV01.Application.Requests.Products
{
    public class GetAllPagedProductOrdersRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
