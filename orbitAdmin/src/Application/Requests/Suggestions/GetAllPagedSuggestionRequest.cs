

namespace SchoolV01.Application.Requests.Suggestions
{
    public class GetAllPagedSuggestionRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}
