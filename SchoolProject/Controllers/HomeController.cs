﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Models;
using SchoolProject.Models.SchoolDbContext;
using Microsoft.AspNetCore.Http;

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
            ViewBag.IsAdmin = HttpContext.Session.GetString("admin") == "true";
            ViewBag.IsStudent = HttpContext.Session.GetString("student") == "true";
            ViewBag.IsUser = (ViewBag.IsAdmin || ViewBag.IsStudent);
            
            return View();
        }

        public IActionResult Login()
        {
            HttpContext.Session.SetString("abc", "true");
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var admin = new { Username = "admin", Password = "admin123" };
            if (model.Username == admin.Username && model.Password == admin.Password)
            {
                HttpContext.Session.SetString("admin", "true");
            }
            else if (db.Student.Where(s=>s.Sname == model.Username && s.Password == model.Password).Any())
            {
                HttpContext.Session.SetString("student", "true");
            }
            else
            {
                ViewBag.Error = true;
                return View();
            }
            
            return RedirectToAction("Index");
        }

        public IActionResult Student()
        {
            if (HttpContext.Session.GetString("admin") != "true")
            {
                return RedirectToAction("Index");
            }

            ViewBag.IsAdmin = HttpContext.Session.GetString("admin") == "true";
            ViewBag.IsStudent = HttpContext.Session.GetString("student") == "true";
            ViewBag.IsUser = (ViewBag.IsAdmin || ViewBag.IsStudent);

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
            ViewBag.IsAdmin = HttpContext.Session.GetString("admin") == "true";
            ViewBag.IsStudent = HttpContext.Session.GetString("student") == "true";
            ViewBag.IsUser = (ViewBag.IsAdmin || ViewBag.IsStudent);

            if (!ViewBag.IsUser)
            {
                return RedirectToAction("Index");
            }

            var courses = db.Course.Include(c=>c.Section).ToList();
            return View(courses);
        }

        [HttpPost]
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
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("student");
            HttpContext.Session.Remove("admin");

            return RedirectToAction("Login");
        }

    }
}
