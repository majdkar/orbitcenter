
namespace SchoolV01.Application.Requests.Products
{
    public class GetAllPagedProductsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
