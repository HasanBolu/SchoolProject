using System;
using System.Collections.Generic;

namespace SchoolProject.Models.SchoolDbContext
{
    public partial class Course
    {
        public string Cname { get; set; }
        public DateTime MeetsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public string Room { get; set; }
        public string CourseInfo { get; set; }
    }
}
