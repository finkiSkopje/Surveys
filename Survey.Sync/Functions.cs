using Survey.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Survey.DAL;
using Survey.Repositories;
using Survey.Domain;
using System.Xml;
using System.IO;

namespace Survey.Sync
{
    public class Functions : IFunctions
    {
        public UnitOfWork uiw = new UnitOfWork();
        public bool CheckForUpdateSemesterChanges(int currentSem, int currentYear, int lastSem, int lastYear)
        {
            if (currentSem == lastSem && currentYear == lastYear)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public void UpdateTeacherCourses(string identity)
        {
            //CODE OMMITED FOR SECURITY REASONS
        }

        public void UpdateStudentCourses(string index)
        {
            //CODE OMMITED FOR SECURITY REASONS
        }

        // 1-winter, 2-summer
        public void GetCurrentSemester(DateTime dt, out int currentSemester, out int year)
        {
            currentSemester = 0;

            var winterSemester = new List<int> { 1, 10, 11, 12 };
            var summerSemester = new List<int> { 2, 3, 4, 5, 6, 7, 8, 9 };


            var currentDay = dt.Day;
            var currentMonth = dt.Month;
            year = dt.Year;

            var isWinter = winterSemester.Contains(currentMonth);

            if (isWinter)
            {
                currentSemester = 1;
                if (currentMonth == 1)
                    year = dt.Year - 1;
            }
            else
            {
                if (currentMonth == 9)
                {
                    if (currentDay > 14)
                        currentSemester = 1;
                    else currentSemester = 2;
                }
                else currentSemester = 2;

            }
        }

        public void GetLastSemester(DateTime dt, out int lastSemester, out int lastYear)
        {
            lastSemester = 0; lastYear = 0;
            int currentSemester = 0, cYear = 0;
            GetCurrentSemester(dt, out currentSemester, out cYear);
            if (currentSemester == 1)
            {
                lastSemester = 2;
                lastYear = cYear;
            }
            else
            {
                lastSemester = 1;
                lastYear = cYear - 1;
            }
        }

        public AppUserTypes RegisterUser(string netid)
        {
            //CODE OMMITED FOR SECURITY REASONS
            return AppUserTypes.anonymous;
        }

        public string GetCurrentSemesterName()
        {
            var dt = DateTime.Now;
            int year = 0;
            int semester = 0;
            GetCurrentSemester(dt, out semester, out year);
            string semType = semester == 1 ? "зимски" : "летен";
            string semYear = semester == 1 ?
                (dt.Month >= 7 && dt.Month <= 12) ?
                string.Format("{0}/{1}", dt.Year, dt.Year + 1) :
                string.Format("{0}/{1}", dt.Year - 1, dt.Year) :
                string.Format("{0}/{1}", dt.Year - 1, dt.Year);


            return string.Format("{0} - {1}", semType, semYear);
        }

        public bool CheckUsersToken(string token)
        {
			//CODE OMMITED FOR SECURITY REASONS
            return true;
        }

        public IEnumerable<DateTime> GenerateDateList(DateTime dt)
        {
            var list = new List<DateTime>();

            for (int i = -3; i < 4; i++)
            {
                list.Add(dt.AddDays(i * 7));
            }

            return list;
        }

        public void AuthenticateCASMock(string ticket, out AppUserTypes result, out User user)
        {
            string netid = ticket;

            if (netid != null)
            {
                user = uiw.Repository<User>().GetByIdentity(netid);
                if (user == null)
                {
                    result = RegisterUser(netid);
                    user = uiw.Repository<User>().GetByIdentity(netid);
                }

                if (user != null)
                {
                    if (user.RoleId == 1)
                    {
                        UpdateTeacherCourses(user.Username);
                        result = AppUserTypes.teacher;
                    }
                    else if (user.RoleId == 2)
                    {
                        UpdateStudentCourses(user.Username);
                        result = AppUserTypes.student;
                    }
                    else if (user.RoleId == 3)
                    {
                        result = AppUserTypes.admin;
                    }
                }
            }
        }

        public void AuthenticateCAS(string ticket, out AppUserTypes result, out User user)
        {
			//CODE OMMITED FOR SECURITY REASONS
			
            string netid = ticket;

            if (netid != null)
            {
                user = uiw.Repository<User>().GetByIdentity(netid);
                if (user == null)
                {
                    result = RegisterUser(netid);
                    user = uiw.Repository<User>().GetByIdentity(netid);
                }

                if (user != null)
                {
                    if (user.RoleId == 1)
                    {
                        UpdateTeacherCourses(user.Username);
                        result = AppUserTypes.teacher;
                    }
                    else if (user.RoleId == 2)
                    {
                        UpdateStudentCourses(user.Username);
                        result = AppUserTypes.student;
                    }
                    else if (user.RoleId == 3)
                    {
                        result = AppUserTypes.admin;
                    }
                }
            }
        }
    }
}