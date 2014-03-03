using Survey.DAL;
using Survey.Domain;
using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Repositories
{
    public static class UsersRepository
    {
        public static User GetByIdentity(this IRepository<User> usersRepository, string identity)
        {
            return usersRepository.Get(filter: x => x.Username == identity).FirstOrDefault();
        }

        public static IList<TeachersEntity> GetAssistantsByCourse(this IRepository<User> usersRepository,
            string courseCode)
        {
            return usersRepository.GetQuery()
                .Where(x => x.SemesterCourses.Count(y => y.CourseCode == courseCode) > 0 &&
                    x.IsTeacher == true //&& x.IsAssistant == true
                    )
                .Select(x => new TeachersEntity
                {
                    Id = x.Id,
                    NameSurname = x.Surname + " " + x.Name
                })
                .OrderBy(x => x.NameSurname)
                .ToList();
        }
    }
}
