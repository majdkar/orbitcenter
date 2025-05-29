using AutoMapper;

using SchoolV01.Application.Features.Suggestions.Commands.AddEdit;
using SchoolV01.Application.Features.Suggestions.Queries.GetAll;
using SchoolV01.Application.Features.Suggestions.Queries.GetById;
using SchoolV01.Domain.Entities.GeneralSettings;
using SchoolV01.Domain.Entities.Suggestions;

namespace SchoolV01.Application.Mappings
{
    public class SuggestionProfile : Profile
    {
        public SuggestionProfile()
        {
            CreateMap<AddEditSuggestionCommand, Suggestion>().ReverseMap();
            CreateMap<AddEditReplyCommand, Suggestion>().ReverseMap();

            CreateMap<GetAllSuggestionsResponse, Suggestion>().ReverseMap();
            CreateMap<GetSuggestionByIdResponse, Suggestion>().ReverseMap();

        }
    }
}