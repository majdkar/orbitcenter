using System.Linq;

namespace SchoolV01.Client.Infrastructure.Routes
{
    public static class ProductsEndpoints
    {
        //products
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/products/GetAllPaged?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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


        public static string GetAllPagedSearchProduct(int pageNumber, int pageSize, string searchString, string[] orderBy, string productname, int propductcategoryid, int propductSubcategoryid, int propductSubSubcategoryid, int propductSubSubSubcategoryid, decimal fromprice, decimal toprice)
        {
            var url = $"api/v1/products/GetAllPagedSearchProduct?productname={productname}&propductcategoryid={propductcategoryid}&propductSubcategoryid={propductSubcategoryid}&propductSubSubcategoryid={propductSubSubcategoryid}&propductSubSubSubcategoryid={propductSubSubSubcategoryid}&fromprice={fromprice}&toprice={toprice}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
            var url = $"api/v1/products/GetAllPagedByCategoryId?parentCategory={parentCategory}&brandId={brandId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string GetAllPagedProductByComanyId(int pageNumber, int pageSize, string searchString, string[] orderBy,int companyId)
        {
            var url = $"api/v1/products/GetAllPagedProductByComanyId?companyId={companyId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string GetAllPagedProductByCategoryId(int pageNumber, int pageSize, string searchString, string[] orderBy, int categoryId)
        {
            var url = $"api/v1/products/GetAllPagedProductByCategoryId?categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
        public static string GetAllProductsBySearch(int pageNumber, int pageSize, string searchString, string[] orderBy, string nameEn, int productParentCategoryId, int productSubCategoryId, int productSubSubCategoryId, int productSubSubSubCategoryId, int brandId, decimal retailpriceStart, decimal retailpriceEnd)
        {
            var url = $"api/v1/products/ProductsBySearch?nameEn={nameEn}&productParentCategoryId={productParentCategoryId}&productSubCategoryId={productSubCategoryId}&productSubSubCategoryId={productSubSubCategoryId}&productSubSubSubCategoryId={productSubSubSubCategoryId}&brandId={brandId}&retailpriceStart={retailpriceStart}&retailpriceEnd={retailpriceEnd}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";

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

        public static string GetProductById(int id)
        {
            return $"api/v1/Products/{id}";
        }
        public static string SaveForCompanyProfile = "api/v1/products/AddEditCompanyProduct";
        public static string DeleteProduct = "api/v1/products";
        public static string GetAll = "api/v1/products";
        public static string ExportFilteredByCompany(string searchString)
        {
            return $"api/v1/products/export?searchString={searchString}";
        }


        //products properties
        public static string GetAllProductProperties(int productId)
        {
            return $"api/v1/ProductProperties/GetAllByProduct/{productId}";
        }
        public static string GetProductPropertyById(int id)
        {
            return $"api/v1/ProductProperties/{id}";
        }
        public static string SaveProperty = "api/v1/ProductProperties";
        public static string DeleteProperty = "api/v1/ProductProperties";


        //products images
        public static string GetAllProductImages(int productId)
        {
            return $"api/v1/ProductImages/GetAllByProduct/{productId}";
        }

        public static string GetAllProductVideos(int productId)
        {
            return $"api/v1/ProductVideos/GetAllByProduct/{productId}";
        }

        public static string SaveImage = "api/v1/ProductImages";

        public static string SaveVideo = "api/v1/ProductVideos";

        public static string DeleteImage = "api/v1/ProductImages";

        public static string DeleteVideo = "api/v1/ProductVideo";

        public static string SetDefaultImage(int productId, int imageId)
        {
            return $"api/v1/ProductImages/SetDefault/{productId}/{imageId}";
        }

        //products Options
        public static string GetAllProductOptions(int productId)
        {
            return $"api/v1/ProductOptions/GetAllByProduct/{productId}";
        }
        public static string GetProductOptionById(int id)
        {
            return $"api/v1/ProductOptions/{id}";
        }
        public static string SaveOption = "api/v1/ProductOptions";
        public static string DeleteOption = "api/v1/ProductOptions";

        //products Offers
        public static string GetAllProductOffers(int productId)
        {
            return $"api/v1/ProductOffers/GetAllByProduct/{productId}";
        }
        public static string GetAllPagedProductOffers(int productId,int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/ProductOffers/GetAllPagedByProduct?productId={productId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";

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
        public static string GetProductOfferById(int id)
        {
            return $"api/v1/ProductOffers/{id}";
        }
        public static string SaveOffer = "api/v1/ProductOffers";
        public static string DeleteOffer = "api/v1/ProductOffers";

        //products Weights
        public static string GetAllProductWeights(int productId)
        {
            return $"api/v1/ProductWeights/GetAllByProduct/{productId}";
        }
        public static string GetAllProductOptionWeights(int productOptionId)
        {
            return $"api/v1/ProductWeights/GetAllByProduct1/{productOptionId}";
        }
        public static string GetProductWeightById(int id)
        {
            return $"api/v1/ProductWeights/{id}";
        }
        public static string SaveWeight = "api/v1/ProductWeights";
        public static string DeleteWeight = "api/v1/ProductWeights";

        
 
        //products ratings
        public static string GetAllPagedProductRatings(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/ProductRatings/GetAllPagedProductRatings?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
        public static string GetProductRatingById(int id)
        {
            return $"api/v1/ProductRatings/{id}";
        }
        public static string SaveRating = "api/v1/ProductRatings";
        public static string Deleterating = "api/v1/ProductRatings";


        public static string GetAllProductRatings(int productId)
        {
            return $"api/v1/ProductRatings/GetAllByProductId/{productId}";
        }
    }
}