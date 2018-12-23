using System;
using System.Collections.Generic;

namespace SchoolProject.Models.SchoolDbContext
{
    public partial class Student
    {
        public Student()
        {
            Enrolled = new HashSet<Enrolled>();
        }

        public int Snum { get; set; }
        public string Sname { get; set; }
        public string Password { get; set; }

        public ICollection<Enrolled> Enrolled { get; set; }
    }
}
