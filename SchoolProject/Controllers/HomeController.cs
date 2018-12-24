using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Models;
using SchoolProject.Models.SchoolDbContext;

namespace SchoolProject.Controllers
{
    public class HomeController : Controller
    {
        SchoolProjectContext db;
        public HomeController()
        {
            this.db = new SchoolProjectContext();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Student()
        {
            var students = db.Student.ToList();
            return View(students);
        }

        [HttpPost]
        public IActionResult AddStudent(Student model)
        {
            db.Student.Add(model);
            db.SaveChanges();
            return RedirectToAction("Student");
        }

        [HttpGet]
        public IActionResult DeleteStudent(int snum)
        {
            var enrolled = db.Enrolled.Where(e => e.Snum == snum);
            db.Enrolled.RemoveRange(enrolled);
            db.SaveChanges();

            var student = db.Student.Find(snum);
            db.Student.Remove(student);
            db.SaveChanges();

            return RedirectToAction("Student");
        }

        public IActionResult Course()
        {
            var courses = db.Course.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddCourse(Course model)
        {
            db.Course.Add(model);
            db.SaveChanges();

            return RedirectToAction("Course");
        }

        [HttpPost]
        public IActionResult DeleteCourse(string cname)
        {
            var enrolled = db.Enrolled.Where(e => e.Cname == cname);
            db.Enrolled.RemoveRange(enrolled);
            db.SaveChanges();

            var course = db.Course.Where(c => c.Cname == cname);
            db.Course.RemoveRange(course);
            db.SaveChanges();

            return View();
        }

    }
}
