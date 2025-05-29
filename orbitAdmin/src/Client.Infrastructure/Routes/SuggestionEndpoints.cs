using SchoolV01.Domain.Enums;
using System.Linq;

namespace SchoolV01.Client.Infrastructure.Routes
{
    public static class SuggestionEndpoints
    {

        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy, SuggestionType type)
        {
            var url = $"api/v1/Suggestions/GetAllPaged?type={type}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string ExportFiltered(string searchString, SuggestionType type)
        {
            return $"{Export}?searchString={searchString}&type={type}";
        }


        public static string GetById(int SuggestionId)
        {
            return $"api/v1/Suggestions/{SuggestionId}";
        }

        public static string Export = "api/v1/Suggestions/export";

        public static string Delete = "api/v1/Suggestions";
        public static string Save = "api/v1/Suggestions";
        public static string SaveReply = "api/v1/Suggestions/SaveReply";

        public static string GetCount = "api/v1/Suggestions/count";
    }
}