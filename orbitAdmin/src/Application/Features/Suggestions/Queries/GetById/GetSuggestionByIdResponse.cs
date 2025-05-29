

using SchoolV01.Domain.Entities.Clients;
using SchoolV01.Domain.Enums;
using System;

namespace SchoolV01.Application.Features.Suggestions.Queries.GetById
{
    public class GetSuggestionByIdResponse
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public virtual Client Client { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string? Reply { get; set; }
        public string Mobile { get; set; }
        public string UserId { get; set; }
        public DateTime CreateOn { get; set; }

        public SuggestionType Type { get; set; }

    }
}
