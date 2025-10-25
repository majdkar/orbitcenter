
namespace SchoolV01.Client.Infrastructure.Routes
{
    public class PayTypesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/PayTypes/export";

        public static string GetAll = "api/v1/PayTypes";
        public static string Delete = "api/v1/PayTypes";
        public static string Save = "api/v1/PayTypes";
        public static string GetCount = "api/v1/PayTypes/count";
    }
}
