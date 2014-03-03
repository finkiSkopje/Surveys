using Survey.Domain;
using Survey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.WebSite.Models
{
    public class StudentsSurveyModel
    {
        public SurveyCours SurveyCourse { get; set; }
        public List<int> UserAnswers { get; set; }
        public List<UserAnswerEntity> UserAnswersString { get; set; }
        public List<int> UserAnswersSecondUser { get; set; }
        public IList<TeachersEntity> Assistants { get; set; }
        public int? TeacherId { get; set; }
        public int? SurveyId { get; set; }
    }
}