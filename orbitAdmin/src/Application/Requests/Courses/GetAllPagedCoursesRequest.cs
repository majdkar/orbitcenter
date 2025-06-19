
namespace SchoolV01.Application.Requests.Courses
{
    public class GetAllPagedCoursesRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
