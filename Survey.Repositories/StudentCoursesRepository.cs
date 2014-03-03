using Survey.DAL;
using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Repositories
{
    public static class StudentCoursesRepository
    {
        public static IEnumerable<StudentCours> GetByStudentId(
            this IRepository<StudentCours> studentCoursesRepository,
            int studentId, int year, int semester)
        {
            //return (studentCoursesRepository.GetQuery("SemesterCours,User")
            //    .Where(x => x.UserId == studentId && x.Semester == semester && x.Year == year)
            //    .OrderBy(x => x.SemesterCours.CourseName))
            //    .ToList();

            return studentCoursesRepository.Get(filter: x => x.UserId == studentId && x.Semester == semester && x.Year == year);
        }
    }
}
