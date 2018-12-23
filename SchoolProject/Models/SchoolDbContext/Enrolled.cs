using System;
using System.Collections.Generic;

namespace SchoolProject.Models.SchoolDbContext
{
    public partial class Enrolled
    {
        public int Snum { get; set; }
        public string Cname { get; set; }

        public Student SnumNavigation { get; set; }
    }
}
