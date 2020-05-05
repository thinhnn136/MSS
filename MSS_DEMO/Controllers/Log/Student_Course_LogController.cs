﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MSS_DEMO.Common;
using MSS_DEMO.Core.Implement;
using MSS_DEMO.Core.Import;
using MSS_DEMO.Models;
using MSS_DEMO.MssService;
using MSS_DEMO.Repository;
using PagedList;

namespace MSS_DEMO.Controllers.Log
{
    
    public class Student_Course_LogController : Controller
    {
        private IUnitOfWork unitOfWork;
        public Student_Course_LogController(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
        }
        [CheckCredential(Role_ID = "4")]
        public ActionResult Index(CoursesReportViewModel model, int? page, string searchCheck)
        {
            List<Student_Course_Log> LogList = new List<Student_Course_Log>();
            string SearchString = model.Email;
            model.searchCheck = searchCheck;
            List<SelectListItem> listSubjiect = new List<SelectListItem>();
            var subject = unitOfWork.Subject.GetAll();
            foreach (var sub in subject)
            {
                listSubjiect.Add(new SelectListItem
                {
                    Text = sub.Subject_Name,
                    Value = sub.Subject_ID
                });
            }
            List<SelectListItem> campusList = new List<SelectListItem>();
            var campus = unitOfWork.Campus.GetAll();
            foreach (var cam in campus)
            {
                campusList.Add(new SelectListItem
                {
                    Text = cam.Campus_Name,
                    Value = cam.Campus_ID
                });
            }
            List<SelectListItem> semesterList = new List<SelectListItem>();
            var semester = unitOfWork.Semesters.GetAll();
            foreach (var sem in semester)
            {
                semesterList.Add(new SelectListItem
                {
                    Text = sem.Semester_Name,
                    Value = sem.Semester_ID
                });
            }
            var listDate = unitOfWork.CoursesLog.GetAll().OrderByDescending(o => o.Date_Import).Where(o => o.Semester_ID == semester[0].Semester_ID).FirstOrDefault().Date_Import;
            List<string> date = new List<string>();
            date.Add(Convert.ToDateTime(listDate).ToString("dd/MM/yyyy"));
            model.importedDate = date;
            model.lstSemester = semesterList;
            model.lstCampus = campusList;
            model.listSubject = listSubjiect;
            if (searchCheck == null)
            {
                page = 1;
            }
            else
            {
                LogList = unitOfWork.CoursesLog.GetPageList();
            }
            if (!String.IsNullOrEmpty(searchCheck))
            {
                if (!String.IsNullOrWhiteSpace(SearchString))
                {
                    LogList = LogList.Where(s => s.Email.Trim().ToUpper().Contains(SearchString.Trim().ToUpper())).ToList();
                }
                if (!String.IsNullOrWhiteSpace(model.ImportedDate))
                {
                    DateTime dt = DateTime.ParseExact(model.ImportedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    LogList = LogList.Where(s => s.Date_Import == dt).ToList();
                }
                if (model.completedCourse != null)
                {
                    LogList = model.completedCourse =="Yes" ? LogList.Where(s => s.Completed == true).ToList() : LogList.Where(s => s.Completed == false).ToList();
                }
                if (model.compulsoryCourse != null)
                {
                    LogList = model.compulsoryCourse == "Yes" ? LogList = LogList.Where(s => s.Subject_ID != null).ToList() : LogList = LogList.Where(s => s.Subject_ID == null).ToList();
                }
                if (!String.IsNullOrWhiteSpace(model.Subject_ID))
                {                  
                    LogList = LogList.Where(s => s.Subject_ID == model.Subject_ID).ToList();
                }
                if (!String.IsNullOrWhiteSpace(model.Campus))
                {
                    LogList = LogList.Where(s => s.Campus == model.Campus).ToList();
                }
                if (!String.IsNullOrWhiteSpace(model.Semester_ID))
                {
                    LogList = LogList.Where(s => s.Semester_ID == model.Semester_ID).ToList();
                }
                if (LogList.Count == 0)
                {
                    ViewBag.Nodata = "Not found data";
                }
                else
                {
                    ViewBag.Nodata = "";
                }
            }
            List<string> completedCour = new List<string>() { "Yes","No"};
            List<string> compulsoryCour = new List<string>() { "Yes","No" };           
            model.completedCour = completedCour;
            model.compulsoryCour = compulsoryCour;
            ViewBag.CountRoll = LogList.Select(o => o.Roll).Distinct().Count();
            ViewBag.CountLog = LogList.Count();
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            model.PageList = LogList.ToList().ToPagedList(pageNumber, pageSize);
            return View(model);
        }
        public ActionResult Mentor()
        {
            var userMentor = (UserLogin)HttpContext.Session[CommonConstants.User_Session];
            DateTime date = DateTime.Now;
            var semester = unitOfWork.Semesters.GetAll().Where(sem => sem.Start_Date < date && sem.End_Date > date).FirstOrDefault().Semester_Name;
            var lstSubjectClass = unitOfWork.CoursesLog.getListSubjectClass(userMentor.UserName, semester);
            if (lstSubjectClass.Count > 0) ViewBag.checkData = "true";
            else ViewBag.checkData = "";
            return View(lstSubjectClass);
        }
        public ActionResult Detail(StatusOverviewModel model,string id,string searchCheck, string selectCoursCompleted, string selectFinalStatus)
        {
            DateTime date = DateTime.Now;
            var semester = unitOfWork.Semesters.GetAll().Where(sem => sem.Start_Date < date && sem.End_Date > date).FirstOrDefault();
            var userMentor = (UserLogin)HttpContext.Session[CommonConstants.User_Session];
            var listSubjectClass = unitOfWork.CoursesLog.getListSubjectClass(userMentor.UserName, semester.Semester_Name);
            CourseraApiSoapClient courseraApiSoap = new CourseraApiSoapClient();
            string jsonDataClass = "";
            model.searchCheck = searchCheck;
            List<StatusOverviewModel> Status = new List<StatusOverviewModel>();
            if (!String.IsNullOrEmpty(searchCheck))
            {
                
                try
                {
                    foreach (var subjectClass in listSubjectClass)
                    {
                        if (id == subjectClass.id)
                        {
                            string authenKey = "A90C9954-1EDD-4330-B9F3-3D8201772EEA";
                            jsonDataClass = courseraApiSoap.getStudents(authenKey, userMentor.UserName.Split('@')[0], subjectClass.Subject_ID.Trim(), subjectClass.Class_ID.Trim(), semester.Semester_Name);
                            var rollFap = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RollFAP>>(jsonDataClass);
                            var rolls = rollFap[0].RollNumber.ToString().Trim();
                            rolls = "," + rolls + ",";
                            var maxDate = new MSSEntities().Student_Course_Log.OrderByDescending(o => o.Date_Import).FirstOrDefault().Date_Import;
                            var statusList = new MSSEntities().sp_Get_Main_Report(maxDate, semester.Semester_ID, Convert.ToInt32(selectCoursCompleted), selectFinalStatus,model.Roll == null ? "" : model.Roll, id.Split('^')[0], "", rolls).ToList();
                            foreach (var item in statusList)
                            {
                                Status.Add(new StatusOverviewModel
                                {
                                    Roll = item.Roll,
                                    Email = item.Email,
                                    Note = item.Note,
                                    SubjectID = item.Subject_ID,
                                    SubjectName = item.Subject_Name,
                                    No_Compulsory_Course = item.No_Compulsory_Course,
                                    No_Course_Completed = item.No_Course_Completed,
                                    Spec_Completed = item.Spec_Completed,
                                    Final_Status = item.Final_status,
                                    Campus = item.Campus_ID
                                });
                            }
                            break;
                        }
                    }
                }
                catch
                {
                    Status = new List<StatusOverviewModel>();
                }
                if (Status.Count == 0)
                {
                    ViewBag.Nodata = "No data found";
                }
                else
                {
                    ViewBag.Nodata = "";
                }
            }
            ViewBag.Subject = id.Split('^')[0];
            ViewBag.Class = id.Split('^')[1];
            ViewBag.SemesterID = semester.Semester_ID;
            List<SelectListItem> selectFinal = new List<SelectListItem>();
            selectFinal.Add(new SelectListItem { Text = "--All--", Value = "" });
            selectFinal.Add(new SelectListItem { Text = "Completed", Value = "Completed" });
            selectFinal.Add(new SelectListItem { Text = "Not Completed", Value = "Not Completed" });
            ViewBag.selectFinalStatus = selectFinal;
            ViewBag.CountRoll = Status.Select(o => o.Roll).Distinct().Count();
            ViewBag.CountLog = Status.Count();
            model.OverviewList = Status;
            return View(model);
        }
        [HttpPost]
        public ActionResult AddNote(string id, string note)
        {   
            try
            {
                MSSEntities context = new MSSEntities();

                Mentor_Log log = new Mentor_Log();
                var Roll = id.Split('^')[0];
                var Semester_ID = id.Split('^')[2];
                var Subject_ID = id.Split('^')[1];


                var noteLog = context.Mentor_Log.Where(o => o.Roll.Trim() == Roll && o.Semester_ID.Trim() == Semester_ID.Trim() && o.Subject_ID.Trim() == Subject_ID.Trim()).FirstOrDefault();
                if (noteLog == null)
                {
                    context.Mentor_Log.Add(new Mentor_Log {
                        Roll = Roll,
                        Semester_ID = Semester_ID,
                        Subject_ID = Subject_ID,
                        Note = note
                    });
                    context.SaveChanges();
                }
                else
                {
                    noteLog.Note = note;
                    context.SaveChanges();
                }                             
            }
            catch (Exception ex)
            {
            }
            return Json(new { check = true }); ;
        }
        [HttpGet]
        public void Export(string check)
        {

            string searchCheck = check.Split('^')[0];
            string Campus = check.Split('^')[1];
            string Semester_ID = check.Split('^')[2];
            string Subject_ID = check.Split('^')[3];
            string completedSpec = check.Split('^')[4];
            string compulsorySpec = check.Split('^')[5];
            string ImportedDate = check.Split('^')[6];
            string Email = check.Split('^')[7];
            var LogList = unitOfWork.CoursesLog.GetPageList();
            if (searchCheck != "1")
            {
                if (Email != "8")
                {
                    LogList = LogList.Where(s => s.Email.Trim().ToUpper().Contains(Email.Trim().ToUpper())).ToList();
                }
                if (ImportedDate != "7")
                {
                    DateTime dt = DateTime.ParseExact(ImportedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    LogList = LogList.Where(s => s.Date_Import == dt).ToList();
                }
                if (completedSpec != "5")
                {
                    LogList = completedSpec == "Yes" ? LogList.Where(s => s.Completed == true).ToList() : LogList.Where(s => s.Completed == false).ToList();
                }
                if (compulsorySpec != "6")
                {
                    LogList = compulsorySpec == "Yes" ? LogList = LogList.Where(s => s.Subject_ID != null).ToList() : LogList = LogList.Where(s => s.Subject_ID == null).ToList();
                }
                if (Subject_ID != "4")
                {
                    LogList = LogList.Where(s => s.Subject_ID == Subject_ID.Trim()).ToList();
                }
                if (Campus != "2")
                {
                    LogList = LogList.Where(s => s.Campus == Campus.Trim()).ToList();
                }
                if (Semester_ID != "3")
                {
                    LogList = LogList.Where(s => s.Semester_ID == Semester_ID.Trim()).ToList();
                }
            }
            CSVConvert csv = new CSVConvert();
            var sb = new StringBuilder();
            var list = LogList.ToList();
            sb.Append(string.Join(",","Name", "Email", "Subject ID", "Campus", "Course Name", "Course Slug","University" ,"Enrollment Time", "Start Time", "Last ActivityTime", "Overall Progress", "Estimated",
                "Completed", "Status", "Program Slug", "Program Name", "Enrollment Source", "Completion Time", "Course Grade", "Date Import"));
            sb.Append(Environment.NewLine);
            foreach (var item in list)
            {
                sb.Append(string.Join(",",
                    csv.AddCSVQuotes(item.Name),
                    csv.AddCSVQuotes(item.Email),
                    csv.AddCSVQuotes(item.Subject_ID),
                    csv.AddCSVQuotes(item.Campus),
                    csv.AddCSVQuotes(item.Course_Name),
                    csv.AddCSVQuotes(item.Course_Slug),
                    csv.AddCSVQuotes(item.University),
                    csv.AddCSVQuotes(item.Course_Enrollment_Time.ToString().Contains("1/1/1970") ? "" : item.Course_Enrollment_Time.ToString()),
                    csv.AddCSVQuotes(item.Course_Start_Time.ToString().Contains("1/1/1970") ? "" : item.Course_Start_Time.ToString()),
                    csv.AddCSVQuotes(item.Last_Course_Activity_Time.ToString().Contains("1/1/1970") ? "" : item.Last_Course_Activity_Time.ToString()),
                    csv.AddCSVQuotes(item.Overall_Progress.ToString()),
                    csv.AddCSVQuotes(item.Estimated.ToString()),
                    csv.AddCSVQuotes(item.Completed.ToString()),
                    csv.AddCSVQuotes(item.Status.ToString()),
                    csv.AddCSVQuotes(item.Program_Slug),
                    csv.AddCSVQuotes(item.Program_Name),
                    csv.AddCSVQuotes(item.Enrollment_Source),
                    csv.AddCSVQuotes(item.Completion_Time.ToString().Contains("1/1/1970") ? "" : item.Completion_Time.ToString()),
                    csv.AddCSVQuotes(item.Course_Grade.ToString()),
                    csv.AddCSVQuotes(item.Date_Import.ToString())
                    ));
                sb.Append(Environment.NewLine);
            }
            var response = System.Web.HttpContext.Current.Response;
            response.BufferOutput = true;
            response.Clear();
            response.ClearHeaders();
            response.ContentEncoding = Encoding.Unicode;
            response.AddHeader("content-disposition", "attachment;filename=Usage-Report.CSV ");
            response.ContentType = "text/plain";
            response.Write(sb.ToString());
            response.End();
        }
    }
}
