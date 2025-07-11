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
using SchoolV01.Application.Features.Courses.Queries.GetAllPaged;

namespace SchoolV01.Client.Infrastructure.Managers.Courses
{
    public interface ICourseSeoManager : IManager
    {
        Task<IResult<List<GetAllCourseSeosResponse>>> GetAllByCourseAsync(int CourseId);

        Task<PaginatedResult<GetAllCourseSeosResponse>> GetAllPagedByCourseAsync(GetAllPagedCourseSeosRequest request);
        Task<IResult<GetCourseSeoByIdResponse>> GetByIdAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditCourseSeoCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}
