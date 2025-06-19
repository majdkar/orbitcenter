
using System.Linq;


namespace SchoolV01.Client.Infrastructure.Routes
{
    public class CourseCategoriesEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/CourseCategories?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
        public static string GetAllPagedMain(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/GetAllPagedMain?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
        public static string GetAllPagedSons(int categoryId,int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/CourseCategories/GetAllSonsAndClassification?categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
       
        public static string GetCount = "api/v1/CourseCategories/count";

        public static string GetCourseCategoryImage(int CourseCategoryId)
        {
            return $"api/v1/CourseCategories/image/{CourseCategoryId}";
        }

        public static string GetCourseCategoryIcon(int CourseCategoryId)
        {
            return $"api/v1/CourseCategories/icon/{CourseCategoryId}";
        }

        public static string GetCourseCategorySons(int CourseCategoryId)
        {
            return $"api/v1/CourseCategories/GetCategorySons/{CourseCategoryId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string GetAll = "api/v1/CourseCategories/GetAll";
        public static string Save = "api/v1/CourseCategories";
        public static string Delete = "api/v1/CourseCategories";
        public static string Export = "api/v1/CourseCategories/export";
        public static string ChangePassword = "api/identity/account/changepassword";
        public static string UpdateProfile = "api/identity/account/updateprofile";

        public static string GetCourseCategoryById(int id)
        {
            return $"api/v1/CourseCategories/{id}";
        }
    }
}
