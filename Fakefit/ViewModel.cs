using Fakefit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakefit
{
    public class ViewModel
    {
        public List<register> register { get; set; }
        public List<all_lessons> LessonsGroup { get; set; }

        public List<all_lessons> MyLessons { get; set; }
    }
}