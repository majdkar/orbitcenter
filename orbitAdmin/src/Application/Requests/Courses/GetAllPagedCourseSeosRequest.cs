

namespace SchoolV01.Application.Requests.Courses
{
    public class GetAllPagedCourseSeosRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public int CourseId { get; set; }
    }
}
