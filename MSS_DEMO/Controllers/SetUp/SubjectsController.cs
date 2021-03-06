﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MSS_DEMO.Models;
using MSS_DEMO.Repository;
using PagedList;

namespace MSS_DEMO.Controllers
{
    public class SubjectsController : Controller
    {
        private IUnitOfWork unitOfWork;
        public SubjectsController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }
        public ActionResult Index(int? page, string SearchString, string searchCheck, string currentFilter)
        {
            List<Subject> List = new List<Subject>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            ViewBag.CurrentFilter = SearchString;
            if (!String.IsNullOrEmpty(searchCheck))
            {
                List = unitOfWork.Subject.GetPageList();
                if (!String.IsNullOrEmpty(SearchString))
                {
                    List = List.Where(s => s.Subject_ID.ToUpper().Contains(SearchString.ToUpper())).ToList();
                }
            }
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            ViewBag.Count = List.Count();
            return View(List.ToList().ToPagedList(pageNumber, pageSize));

        }
        public ActionResult Details(string id)
        {
            var Subject = unitOfWork.Subject.GetById(id);
            return View(Subject);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Subject_ID,Subject_Name")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Subject.Insert(subject);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(subject);
        }


        public ActionResult Edit(string id)
        {
            var subject = unitOfWork.Subject.GetById(id);
            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Subject_ID,Subject_Name")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Subject.Update(subject);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(subject);
        }
        public ActionResult Delete(string id)
        {
            var subject = unitOfWork.Subject.GetById(id);
            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var subject = unitOfWork.Subject.GetById(id);
            unitOfWork.Subject.Delete(subject);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
