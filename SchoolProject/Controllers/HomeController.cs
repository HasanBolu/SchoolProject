using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Models;
using SchoolProject.Models.SchoolDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SchoolProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace SchoolProject.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        SchoolProjectContext db;
        MainService mainService;
        public HomeController()
        {
            this.db = new SchoolProjectContext();
            this.mainService = new MainService();
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            ClaimsIdentity claimsIdentity;
            var authProperties = new AuthenticationProperties
            {

            };

            var admin = new { Username = "admin", Password = "admin123" };
            if (model.Username == admin.Username && model.Password == admin.Password)
            {
                claimsIdentity = mainService.GetLoggedInUserData("admin", "admin", "Admin");
            }
            else if (db.Student.Where(s => s.Sname == model.Username && s.Password == model.Password).Any())
            {
                claimsIdentity = mainService.GetLoggedInUserData(model.Username, "student", "Student");
                claimsIdentity.AddClaim(new Claim("snum", db.Student.Where(s => s.Sname == model.Username && s.Password == model.Password).FirstOrDefault().Snum.ToString()));
            }
            else
            {
                ViewBag.Error = true;
                return View();
            }

            HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
            return RedirectToAction("Index");
        }

        [Authorize(Roles="Admin")]
        public IActionResult Student()
        {
            var students = db.Student.ToList();
            return View(students);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddStudent(Student model)
        {
            db.Student.Add(model);
            db.SaveChanges();
            return RedirectToAction("Student");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin,Student")]
        public IActionResult Course()
        {
            var courses = db.Course.Include(c => c.Section).Include(c => c.Enrolled).ToList();
            return View(courses);
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult AddCourseToStudent(string cname)
        {
            var enrolled = new Enrolled();
            enrolled.Cname = cname;
            enrolled.Snum = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "snum").Value);
            db.Enrolled.Add(enrolled);
            db.SaveChanges();

            return RedirectToAction("Course");
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult RemoveCourseOfStudent(string cname)
        {
            var enrolled = db.Enrolled.Where(e => e.Cname == cname && e.Snum == Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "snum").Value)).FirstOrDefault();
            db.Enrolled.Remove(enrolled);
            db.SaveChanges();

            return RedirectToAction("Course");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCourse(CourseViewModel model)
        {
            var course = new Course();
            course.Cname = model.Cname;
            course.CourseInfo = model.CourseInfo;
            db.Course.Add(course);
            db.SaveChanges();

            for (int i = 0; i < model.MeetsOn.Count(); i++)
            {
                var section = new Section();
                section.Cname = model.Cname;
                section.MeetsOn = model.MeetsOn[i];
                section.MeetsAt = TimeSpan.Parse(model.MeetsAt[i]);
                section.EndsAt = TimeSpan.Parse(model.EndsAt[i]);
                section.Room = model.Room;

                db.Section.Add(section);
            }

            db.SaveChanges();

            return RedirectToAction("Course");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCourse(string cname)
        {
            var enrolled = db.Enrolled.Where(e => e.Cname == cname);
            db.Enrolled.RemoveRange(enrolled);
            db.SaveChanges();

            var section = db.Section.Where(c => c.Cname == cname);
            db.Section.RemoveRange(section);
            db.SaveChanges();

            var course = db.Course.Where(c => c.Cname == cname);
            db.Course.RemoveRange(course);
            db.SaveChanges();

            return RedirectToAction("Course");
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public IActionResult LessonSchedule()
        {
            var snum = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(x=>x.Type == "snum").Value);
            var courses = db.Course.Include(c => c.Enrolled)
                                   .Include(c => c.Section)
                                   .Where(c => c.Enrolled.Where(e => e.Snum == snum).Any())
                                   .ToList();

            return View(courses);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Student")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
