using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Domain
{
    public class SurveyCoursesEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsRequired { get; set; }
        public string CourseName { get; set; }
        public string TeacherNameSurname { get; set; }
    }
}
