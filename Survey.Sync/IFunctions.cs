using Survey.Domain;
using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Sync
{
    public interface IFunctions
    {
        bool CheckForUpdateSemesterChanges(int currentSem, int currentYear, int lastSem, int lastYear);
        void UpdateTeacherCourses(string identity);
        void UpdateStudentCourses(string index);
        void GetCurrentSemester(DateTime dt, out int currentSemester, out int year);
        void GetLastSemester(DateTime dt, out int lastSemester, out int lastYear);
        AppUserTypes RegisterUser(string netid);
        string GetCurrentSemesterName();
        bool CheckUsersToken(string token);
        IEnumerable<DateTime> GenerateDateList(DateTime dt);
        void AuthenticateCASMock(string ticket, out AppUserTypes result, out User user);
        void AuthenticateCAS(string ticket, out AppUserTypes result, out User user);
    }
}
