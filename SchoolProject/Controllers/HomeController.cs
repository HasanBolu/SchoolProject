using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class HomeController : Controller
    {
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
            return View();
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
