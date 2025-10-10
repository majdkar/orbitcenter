using System.Linq;

namespace SchoolV01.Client.Infrastructure.Routes
{
    public static class ProductOrdersEndpoints
    {
        //ProductOrders
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/ProductOrders/GetAllPaged?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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


        public static string GetAllPagedSearchProduct(int pageNumber, int pageSize, string searchString, string[] orderBy, string OrderNumber, int ClientId, int ProductId, decimal fromprice, decimal toprice)
        {
            var url = $"api/v1/ProductOrders/GetAllPagedSearchProduct?OrderNumber={OrderNumber}&ProductId={ProductId}&ClientId={ClientId}&fromprice={fromprice}&toprice={toprice}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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



        public static string GetProductOrderById(int id)
        {
            return $"api/v1/ProductOrders/{id}";
        }
        public static string SaveOrder = "api/v1/ProductOrders";
        public static string DeleteOrder = "api/v1/ProductOrders";
        public static string GetAll = "api/v1/ProductOrders";
        public static string ExportFilteredByCompany(string searchString)
        {
            return $"api/v1/ProductOrders/export?searchString={searchString}";
        }


       



    }
}