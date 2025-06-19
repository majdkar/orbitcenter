

namespace SchoolV01.Application.Requests.Courses
{
    public class GetAllPagedCourseOffersRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public int CourseId { get; set; }
    }
}
