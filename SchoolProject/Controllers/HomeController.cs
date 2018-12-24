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
        public IActionResult AddStudent(StudentViewModel model)
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteStudent(int studentId)
        {
            return View();
        }

        public IActionResult Course()
        {
            var courses = db.Course.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddCourse()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DeleteCourse()
        {
            return View();
        }

    }
}
