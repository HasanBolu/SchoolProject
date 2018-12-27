using System;
using System.Collections.Generic;

namespace SchoolProject.Models.SchoolDbContext
{
    public partial class Course
    {
        public Course()
        {
            Enrolled = new HashSet<Enrolled>();
            Section = new HashSet<Section>();
        }

        public string Cname { get; set; }
        public string CourseInfo { get; set; }

        public ICollection<Enrolled> Enrolled { get; set; }
        public ICollection<Section> Section { get; set; }
    }
}
