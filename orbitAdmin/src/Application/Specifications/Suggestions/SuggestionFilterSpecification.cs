using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Suggestions;
using SchoolV01.Domain.Enums;

namespace SchoolV01.Application.Specifications.Suggestions
{
    public class SuggestionFilterSpecification : HeroSpecification<Suggestion>
    {
        public SuggestionFilterSpecification(string searchString, SuggestionType type)
        {
    
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.Description.Contains(searchString) || p.Email.Contains(searchString))
                                && !p.Deleted;
            }
            else
            {
                Criteria = p => !p.Deleted && (string.IsNullOrEmpty(type.ToString()) || p.Type == type) ;
            }
        }
    }
}
