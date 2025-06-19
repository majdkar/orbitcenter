

namespace SchoolV01.Application.Requests.CourseCategories
{
    public class GetAllPagedCourseCategoriesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
