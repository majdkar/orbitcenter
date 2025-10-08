using System.Linq;

namespace SchoolV01.Client.Infrastructure.Routes
{
    public static class CourseOrdersEndpoints
    {
        //CourseOrders
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/CourseOrders/GetAllPaged?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }


        public static string GetAllPagedSearchCourse(int pageNumber, int pageSize, string searchString, string[] orderBy, string OrderNumber, int ClientId, int CourseId, decimal fromprice, decimal toprice)
        {
            var url = $"api/v1/CourseOrders/GetAllPagedSearchCourse?OrderNumber={OrderNumber}&CourseId={CourseId}&ClientId={ClientId}&fromprice={fromprice}&toprice={toprice}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }



        public static string GetCourseOrderById(int id)
        {
            return $"api/v1/CourseOrders/{id}";
        }
        public static string SaveOrder = "api/v1/CourseOrders";
        public static string DeleteOrder = "api/v1/CourseOrders";
        public static string GetAll = "api/v1/CourseOrders";
        public static string ExportFilteredByCompany(string searchString)
        {
            return $"api/v1/CourseOrders/export?searchString={searchString}";
        }


       



    }
}