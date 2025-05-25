using System;
using System.Linq;
namespace SchoolV01.Client.Infrastructure.Routes
{
    public static class CompaniesEndpoints
    {
       
       

        public static string GetAllPagedClientCompanies(string companyName, string email, string phoneNumber, int CountryId, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/companies/PagedClientCompanies?companyName={companyName}&email={email}&phoneNumber={phoneNumber}&CountryId={CountryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

     
        public static string GetAllPendingPaged(string companyName, string email, string phoneNumber, int CountryId, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/companies/PendingPaged?companyName={companyName}&email={email}&phoneNumber={phoneNumber}&CountryId={CountryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string GetAllRefusedPaged(string companyName, string email, string phoneNumber, int CountryId, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/companies/RefusedPaged?companyName={companyName}&email={email}&phoneNumber={phoneNumber}&CountryId={CountryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
        
        public static string GetAllAcceptedPaged(string companyName, string email, string phoneNumber, int CountryId, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/companies/AcceptedPaged?companyName={companyName}&email={email}&phoneNumber={phoneNumber}&CountryId={CountryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

    

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string GetById(int companyId)
        {
            return $"api/v1/companies/{companyId}";
        }

        public static string GetByClientId(int clientId)
        {
            return $"api/v1/companies/GetByClientId/{clientId}";
        }

        public static string GetAllAccepted()
        {
            return $"api/v1/companies/AcceptedPaged";
        }
        public static string GetAll = "api/v1/companies";
        public static string Save = "api/v1/companies";
        public static string Delete = "api/v1/companies";
        public static string Accept = "api/v1/companies/accept";
        public static string Refuse = "api/v1/companies/refuse";
        public static string Export = "api/v1/companies/export";
        public static string ChangePassword = "api/identity/account/changepassword";
        public static string UpdateProfile = "api/identity/account/updateprofile";
    }
}
