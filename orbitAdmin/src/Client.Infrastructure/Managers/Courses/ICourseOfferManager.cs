using SchoolV01.Application.Features.Courses.Commands.AddEdit;
using SchoolV01.Application.Features.Courses.Queries.GetAll;
using SchoolV01.Application.Features.Courses.Queries.GetById;
using SchoolV01.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SchoolV01.Application.Requests.Courses;

namespace SchoolV01.Client.Infrastructure.Managers.Courses
{
    public interface ICourseOfferManager : IManager
    {
        Task<IResult<List<GetAllCourseOffersResponse>>> GetAllByCourseAsync(int CourseId);
        Task<PaginatedResult<GetAllCourseOffersResponse>> GetAllPagedByCourseAsync(GetAllPagedCourseOffersRequest request);
        Task<IResult<GetCourseOfferByIdResponse>> GetByIdAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditCourseOfferCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
