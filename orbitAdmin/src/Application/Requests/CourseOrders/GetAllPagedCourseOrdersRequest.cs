
namespace SchoolV01.Application.Requests.Courses
{
    public class GetAllPagedCourseOrdersRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
