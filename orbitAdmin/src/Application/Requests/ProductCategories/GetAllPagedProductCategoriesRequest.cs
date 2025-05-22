

namespace SchoolV01.Application.Requests.ProductCategories
{
    public class GetAllPagedProductCategoriesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
