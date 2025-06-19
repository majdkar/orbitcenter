using System.Linq;

namespace SchoolV01.Client.Infrastructure.Routes
{
    public static class CoursesEndpoints
    {
        //Courses
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/Courses/GetAllPaged?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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


        public static string GetAllPagedSearchCourse(int pageNumber, int pageSize, string searchString, string[] orderBy, string Coursename, int propductcategoryid, int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice)
        {
            var url = $"api/v1/Courses/GetAllPagedSearchCourse?Coursename={Coursename}&propductcategoryid={propductcategoryid}&propductSubcategoryid={propductSubcategoryid}&propductSubSubcategoryid={propductSubSubcategoryid}&propductSubSubSubcategoryid={propductSubSubSubcategoryid}&fromprice={fromprice}&toprice={toprice}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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


        public static string GetAllPagedByCategoryId(int pageNumber, int pageSize, string searchString, string[] orderBy,int parentCategory,int brandId)
        {
            var url = $"api/v1/Courses/GetAllPagedByCategoryId?parentCategory={parentCategory}&brandId={brandId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string GetAllPagedCourseByComanyId(int pageNumber, int pageSize, string searchString, string[] orderBy,int companyId)
        {
            var url = $"api/v1/Courses/GetAllPagedCourseByComanyId?companyId={companyId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string GetAllPagedCourseByCategoryId(int pageNumber, int pageSize, string searchString, string[] orderBy, int categoryId)
        {
            var url = $"api/v1/Courses/GetAllPagedCourseByCategoryId?categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
        public static string GetAllCoursesBySearch(int pageNumber, int pageSize, string searchString, string[] orderBy, string nameEn, int CourseParentCategoryId, int CourseSubCategoryId, int CourseSubSubCategoryId, int CourseSubSubSubCategoryId, int brandId, decimal retailpriceStart, decimal retailpriceEnd)
        {
            var url = $"api/v1/Courses/CoursesBySearch?nameEn={nameEn}&CourseParentCategoryId={CourseParentCategoryId}&CourseSubCategoryId={CourseSubCategoryId}&CourseSubSubCategoryId={CourseSubSubCategoryId}&CourseSubSubSubCategoryId={CourseSubSubSubCategoryId}&brandId={brandId}&retailpriceStart={retailpriceStart}&retailpriceEnd={retailpriceEnd}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";

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

        public static string GetCourseById(int id)
        {
            return $"api/v1/Courses/{id}";
        }
        public static string SaveForCompanyProfile = "api/v1/Courses/AddEditCompanyCourse";
        public static string DeleteCourse = "api/v1/Courses";
        public static string GetAll = "api/v1/Courses";
        public static string ExportFilteredByCompany(string searchString)
        {
            return $"api/v1/Courses/export?searchString={searchString}";
        }


        //Courses properties
        public static string GetAllCourseProperties(int CourseId)
        {
            return $"api/v1/CourseProperties/GetAllByCourse/{CourseId}";
        }
        public static string GetCoursePropertyById(int id)
        {
            return $"api/v1/CourseProperties/{id}";
        }
        public static string SaveProperty = "api/v1/CourseProperties";
        public static string DeleteProperty = "api/v1/CourseProperties";


        //Courses images
        public static string GetAllCourseImages(int CourseId)
        {
            return $"api/v1/CourseImages/GetAllByCourse/{CourseId}";
        }

        public static string GetAllCourseVideos(int CourseId)
        {
            return $"api/v1/CourseVideos/GetAllByCourse/{CourseId}";
        }

        public static string SaveImage = "api/v1/CourseImages";

        public static string SaveVideo = "api/v1/CourseVideos";

        public static string DeleteImage = "api/v1/CourseImages";

        public static string DeleteVideo = "api/v1/CourseVideo";

        public static string SetDefaultImage(int CourseId, int imageId)
        {
            return $"api/v1/CourseImages/SetDefault/{CourseId}/{imageId}";
        }

        //Courses Options
        public static string GetAllCourseOptions(int CourseId)
        {
            return $"api/v1/CourseOptions/GetAllByCourse/{CourseId}";
        }
        public static string GetCourseOptionById(int id)
        {
            return $"api/v1/CourseOptions/{id}";
        }
        public static string SaveOption = "api/v1/CourseOptions";
        public static string DeleteOption = "api/v1/CourseOptions";

        //Courses Offers
        public static string GetAllCourseOffers(int CourseId)
        {
            return $"api/v1/CourseOffers/GetAllByCourse/{CourseId}";
        }
        public static string GetAllPagedCourseOffers(int CourseId,int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/CourseOffers/GetAllPagedByCourse?CourseId={CourseId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";

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
        public static string GetCourseOfferById(int id)
        {
            return $"api/v1/CourseOffers/{id}";
        }
        public static string SaveOffer = "api/v1/CourseOffers";
        public static string DeleteOffer = "api/v1/CourseOffers";

        //Courses Weights
        public static string GetAllCourseWeights(int CourseId)
        {
            return $"api/v1/CourseWeights/GetAllByCourse/{CourseId}";
        }
        public static string GetAllCourseOptionWeights(int CourseOptionId)
        {
            return $"api/v1/CourseWeights/GetAllByCourse1/{CourseOptionId}";
        }
        public static string GetCourseWeightById(int id)
        {
            return $"api/v1/CourseWeights/{id}";
        }
        public static string SaveWeight = "api/v1/CourseWeights";
        public static string DeleteWeight = "api/v1/CourseWeights";

        
 
        //Courses ratings
        public static string GetAllPagedCourseRatings(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/CourseRatings/GetAllPagedCourseRatings?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
        public static string GetCourseRatingById(int id)
        {
            return $"api/v1/CourseRatings/{id}";
        }
        public static string SaveRating = "api/v1/CourseRatings";
        public static string Deleterating = "api/v1/CourseRatings";


        public static string GetAllCourseRatings(int CourseId)
        {
            return $"api/v1/CourseRatings/GetAllByCourseId/{CourseId}";
        }
    }
}