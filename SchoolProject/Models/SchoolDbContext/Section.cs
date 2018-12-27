using System;
using System.Collections.Generic;

namespace SchoolProject.Models.SchoolDbContext
{
    public partial class Section
    {
        public string Cname { get; set; }
        public int MeetsOn { get; set; }
        public TimeSpan MeetsAt { get; set; }
        public string Room { get; set; }

        public Course CnameNavigation { get; set; }
    }
}
