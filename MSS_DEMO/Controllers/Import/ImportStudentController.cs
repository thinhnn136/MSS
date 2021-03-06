﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MSS_DEMO.Models;
using System.IO;
using MSS_DEMO.Repository;
using MSS_DEMO.Core.Interface;
using MSS_DEMO.Core.Import;
using System.Text;
using System.Linq;

namespace MSS_DEMO.Controllers
{
    public class ImportStudentController : Controller
    {
        private IUnitOfWork unitOfWork;
        private IGetRow getRow;
        public ImportStudentController(IUnitOfWork _unitOfWork, IGetRow _getRow)
        {
            this.unitOfWork = _unitOfWork;
            this.getRow = _getRow;
        }

        public ActionResult Index()
        {
            List<Semester> semester = unitOfWork.Semesters.GetAll();
            List<Semester> _semester = new List<Semester>();
            _semester.Add(new Semester { Semester_ID = "None", Semester_Name = "--- Choose Semester ---" });
            foreach (var sem in semester)
            {
                _semester.Add(sem);
            }
            ViewBag.Semester_ID = new SelectList(_semester, "Semester_ID", "Semester_Name");
            return View();
        }

        [HttpPost]
        public ActionResult Import()
        {
            string messageImport = "";
            CSVConvert csv = new CSVConvert();
            try
            {
                HttpPostedFileBase postedFile = Request.Files[0];
                string semester = Request["Semester"];
                if (postedFile != null)
                {
                    try
                    {
                        string fileExtension = Path.GetExtension(postedFile.FileName);
                        if (fileExtension != ".csv")
                        {
                            messageImport = "Please select the csv file with .csv extension";
                            return Json(new { message = messageImport });
                        }
                        try
                        {
                            using (var sreader = new StreamReader(postedFile.InputStream))
                            {
                                string[] headers = sreader.ReadLine().Split(',');
                                while (!sreader.EndOfStream)
                                {
                                    List<string> rows = csv.RegexRow(sreader);
                                    //if (!unitOfWork.Classes.CheckExitsClass(getRow.GetClassStudent(rows).Class_ID)
                                    //    || 
                                    if (unitOfWork.Subject.CheckExitsSubject(getRow.GetSubjectStudent(rows).Subject_ID))
                                    {
                                        if (unitOfWork.Students.CheckExitsStudent(getRow.GetStudent(rows, semester).Roll)
                                            && !unitOfWork.Subject.IsExitsSubject(getRow.GetSubjectStudent(rows).Subject_ID))
                                        {
                                            unitOfWork.SubjectStudent.Insert(getRow.GetSubjectStudent(rows));
                                           // unitOfWork.ClassStudent.Insert(getRow.GetClassStudent(rows));
                                        }
                                        else
                                        if (!unitOfWork.Students.CheckExitsStudent(getRow.GetStudent(rows, semester).Roll))
                                        {
                                            unitOfWork.Students.Insert(getRow.GetStudent(rows, semester));
                                            unitOfWork.SubjectStudent.Insert(getRow.GetSubjectStudent(rows));                                  
                                            //unitOfWork.ClassStudent.Insert(getRow.GetClassStudent(rows));
                                        }
                                    }                       
                                }
                            }
                            if (unitOfWork.Save())
                            {
                                messageImport = "Import successfull!";
                            }
                            else
                            {
                                messageImport = "Import Fail!";
                            }
                        }
                        catch (Exception ex)
                        {
                            messageImport = ex.Message;
                        }
                        return Json(new { message = messageImport });
                    }
                    catch (Exception ex)
                    {
                        messageImport = ex.Message;
                    }
                }
                else
                {
                    messageImport = "Please select the file first to upload.";
                }
            }
            catch
            {
                messageImport = "Please select the file first to upload.";
            }
            return Json(new { message = messageImport });
        }
        public void Export_Student_CSV()
        {
            var sb = new StringBuilder(); 
            Type type = typeof(Student);
            var props = type.GetProperties();
            sb.Append(string.Join(",", "No", "Full Name","Email", "External Id","Subject"));
            sb.Append(Environment.NewLine);
            sb.Append(string.Join(",", "SE0001", "Full Name SE0001", "FullnameSE0001@fpt.edu.vn", "HRM201c-HN-SE0001", "HRM201c"));
            sb.Append(Environment.NewLine);
            sb.Append(string.Join(",", "SE0002", "Full Name SE0002", "FullnameSE0002@fpt.edu.vn", "SSL201c-SG-SE0002", "SSL201c"));
            sb.Append(Environment.NewLine);
            sb.Append(string.Join(",", "SE0003", "Full Name SE0003", "FullnameSE0003@fpt.edu.vn", "PMG201c-DN-SE0003", "PMG201c"));
            sb.Append(Environment.NewLine);
            var response = System.Web.HttpContext.Current.Response;
            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition", "attachment;filename=StudentTemplate.CSV ");
            response.ContentType = "text/plain";
            response.Write(sb.ToString());
            response.End();
        }

    }
}
