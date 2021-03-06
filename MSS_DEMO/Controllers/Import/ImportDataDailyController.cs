﻿using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System;
using MSS_DEMO.Core.Interface;
using MSS_DEMO.Repository;
using MSS_DEMO.Core.Import;
using MSS_DEMO.Common;

namespace MSS_DEMO.Controllers
{
    [CheckCredential(Role_ID = "4")]
    public class ImportDataDailyController : Controller
    {
        private IUnitOfWork unitOfWork;
        private IGetRow getRow;
        public ImportDataDailyController(IUnitOfWork _unitOfWork, IGetRow _getRow)
        {
            this.unitOfWork = _unitOfWork;
            this.getRow = _getRow;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            int userID = int.Parse(Request["UserID"]);
            string _dateImport = Request["dateImport"];          
            CSVConvert csv = new CSVConvert();
            string messageImport = "";
            HttpFileCollectionBase files = Request.Files;
            if (files.Count == 0)
            {
                messageImport = "Please select the file first to upload!";
                return Json(new { message = messageImport }, JsonRequestBehavior.AllowGet);
            }
            for (int i = 0; i < files.Count; i++)
            {
                HttpPostedFileBase postedFile = files[i];

                if (postedFile != null)
                {
                    try
                    {
                        string fileExtension = Path.GetExtension(postedFile.FileName);
                        if (fileExtension != ".csv")
                        {
                            messageImport = "Please select the excel file with .csv extension";
                            return Json(new { message = messageImport }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        if (fileExtension == ".csv")
                        {
                            using (var sreader = new StreamReader(postedFile.InputStream))
                            {
                                if (postedFile.FileName.Contains("specialization-report"))
                                {
                                    string[] headers = sreader.ReadLine().Split(',');
                                    var listIdSubject = unitOfWork.Specifications.GetAll();
                                    while (!sreader.EndOfStream)
                                    {
                                        List<string> rows = csv.RegexRow(sreader);
                                        if (!unitOfWork.Students.IsExtisStudent(rows[1].ToString().Split('@')[0]))
                                        {
                                            continue;
                                        }
                                        unitOfWork.SpecificationsLog.Insert(getRow.GetStudentSpec(rows, userID,_dateImport, listIdSubject));
                                    }
                                }
                                else
                                if (postedFile.FileName.Contains("usage-report"))
                                {
                                    string[] headers = sreader.ReadLine().Split(',');
                                    var listIdCourses = unitOfWork.Courses.GetListID();
                                    while (!sreader.EndOfStream)
                                    {
                                        List<string> rows = csv.RegexRow(sreader);
                                        if (!unitOfWork.Students.IsExtisStudent(rows[1].ToString().Split('@')[0]))
                                        {
                                            continue;
                                        }
                                        unitOfWork.CoursesLog.Insert(getRow.GetStudentCourse(rows, userID, _dateImport, listIdCourses));
                                    }
                                }
                                else
                                {
                                    messageImport = "Please choose Specialization-report file or Usage-report file!";
                                    return Json(new { message = messageImport }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            if (unitOfWork.Save())
                            {
                                messageImport = "Import successfull!";
                            }
                            else
                            {
                                messageImport = "Exception!";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        messageImport = ex.Message;
                    }
                }
                else
                {
                    messageImport = "Please select the file first to upload!";
                }
            }
            return Json(new { message = messageImport }, JsonRequestBehavior.AllowGet);
        }
    }
}

