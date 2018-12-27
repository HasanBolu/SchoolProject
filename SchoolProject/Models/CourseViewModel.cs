using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolProject.Models
{
    public class CourseViewModel
    {
        public string Cname { get; set; }
        public string CourseInfo { get; set; }
        public int[] MeetsOn { get; set; }
        public string[] MeetsAt { get; set; }
        public string[] EndsAt { get; set; }
        public string Room { get; set; }
    }

    


}
