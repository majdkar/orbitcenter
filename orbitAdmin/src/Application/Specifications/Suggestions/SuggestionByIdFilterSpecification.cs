using SchoolV01.Application.Specifications.Base;
using SchoolV01.Domain.Entities.Suggestions;

namespace SchoolV01.Application.Specifications.Suggestions
{
    public class SuggestionByIdFilterSpecification : HeroSpecification<Suggestion>
    {
        public SuggestionByIdFilterSpecification(int Id)
        {
            Includes.Add(p => p.Client);
            Criteria = p => p.Id == Id;
        }
    }
}
