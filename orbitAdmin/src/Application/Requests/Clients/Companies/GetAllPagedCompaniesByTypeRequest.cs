
namespace SchoolV01.Application.Requests.Clients.Companies
{
    public class GetAllPagedCompaniesByTypeRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public string CompanyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int CountryId { get; set; }
        public string Status { get; set; }
    }
}
