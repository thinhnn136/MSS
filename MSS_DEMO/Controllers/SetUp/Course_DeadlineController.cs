﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MSS_DEMO.Core.Implement;
using MSS_DEMO.Models;
using MSS_DEMO.Repository;
using PagedList;

namespace MSS_DEMO.Controllers.SetUp
{
    public class Course_DeadlineController : Controller
    {
        private IUnitOfWork unitOfWork;
        public Course_DeadlineController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }

        public ActionResult Index(DeadlineCoursesViewModel model, int? page, string searchCheck)
        {
            List<Cour_dealine> List = new List<Cour_dealine>();
            string SearchString = model.Courses_Name;
            if (searchCheck != null)
            {
                List = unitOfWork.DeadLine.GetPageList();
            }
            else
            {
                page = 1;
            }
            if (!String.IsNullOrEmpty(searchCheck))
            {
                if (!String.IsNullOrEmpty(SearchString))
                {
                    List = List.Where(s => s.Courses_Name.ToUpper().Contains(SearchString.ToUpper())).ToList();
                }
                if (!String.IsNullOrEmpty(model.Semester_ID))
                {
                    List = List.Where(s => s.Semester_Name.ToUpper().Contains(model.Semester_ID.ToUpper())).ToList();
                }
            }
            List<string> semester = unitOfWork.Semesters.GetAll().Select(o => o.Semester_Name).ToList();
            model.lstSemester = semester;
            model.searchCheck = searchCheck;
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            ViewBag.Count = List.Count();
            model.PageList = List.ToList().ToPagedList(pageNumber, pageSize);
            return View(model);

        }

        public ActionResult Details(int? id)
        {
            return View(unitOfWork.DeadLine.GetById(id));
        }

        public ActionResult Create()
        {
            ViewBag.Course_ID = new SelectList(unitOfWork.Courses.GetAll(), "Course_ID", "Course_Name");
            ViewBag.Semester_ID = new SelectList(unitOfWork.Semesters.GetAll(), "Semester_ID", "Semester_Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Semester_ID,Course_ID,Deadline,Course_Deadline_ID")] Course_Deadline course_Deadline)
        {
            ViewBag.CheckExits = "";
            ViewBag.Course_ID = new SelectList(unitOfWork.Courses.GetAll(), "Course_ID", "Course_Name");
            ViewBag.Semester_ID = new SelectList(unitOfWork.Semesters.GetAll(), "Semester_ID", "Semester_Name");
            if (unitOfWork.DeadLine.IsExitsDeadline(course_Deadline))
            {
                ViewBag.CheckExits = "true";
                return View();
            }
            if (ModelState.IsValid)
            {
                unitOfWork.DeadLine.Insert(course_Deadline);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }        
            return View(course_Deadline);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Course_ID = new SelectList(unitOfWork.Courses.GetAll(), "Course_ID", "Course_Name");
            ViewBag.Semester_ID = new SelectList(unitOfWork.Semesters.GetAll(), "Semester_ID", "Semester_Name");
            var deadline = unitOfWork.DeadLine.GetById(id);
            return View(deadline);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Semester_ID,Course_ID,Deadline,Course_Deadline_ID")] Course_Deadline course_Deadline)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.DeadLine.Update(course_Deadline);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.Course_ID = new SelectList(unitOfWork.Courses.GetAll(), "Course_ID", "Course_Name");
            ViewBag.Semester_ID = new SelectList(unitOfWork.Semesters.GetAll(), "Semester_ID", "Semester_Name");
            return View(course_Deadline);
        }

        public ActionResult Delete(int? id)
        {
            return View(unitOfWork.DeadLine.GetById(id));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var subject = unitOfWork.DeadLine.GetById(id);
            unitOfWork.DeadLine.Delete(subject);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

    }
}
