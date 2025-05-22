

namespace SchoolV01.Application.Requests.Products
{
    public class GetAllPagedProductOffersRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public int productId { get; set; }
    }
}
