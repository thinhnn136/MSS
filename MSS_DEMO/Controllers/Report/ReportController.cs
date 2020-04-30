﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSS_DEMO.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using PagedList;
using MSS_DEMO.Common;
using System.Globalization;
using Rotativa;
using Microsoft.Ajax.Utilities;

namespace MSS_DEMO.Controllers
{
    [CheckCredential(Role_ID = "3")]
    public class ReportController : Controller
    {
       
        // GET: Report       
        public ActionResult Index(Report rp, string SelectDatetime, string searchCheck, string weekNumber, string SelectSemester)
        {
            List<Report> reportStudent = new List<Report>();
            List<Report> reportCourse= new List<Report>();

            var context = new MSSEntities();

            List<SelectListItem> selectSemes = new List<SelectListItem>();

            var listSemes = (from semes in context.Semesters
                             select semes).ToList();
            var orderedListSemes = listSemes.OrderByDescending(x => x.Start_Date).ToList();
            foreach (var item in orderedListSemes)
            {
                selectSemes.Add(new SelectListItem
                {
                    Text = item.Semester_Name,
                    Value = item.Semester_ID
                });
            }
            ViewBag.SelectSemester = selectSemes;

            if (String.IsNullOrEmpty(searchCheck) && orderedListSemes.Count > 0)
            {
                //SelectSemester = (from a in context.Semesters
                //                  where a.Start_Date < DateTime.Now && a.End_Date > DateTime.Now
                //                  select a.Semester_ID).FirstOrDefault();
                if (String.IsNullOrEmpty(searchCheck) && orderedListSemes.Count > 0)
                {
                    SelectSemester = orderedListSemes[0].Semester_ID;
                }
            }

            DateTime date;
            ViewBag.SelectDatetime = Date(SelectSemester);
            date = Convert.ToDateTime(Date(SelectSemester).Select(m => m.Value).FirstOrDefault());
            if (SelectDatetime != null)
            {
                date = Convert.ToDateTime(SelectDatetime);
            }
            if (searchCheck == "1")
            {
                List<string> dateL = new List<string>();
                dateL.Add(Convert.ToDateTime(date).ToString("dd/MM/yyyy"));
                ViewBag.SelectDatetime = new SelectList(dateL);
            }
            if (!String.IsNullOrEmpty(searchCheck))
            {
                var listSub = (from sub in context.Subjects
                               where sub.Subject_Active == true
                               select sub).ToList();
                foreach (var sub in listSub)
                {
                    var Totall = (from subject in context.Subjects
                                  join sub_stu in context.Subject_Student on subject.Subject_ID equals sub_stu.Subject_ID
                                  join stu in context.Students on sub_stu.Roll equals stu.Roll
                                  where subject.Subject_ID == sub.Subject_ID && stu.Semester_ID == SelectSemester && sub_stu.Semester_ID == SelectSemester
                                  select stu.Roll).Count();

                    var Type = (from spec in context.Specifications
                                where sub.Subject_ID == spec.Subject_ID && spec.Is_Real_Specification == true
                                select spec.Specification_ID).Count();
                    String ListType = "";
                    if (Type == 1)
                    {
                        ListType = "Spec";
                    }
                    else
                    {
                        ListType = "Course";
                    }

                    List<double> count = new List<double>();
                    List<string> name = new List<string>();

                    foreach (var cp in context.Campus)
                    {
                        count.Add(Campus(sub.Subject_ID, cp.Campus_ID, SelectSemester));
                        name.Add(cp.Campus_ID);
                    }
                    ViewBag.Name = name;
                    ViewBag.Cmp = count;

                    reportStudent.Add(new Report { Sub = sub.Subject_ID, Name = sub.Subject_Name, Type = ListType, Total = Totall, Cmp = count });

                    List<double> countS = new List<double>();
                    if (date != null)
                    {
                        int Total = Count(sub.Subject_ID, "", date);

                        foreach (var cp in context.Campus)
                        {
                            countS.Add(Count(sub.Subject_ID, cp.Campus_ID, date));
                        }
                        reportCourse.Add(new Report { Sub = sub.Subject_ID, Name = sub.Subject_Name, Study = percent(Total, Totall), Total = Total, Cmp = countS });
                    }

                }
                var TotalStudent = (from stu in context.Students
                                    where stu.Semester_ID == SelectSemester
                                    select stu.Roll).Count();
                List<double> temp = new List<double>();
                temp.Add(Campus("", "", SelectSemester));
                foreach (var cp in context.Campus)
                {
                    temp.Add(Campus("", cp.Campus_ID, SelectSemester));
                }
                ViewBag.Count = temp;
                ViewBag.TotalStudent1 = TotalStudent;
                int TotalStudent2 = Count("", "", date);
                ViewBag.TotalStudent2 = TotalStudent2;


                List<double> tempC = new List<double>();
                ViewBag.TotalPercent = percent(TotalStudent2, TotalStudent);
                tempC.Add(Count("", "", date));
                foreach (var cp in context.Campus)
                {
                    tempC.Add(Count("", cp.Campus_ID, date));
                }
                ViewBag.Count2 = tempC;

                List<double> per = new List<double>();
                //per.Add(percent(Count("", ""), Campus("", "")));
                foreach (var cp in context.Campus)
                {
                    per.Add(percent(Count("", cp.Campus_ID, date), Campus("", cp.Campus_ID, SelectSemester)));
                }
                ViewBag.Per = per;

                var studentComplete = (from stu_cour_log in context.Student_Course_Log
                                       where stu_cour_log.Completed == true && stu_cour_log.Course_ID != null && stu_cour_log.Date_Import == date && stu_cour_log.Semester_ID == SelectSemester
                                       select stu_cour_log.Roll).Distinct().Count();
                var courseComplete = (from stu_cour_log in context.Student_Course_Log
                                      where stu_cour_log.Completed == true && stu_cour_log.Course_ID != null && stu_cour_log.Date_Import == date && stu_cour_log.Semester_ID == SelectSemester
                                      select stu_cour_log.Course_ID).Count();

                ViewBag.studentComplete = studentComplete;
                ViewBag.courseComplete = courseComplete;

                var RollList = (from stu in context.Students
                                where stu.Semester_ID == SelectSemester
                                select stu.Roll).ToList();
                int CountStudent = 0;
                //DateTime nowDate = DateTime.Now;
                var starDate = (from semes in context.Semesters
                                where semes.Semester_ID == SelectSemester
                                select semes.Start_Date).FirstOrDefault();
                TimeSpan timeSpan = date - (DateTime)starDate;
                //var weekN = Math.Floor((double)(date.Day - starDate.Value.Day) / 7);
                double weekN = 0;
                if (date != DateTime.MinValue)
                {
                    weekN = Math.Floor(timeSpan.TotalDays / 7);
                }
                //ViewBag.Week = weekN;
                int weekOff = 0;
                if (!String.IsNullOrEmpty(weekNumber))
                {
                    weekOff = Int32.Parse(weekNumber);
                }
                ViewBag.weekNumber = weekOff;
                ViewBag.date = date;
                ViewBag.semester = SelectSemester;
                var weekTotal = weekN - weekOff;
                if (weekTotal > 0)
                {
                    foreach (var item in RollList)
                    {
                        var Estimated = (from stu_cour_log in context.Student_Course_Log
                                         where stu_cour_log.Roll == item && stu_cour_log.Course_ID != null && stu_cour_log.Date_Import == date && stu_cour_log.Semester_ID == SelectSemester
                                         select stu_cour_log.Estimated).ToList();
                        var EstimatedTotal = Estimated.Sum();
                        if ((EstimatedTotal / weekTotal) < 5)
                        {
                            CountStudent++;
                        }
                    }
                    var perc = percent(CountStudent, TotalStudent);
                    ViewBag.Estimated = CountStudent;
                    ViewBag.Percent = perc;
                }
            }

            rp.rp1 = reportStudent;
            rp.rp2 = reportCourse;
            return View(rp);
        }

        public ActionResult Enrollment(ListStudent ls,string searchString, string searchCheck, string SelectCampus, string SelectSubject, string SelectSemester, string SelectDatetime)
        {
            var context = new MSSEntities();
            List<ListStudent> studentList = new List<ListStudent>();

            List<SelectListItem> selectSemes = new List<SelectListItem>();

            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToUpper();
            }
            var listSemes = (from semes in context.Semesters
                             select semes).ToList();
            var orderedListSemes = listSemes.OrderByDescending(x => x.Start_Date).ToList();
            foreach (var item in orderedListSemes)
            {
                selectSemes.Add(new SelectListItem
                {
                    Text = item.Semester_Name,
                    Value = item.Semester_ID
                });
            }
            ViewBag.SelectSemester = selectSemes;
            if (String.IsNullOrEmpty(searchCheck) && orderedListSemes.Count > 0)
            {
                SelectSemester = orderedListSemes[0].Semester_ID;
            }

            DateTime date;
            ViewBag.SelectDatetime = Date(SelectSemester);
            date = Convert.ToDateTime(Date(SelectSemester).Select(m => m.Value).FirstOrDefault());
            if (SelectDatetime != null)
            {
                date = Convert.ToDateTime(SelectDatetime);
            }


            if (!String.IsNullOrEmpty(searchCheck))
            {

                var CourseStudent = (from stu_cour_log in context.Student_Course_Log
                                     where stu_cour_log.Semester_ID == SelectSemester
                                     select stu_cour_log.Roll).Distinct().ToList();
                var Student = (from stu in context.Students
                               where stu.Semester_ID == SelectSemester
                               select stu.Roll).Distinct().ToList();

                var listStudentEnrollment = (from stu in context.Students
                                             join sub_stu in context.Subject_Student on stu.Roll equals sub_stu.Roll
                                             join sub in context.Subjects on sub_stu.Subject_ID equals sub.Subject_ID
                                             join stu_cour_log in context.Student_Course_Log on stu.Roll equals stu_cour_log.Roll
                                             join cour in context.Courses on stu_cour_log.Course_ID equals cour.Course_ID
                                             where stu.Semester_ID == SelectSemester && sub_stu.Semester_ID == SelectSemester && sub.Subject_Active == true && stu_cour_log.Date_Import == date
                                             select stu.Roll).Distinct().ToList();
                    
                var NoEnrollment = Student.Except(CourseStudent);
                var NotRequiredCourse = NoEnrollment.Except(listStudentEnrollment);
                var infoAllStudent = (from stu in context.Students
                                   where stu.Semester_ID == SelectSemester
                                   select stu).Distinct().ToList();


                foreach (var item in NoEnrollment)
                {
                    var info = infoAllStudent.Where(m => m.Roll == item).FirstOrDefault();
                    List<string> subjectStudent =   (from stu in context.Students
                                                     join sub_stu in context.Subject_Student on stu.Roll equals sub_stu.Roll
                                                     join sub in context.Subjects on sub_stu.Subject_ID equals sub.Subject_ID
                                                     where stu.Roll == item && stu.Semester_ID == SelectSemester && sub_stu.Semester_ID == SelectSemester
                                                     select sub.Subject_Name).ToList();
                                
                     studentList.Add(new ListStudent { Email = info.Email, Roll = info.Roll, Full_Name = info.Full_Name, Campus = info.Campus_ID, ListSubject = subjectStudent, Note = "No Enrollment" });


                }
                foreach (var item in NotRequiredCourse)
                {
                    var info = infoAllStudent.Where(m => m.Roll == item).FirstOrDefault();
                    List<string> subjectStudent = (from stu in context.Students
                                                   join sub_stu in context.Subject_Student on stu.Roll equals sub_stu.Roll
                                                   join sub in context.Subjects on sub_stu.Subject_ID equals sub.Subject_ID
                                                   where stu.Roll == item && stu.Semester_ID == SelectSemester && sub_stu.Semester_ID == SelectSemester
                                                   select sub.Subject_Name).ToList();

                     studentList.Add(new ListStudent { Email = info.Email, Roll = info.Roll, Full_Name = info.Full_Name, Campus = info.Campus_ID, ListSubject = subjectStudent, Note = "Have not entered the required course" });


                }

                if (!String.IsNullOrEmpty(searchString))
                {
                    studentList = studentList.Where(a => a.Roll.Contains(searchString)).ToList();        
                }
                if (!String.IsNullOrEmpty(SelectCampus))
                {
                    studentList = studentList.Where(a => a.Campus.Contains(SelectCampus)).ToList();
                }
                if (!String.IsNullOrEmpty(SelectSubject))
                {
                    studentList = studentList.Where(a => a.ListSubject.Contains(SelectSubject)).ToList();
                }
            }

            var listCampus = (from cp in context.Campus
                              select cp).ToList();
            var listSubject = (from sub in context.Subjects
                               where sub.Subject_Active == true
                               select sub.Subject_Name).ToList();
            List<SelectListItem> selectCp = new List<SelectListItem>();
            List<SelectListItem> selectSj = new List<SelectListItem>();
            selectCp.Add(new SelectListItem
            {
                Text = "--Select Campus--",
                Value = ""
            });
            foreach (var a in listCampus)
            {
                selectCp.Add(new SelectListItem
                {
                    Text = a.Campus_Name,
                    Value = a.Campus_ID
                });
            }

            selectSj.Add(new SelectListItem
            {
                Text = "--Select Subject--",
                Value = ""
            });
            foreach (var a in listSubject)
            {
                selectSj.Add(new SelectListItem
                {
                    Text = a,
                    Value = a
                });
            }
            ViewBag.SelectSubject = selectSj;
            ViewBag.SelectCampus = selectCp;
            ls.ls1 = studentList;

            ViewBag.TotalSearch = studentList.Count();
            //rp.Students = s;
            return View("Enrollment", ls);
        }

        public ActionResult Member(InfoStudent Info, string searchString, string searchCheck, string SelectSemester, string SelectDatetime)
        {
            var context = new MSSEntities();
            List<InfoStudent> infoOfStudent = new List<InfoStudent>();
            //List<InfoStudent> distinctList = new List<InfoStudent>();

            List<SelectListItem> selectSemes = new List<SelectListItem>();
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToUpper();
            }
            var listSemes = (from a in context.Semesters
                             select a).ToList();
            var orderedListSemes = listSemes.OrderByDescending(x => x.Start_Date).ToList();
            foreach (var a in orderedListSemes)
            {
                selectSemes.Add(new SelectListItem
                {
                    Text = a.Semester_Name,
                    Value = a.Semester_ID
                });
            }
            ViewBag.SelectSemester = selectSemes;
            if (String.IsNullOrEmpty(searchCheck) && orderedListSemes.Count > 0)
            {
                SelectSemester = orderedListSemes[0].Semester_ID;
            }
            DateTime date;
            ViewBag.SelectDatetime = Date(SelectSemester);
            date = Convert.ToDateTime(Date(SelectSemester).Select(m => m.Value).FirstOrDefault());
            if (SelectDatetime != null)
            {
                date = Convert.ToDateTime(SelectDatetime);
            }

            if (!String.IsNullOrEmpty(searchCheck))
            {
                infoOfStudent = (from stu_cour_log in context.Student_Course_Log
                                 where stu_cour_log.Roll == searchString && stu_cour_log.Semester_ID == SelectSemester && stu_cour_log.Date_Import == date
                                 select new
                                 {
                                     Course_Name = stu_cour_log.Course_Name,
                                     Course_Enrollment_Time = stu_cour_log.Course_Enrollment_Time,
                                     Last_Course_Activity_Time = stu_cour_log.Last_Course_Activity_Time,
                                     Overall_Progress = stu_cour_log.Overall_Progress,
                                     Completion_Time = stu_cour_log.Completion_Time,
                                     Estimated = stu_cour_log.Estimated,
                                     Completed = stu_cour_log.Completed
                                 }).ToList().Select(p => new InfoStudent
                                 {
                                     Course_Name = p.Course_Name,
                                     Course_Enrollment_Time = (DateTime)p.Course_Enrollment_Time,
                                     Last_Course_Activity_Time = (DateTime)p.Last_Course_Activity_Time,
                                     Overall_Progress = (double)p.Overall_Progress,
                                     Completion_Time = (DateTime)p.Completion_Time,
                                     Estimated = (double)p.Estimated,
                                     Completed = (bool)p.Completed
                                 }).ToList();
            }
            var distinctList = infoOfStudent.GroupBy(x => x.Course_Name).Select(y => y.FirstOrDefault());
            ViewBag.TotalSearch = distinctList.Count();
            Info.InforList = distinctList.OrderBy(t => t.Overall_Progress).ToList();
            return View("Member", Info);
        }

        public ActionResult NotRequiredCourse(ListNotRequiredCourse lc, string SelectString, string searchCheck, string SelectSemester, string SelectDatetime)
        {
            var context = new MSSEntities();
            List<ListNotRequiredCourse> listNotRequiredCourses = new List<ListNotRequiredCourse>();

            List<SelectListItem> selectSemes = new List<SelectListItem>();

            var listSemes = (from a in context.Semesters
                             select a).ToList();
            var orderedListSemes = listSemes.OrderByDescending(x => x.Start_Date).ToList();
            foreach (var a in orderedListSemes)
            {
                selectSemes.Add(new SelectListItem
                {
                    Text = a.Semester_Name,
                    Value = a.Semester_ID
                });
            }
            ViewBag.SelectSemester = selectSemes;

            if (String.IsNullOrEmpty(searchCheck) && orderedListSemes.Count > 0)
            {
                SelectSemester = orderedListSemes[0].Semester_ID;
            }

            DateTime date;
            ViewBag.SelectDatetime = Date(SelectSemester);
            date = Convert.ToDateTime(Date(SelectSemester).Select(m => m.Value).FirstOrDefault());
            if (SelectDatetime != null)
            {
                date = Convert.ToDateTime(SelectDatetime);
            }

            if (!String.IsNullOrEmpty(searchCheck))
            {
                var NotRequired = (from stu_cour_log in context.Student_Course_Log
                                   where stu_cour_log.Course_ID == null && stu_cour_log.Semester_ID == SelectSemester && stu_cour_log.Date_Import == date
                                   select stu_cour_log.Course_Name).Distinct().ToList();
                foreach (var item in NotRequired)
                {
                    var completed = (from stu_cour_log in context.Student_Course_Log
                                     where stu_cour_log.Course_Name == item && stu_cour_log.Completed == true && stu_cour_log.Course_ID == null && stu_cour_log.Semester_ID == SelectSemester && stu_cour_log.Date_Import == date
                                     select stu_cour_log.Roll).Distinct().Count();
                    var notCompleted = (from stu_cour_log in context.Student_Course_Log
                                        where stu_cour_log.Course_Name == item && stu_cour_log.Completed == false && stu_cour_log.Course_ID == null && stu_cour_log.Semester_ID == SelectSemester && stu_cour_log.Date_Import == date
                                        select stu_cour_log.Roll).Distinct().Count();
                    listNotRequiredCourses.Add(new ListNotRequiredCourse { Name = item, Complelted = completed, NotComplelted = notCompleted });
                }

                if (!String.IsNullOrEmpty(SelectString))
                {
                    if(SelectString == "1")
                    {
                        listNotRequiredCourses = listNotRequiredCourses.Where(a => (a.Complelted > 0)).OrderByDescending(a => a.Complelted).ToList();
                    }
                    if (SelectString == "2")
                    {
                        listNotRequiredCourses = listNotRequiredCourses.Where(a => (a.Complelted == 0)).OrderByDescending(a => a.NotComplelted).ToList();
                    }

                }
            }

            List<SelectListItem> selectCompleted = new List<SelectListItem>();
            selectCompleted.Add(new SelectListItem { Text = "--Select Completed--", Value = ""});
            selectCompleted.Add(new SelectListItem { Text = "Completed", Value = "1" });
            selectCompleted.Add(new SelectListItem { Text = "Not Completed", Value = "2" });

            ViewBag.SelectString = selectCompleted;
            lc.lc1 = listNotRequiredCourses.OrderByDescending(a => a.Complelted).ToList();

            ViewBag.TotalSearch = listNotRequiredCourses.Count();
            //rp.Students = s;
            return View("NotRequiredCourse", lc);
        }

        public ActionResult CertificateReport(CertificateViewModel cv, string searchCheck,string SearchString, string SelectString, string SelectSemester)
        {
            var context = new MSSEntities();
            var rollList = (from certi in context.Certificates
                            where certi.Semester_ID == SelectSemester
                            select certi.Roll).Distinct().ToList();
            List<CertificateViewModel> certificate = new List<CertificateViewModel>();

            List<SelectListItem> selectSemes = new List<SelectListItem>();
            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.Trim().ToUpper();
            }
            var listSemes = (from a in context.Semesters
                             select a).ToList();
            var orderedListSemes = listSemes.OrderByDescending(x => x.Start_Date).ToList();
            foreach (var a in orderedListSemes)
            {
                selectSemes.Add(new SelectListItem
                {
                    Text = a.Semester_Name,
                    Value = a.Semester_ID
                });
            }
            ViewBag.SelectSemester = selectSemes;

            if (!String.IsNullOrEmpty(searchCheck))
            {
                 certificate = (from certi in context.Certificates
                                where certi.Semester_ID == SelectSemester
                                   select new
                                   {
                                       Link = certi.Link,
                                       Date_Submit = certi.Date_Submit,
                                       Roll = certi.Roll,
                                       Course_ID = certi.Course_ID,
                                       Specification_ID = certi.Specification_ID
                                   }).ToList().Select(p => new CertificateViewModel
                                   {
                                       Link = p.Link,
                                       Date_Submit = p.Date_Submit,
                                       Roll = p.Roll,
                                       CourseId = (int)p.Course_ID,
                                       SpecID = (int)p.Specification_ID,
                                       CourseName = (from b in context.Courses
                                                     where b.Course_ID == p.Course_ID
                                                     select b.Course_Name).FirstOrDefault(),
                                       SubjectName = (from b in context.Specifications
                                                      join c in context.Subjects on b.Subject_ID equals c.Subject_ID
                                                      where b.Specification_ID == p.Specification_ID
                                                      select c.Subject_Name).FirstOrDefault()
                                   }).ToList();
                if (!String.IsNullOrEmpty(SearchString))
                {
                    certificate = certificate.Where(a => a.Roll.Contains(SearchString)).ToList();
                }
                if (!String.IsNullOrEmpty(SelectString))
                {
                    certificate = certificate.Where(a => a.SubjectName.Contains(SelectString)).ToList();
                }
            }
               
            var listSubject = (from sub in context.Subjects
                               where sub.Subject_Active == true
                               select sub.Subject_Name).ToList();
            List<SelectListItem> selectSj = new List<SelectListItem>();
            selectSj.Add(new SelectListItem
            {
                Text = "--Select Subject--",
                Value = ""
            });
            foreach (var a in listSubject)
            {
                selectSj.Add(new SelectListItem
                {
                    Text = a,
                    Value = a
                });
            }
            ViewBag.SelectString = selectSj;
            ViewBag.TotalSearch = certificate.Count();

            cv.certificatesModel = certificate;



            return View("CertificateReport", cv);
        }
        
        public ActionResult PrintViewToPdf(string SelectDatetime, string searchCheck, string weekNumber, string SelectSemester)
        {
            var rpN = new Report();
            var report = new ActionAsPdf("Index", new { rpN, weekNumber, searchCheck, SelectDatetime, SelectSemester });
            return report;
        }

        public ActionResult SpecCompleted(ListStudent listS, string SelectSemester, string searchCheck, string SearchString, string SelectDatetime, string SelectCampus, string Compulsory, string SelectSubject)
        {
            var context = new MSSEntities();
            List<SelectListItem> selectSemes = new List<SelectListItem>();

            var listSemes = (from a in context.Semesters
                             select a).ToList();
            var orderedListSemes = listSemes.OrderByDescending(x => x.Start_Date).ToList();
            foreach (var a in orderedListSemes)
            {
                selectSemes.Add(new SelectListItem
                {
                    Text = a.Semester_Name,
                    Value = a.Semester_ID
                });
            }
            ViewBag.SelectSemester = selectSemes;


            var listCampus = (from a in context.Campus
                              select a).ToList();
            List<SelectListItem> selectCp = new List<SelectListItem>();
            List<SelectListItem> selectCompulsory = new List<SelectListItem>();
            selectCompulsory.Add(new SelectListItem { Text = "--All--", Value = "" });
            selectCompulsory.Add(new SelectListItem { Text = "Yes", Value = "Yes" });
            selectCompulsory.Add(new SelectListItem { Text = "No", Value = "No" });
            ViewBag.Compulsory = selectCompulsory;

            selectCp.Add(new SelectListItem
            {
                Text = "--Select Campus--",
                Value = ""
            });
            foreach (var a in listCampus)
            {
                selectCp.Add(new SelectListItem
                {
                    Text = a.Campus_Name,
                    Value = a.Campus_ID
                });
            }
            ViewBag.SelectCampus = selectCp;
            ViewBag.SelectSubject = Sub();

            if (String.IsNullOrEmpty(searchCheck) && orderedListSemes.Count > 0)
            {
                SelectSemester = orderedListSemes[0].Semester_ID;
            }
            DateTime date;
            ViewBag.SelectDatetime = Date(SelectSemester);
            date = Convert.ToDateTime(Date(SelectSemester).Select(m => m.Value).FirstOrDefault());
            if (SelectDatetime != null)
            {
                date = Convert.ToDateTime(SelectDatetime);
            }
            List<ListStudent> ListStudentCompleted = new List<ListStudent>();
            List<Student_Specification_Log> specCompletedListSpec = new List<Student_Specification_Log>();
            List<string> studentList = new List<string>();
            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.Trim().ToUpper();
            }
            if (!String.IsNullOrEmpty(searchCheck))
            {
                var infoStudent = (from stu in context.Students
                                   where stu.Semester_ID == SelectSemester
                                   select stu).ToList();
                var infoSub = (from sub in context.Subjects
                               where sub.Subject_Active == true
                               select sub).ToList();
                if(Compulsory == "")
                {
                    var listSpecCompulsoryCompleted = context.sp_Get_Compulsory_Spec_Completion(date, SelectSemester);
                    var listSpecNonCompulsoryCompleted = context.sp_Get_Non_Compulsory_Spec_Completion(date, SelectSemester);
                    if(listSpecCompulsoryCompleted!=null && listSpecNonCompulsoryCompleted!= null)
                    {
                        var listAll = listSpecCompulsoryCompleted.Concat(listSpecNonCompulsoryCompleted.Select(m => m.Roll_Sub)).Distinct();
                        foreach (var item in listAll)
                        {
                            var roll = item.Split('-')[0];
                            var sub = item.Split('-')[1];
                            var info = infoStudent.Where(m => m.Roll == roll).FirstOrDefault();
                            var subName = infoSub.Where(m => m.Subject_ID == sub).FirstOrDefault();
                            ListStudentCompleted.Add(new ListStudent { Roll = roll, Campus = info.Campus_ID, Subject = subName.Subject_Name, Semester_ID = SelectSemester, Email = info.Email, Subject_ID = sub });
                        }
                    }                    
                }
                else if(Compulsory == "Yes")
                {
                    var listSpecCompulsoryCompleted = context.sp_Get_Compulsory_Spec_Completion(date, SelectSemester);
                    foreach(var item in listSpecCompulsoryCompleted)
                    {
                        var roll = item.Split('-')[0];
                        var sub = item.Split('-')[1];
                        var info = infoStudent.Where(m => m.Roll == roll).FirstOrDefault();
                        var subName = infoSub.Where(m => m.Subject_ID == sub).FirstOrDefault();
                        ListStudentCompleted.Add(new ListStudent { Roll = roll, Campus = info.Campus_ID, Subject = subName.Subject_Name, Semester_ID = SelectSemester, Email = info.Email, Subject_ID = sub });
                    }
                }else if(Compulsory == "No")
                {
                    var listSpecNonCompulsoryCompleted = context.sp_Get_Non_Compulsory_Spec_Completion(date, SelectSemester);
                    foreach (var item in listSpecNonCompulsoryCompleted)
                    {
                        var roll = item.Roll_Sub.Split('-')[0];
                        var sub = item.Roll_Sub.Split('-')[1];
                        var info = infoStudent.Where(m => m.Roll == roll).FirstOrDefault();
                        var subName = infoSub.Where(m => m.Subject_ID == sub).FirstOrDefault();
                        ListStudentCompleted.Add(new ListStudent { Roll = roll, Campus = info.Campus_ID, Subject = subName.Subject_Name, Semester_ID = SelectSemester, Email = info.Email, Subject_ID = sub });
                    }
                }

                if (!String.IsNullOrEmpty(SelectCampus))
                {
                    ListStudentCompleted = ListStudentCompleted.Where(a => a.Campus.Contains(SelectCampus)).ToList();
                }
                if (!String.IsNullOrEmpty(SelectSubject))
                {
                    ListStudentCompleted = ListStudentCompleted.Where(a => a.Subject_ID.Contains(SelectSubject)).ToList();
                }
                if (!String.IsNullOrEmpty(SearchString))
                {
                    ListStudentCompleted = ListStudentCompleted.Where(a => a.Roll.Contains(SearchString)).ToList();
                    ViewBag.TotalSearch = ListStudentCompleted.Count();
                    ViewBag.Spec = (from a in context.Students
                                    join b in context.Subject_Student on a.Roll equals b.Roll
                                    join c in context.Subjects on b.Subject_ID equals c.Subject_ID
                                    where a.Semester_ID == SelectSemester && b.Semester_ID == SelectSemester && a.Roll == SearchString
                                    select c.Subject_ID).Count();
                    if (ViewBag.TotalSearch > 0 && ViewBag.Spec > 0)
                    {
                        ViewBag.Percent = percent(ViewBag.TotalSearch, ViewBag.Spec);
                    }
                }
                else if (String.IsNullOrEmpty(SearchString))
                {
                    ViewBag.TotalSearchForSearchNull = ListStudentCompleted.Count();
                }
            }
            
            listS.ls1 = ListStudentCompleted;

            return View("SpecCompleted", listS);
        }

        public ActionResult Bonus(ListStudent listS, string SelectSemester, string searchCheck, string SearchString, string SelectDatetime, string Display, string SelectSubject)
        {
            var context = new MSSEntities();
            List<ListStudent> bonusListStudent = new List<ListStudent>();
            List<ListStudent> ListCompleted = new List<ListStudent>();
            List<SelectListItem> selectSemes = new List<SelectListItem>();

            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.Trim().ToUpper();
            }
            var listSemes = (from a in context.Semesters
                             select a).ToList();
            var orderedListSemes = listSemes.OrderByDescending(x => x.Start_Date).ToList();
            foreach (var a in orderedListSemes)
            {
                selectSemes.Add(new SelectListItem
                {
                    Text = a.Semester_Name,
                    Value = a.Semester_ID
                });
            }
            ViewBag.SelectSemester = selectSemes;
            if (String.IsNullOrEmpty(searchCheck) && orderedListSemes.Count > 0)
            {
                SelectSemester = orderedListSemes[0].Semester_ID;
            }
            ViewBag.SelectSubject = Sub();
            DateTime date;
            ViewBag.SelectDatetime = Date(SelectSemester);
            date = Convert.ToDateTime(Date(SelectSemester).Select(m => m.Value).FirstOrDefault());
            if(SelectDatetime!= null)
            {
                date = Convert.ToDateTime(SelectDatetime);
            }


            if (!String.IsNullOrEmpty(searchCheck) && date != DateTime.MinValue)
            {
                 var listCourseCompleted = (from stu in context.Students
                                               join sub_stu in context.Subject_Student on stu.Roll equals sub_stu.Roll
                                               join sub in context.Subjects on sub_stu.Subject_ID equals sub.Subject_ID
                                               join stu_cour_log in context.Student_Course_Log on stu.Roll equals stu_cour_log.Roll
                                               join cour in context.Courses on stu_cour_log.Course_ID equals cour.Course_ID
                                               join cour_dead in context.Course_Deadline on cour.Course_ID equals cour_dead.Course_ID
                                               where stu.Semester_ID == SelectSemester && sub_stu.Semester_ID == SelectSemester && stu_cour_log.Completed == true && stu_cour_log.Date_Import == date && stu_cour_log.Course_ID != null && cour_dead.Semester_ID == SelectSemester && stu_cour_log.Completion_Time <= cour_dead.Deadline && sub.Subject_Active == true
                                               select stu_cour_log).ToList();

                if (Display == "Display Student Bonus")
                {
                    var listStudent = (from stu in context.Students
                                       join sub_stu in context.Subject_Student on stu.Roll equals sub_stu.Roll
                                       join sub in context.Subjects on sub_stu.Subject_ID equals sub.Subject_ID
                                       join stu_cour_log in context.Student_Course_Log on stu.Roll equals stu_cour_log.Roll
                                       join cour in context.Courses on stu_cour_log.Course_ID equals cour.Course_ID
                                       join cour_dead in context.Course_Deadline on cour.Course_ID equals cour_dead.Course_ID
                                       where stu.Semester_ID == SelectSemester && sub_stu.Semester_ID == SelectSemester && stu_cour_log.Completed == true && stu_cour_log.Date_Import == date && stu_cour_log.Course_ID != null && cour_dead.Semester_ID == SelectSemester && stu_cour_log.Completion_Time <= cour_dead.Deadline && sub.Subject_Active == true
                                       select new { sub_stu, stu, sub, cour }).DistinctBy(m => m.sub_stu).ToList();
                    foreach (var t in listStudent)
                    {
                        var countBonus = listCourseCompleted.Where(m => m.Roll == t.sub_stu.Roll && m.Subject_ID == t.sub_stu.Subject_ID).Count();
                        double bonus = 0;
                        bonus = countBonus * 0.25;
                        if (bonus > 1)
                        {
                            bonus = 1;
                        }
                        if (bonus > 0)
                        {
                            bonusListStudent.Add(new ListStudent { Roll = t.sub_stu.Roll, Campus = t.stu.Campus_ID, PBonus = bonus, Subject = t.sub.Subject_Name,Subject_ID = t.sub.Subject_ID });
                        }
                    }
                }
                if (Display == "Display Student-Course Bonus")
                {
                    var listStudent = (from stu in context.Students
                                       join sub_stu in context.Subject_Student on stu.Roll equals sub_stu.Roll
                                       join sub in context.Subjects on sub_stu.Subject_ID equals sub.Subject_ID
                                       join stu_cour_log in context.Student_Course_Log on stu.Roll equals stu_cour_log.Roll
                                       join cour in context.Courses on stu_cour_log.Course_ID equals cour.Course_ID
                                       join cour_dead in context.Course_Deadline on cour.Course_ID equals cour_dead.Course_ID
                                       where stu.Semester_ID == SelectSemester && sub_stu.Semester_ID == SelectSemester && stu_cour_log.Completed == true && stu_cour_log.Date_Import == date && stu_cour_log.Course_ID != null && cour_dead.Semester_ID == SelectSemester && stu_cour_log.Completion_Time <= cour_dead.Deadline && sub.Subject_Active == true
                                       select new { sub_stu, stu, sub, cour }).ToList();
                    foreach (var t in listStudent)
                    {
                        double bonus = 0.25;
                        bonusListStudent.Add(new ListStudent { Roll = t.sub_stu.Roll, Campus = t.stu.Campus_ID, PBonus = bonus, Subject = t.sub.Subject_Name, Course_Name = t.cour.Course_Name, Subject_ID = t.sub.Subject_ID});

                    }
                    bonusListStudent = bonusListStudent.OrderBy(m => m.Roll).ToList();
                }

                if (!String.IsNullOrEmpty(SearchString))
                {
                    bonusListStudent = bonusListStudent.Where(a => a.Roll.Contains(SearchString)).ToList();
                }
                if (!String.IsNullOrEmpty(SelectSubject))
                {
                    bonusListStudent = bonusListStudent.Where(a => a.Subject_ID.Contains(SelectSubject)).ToList();
                }
            }
                
            ViewBag.TotalSearch = bonusListStudent.Count();
            listS.ls1 = bonusListStudent;
            return View("Bonus", listS);
        }

        public ActionResult Estimated(EstimatedViewModel EstimatedVM,string SelectSemester, string searchCheck, string SearchString, string SelectDatetime)
        {
            var context = new MSSEntities();
            List<SelectListItem> selectSemes = new List<SelectListItem>();

            if (!String.IsNullOrEmpty(SearchString))
            {
                SearchString = SearchString.Trim().ToUpper();
            }
            var listSemes = (from a in context.Semesters
                             select a).ToList();
            var orderedListSemes = listSemes.OrderByDescending(x => x.Start_Date).ToList();
            foreach (var a in orderedListSemes)
            {
                selectSemes.Add(new SelectListItem
                {
                    Text = a.Semester_Name,
                    Value = a.Semester_ID
                });
            }
            ViewBag.SelectSemester = selectSemes;

            List<EstimatedViewModel> EstimatedList = new List<EstimatedViewModel>();
            if (String.IsNullOrEmpty(searchCheck) && orderedListSemes.Count > 0)
            {
                SelectSemester = orderedListSemes[0].Semester_ID;
            }
            ViewBag.SelectSubject = Sub();
            DateTime date;
            ViewBag.SelectDatetime = Date(SelectSemester);
            date = Convert.ToDateTime(Date(SelectSemester).Select(m => m.Value).FirstOrDefault());
            if (SelectDatetime != null)
            {
                date = Convert.ToDateTime(SelectDatetime);
            }
            if (!String.IsNullOrEmpty(searchCheck))
            {
                var studentLog = (from stu_cour_log in context.Student_Course_Log
                                  where stu_cour_log.Date_Import == date && stu_cour_log.Semester_ID == SelectSemester
                                  select stu_cour_log).ToList();
                var rollList = (from stu_cour_log in context.Student_Course_Log
                                where stu_cour_log.Date_Import == date && stu_cour_log.Semester_ID == SelectSemester
                                select stu_cour_log.Roll).Distinct().ToList();


                foreach (var t in rollList)
                {
                    var info = studentLog.Where(m => m.Roll == t).FirstOrDefault();
                    var totalEstimated = studentLog.Where(m => m.Roll == t).Sum(m => m.Estimated);
                    var compulsory = studentLog.Where(m => m.Roll == t && m.Course_ID != null).Sum(m => m.Estimated);
                    var nonCompulsory = studentLog.Where(m => m.Roll == t && m.Course_ID == null).Sum(m => m.Estimated);
                    EstimatedList.Add(new EstimatedViewModel { Roll = t, Campus = info.Campus, Email = info.Email, TotalEstimated = (double)totalEstimated, Compulsory = (double)compulsory, NonCompulsory = (double)nonCompulsory });
                }
                EstimatedList = EstimatedList.OrderByDescending(m => m.Compulsory).ToList();
                if (!String.IsNullOrEmpty(SearchString))
                {
                    EstimatedList = EstimatedList.Where(a => a.Roll.Contains(SearchString)).ToList();
                }
            }
            
            ViewBag.TotalSearch = EstimatedList.Count();
            EstimatedVM.EstimatedModel = EstimatedList;
            return View("Estimated", EstimatedVM);
        }

        private int Count(string subject, string campus, DateTime dateImport)
        {
            //double per;
            //DateTime date = Convert.ToDateTime(dateImport);
            int count = 0;
            var context = new MSSEntities();
            if (subject != "" && campus == "")
            {
                count = (from stu_cour_log in context.Student_Course_Log
                         join cour in context.Courses on stu_cour_log.Course_ID equals cour.Course_ID
                         join spec in context.Specifications on cour.Specification_ID equals spec.Specification_ID
                         join sub in context.Subjects on spec.Subject_ID equals sub.Subject_ID
                         where subject == stu_cour_log.Subject_ID && stu_cour_log.Date_Import == dateImport
                         select stu_cour_log.Roll).Distinct().Count();
            }
            else if (subject == "" && campus == "")
            {
                count = (from stu_cour_log in context.Student_Course_Log
                         join cour in context.Courses on stu_cour_log.Course_ID equals cour.Course_ID
                         join spec in context.Specifications on cour.Specification_ID equals spec.Specification_ID
                         join sub in context.Subjects on spec.Subject_ID equals sub.Subject_ID
                         where stu_cour_log.Date_Import == dateImport
                         select stu_cour_log.Roll).Distinct().Count();
            }
            else if(subject != "" && campus != "")
            {
                count = (from stu_cour_log in context.Student_Course_Log
                         join cour in context.Courses on stu_cour_log.Course_ID equals cour.Course_ID
                         join spec in context.Specifications on cour.Specification_ID equals spec.Specification_ID
                         join sub in context.Subjects on spec.Subject_ID equals sub.Subject_ID
                         where stu_cour_log.Campus == campus && stu_cour_log.Subject_ID == subject && stu_cour_log.Date_Import == dateImport
                         select stu_cour_log.Roll).Distinct().Count();
            }
            else if(subject == "" && campus != "")
            {
                count = (from stu_cour_log in context.Student_Course_Log
                         join cour in context.Courses on stu_cour_log.Course_ID equals cour.Course_ID
                         join spec in context.Specifications on cour.Specification_ID equals spec.Specification_ID
                         join sub in context.Subjects on spec.Subject_ID equals sub.Subject_ID
                         where stu_cour_log.Campus == campus && stu_cour_log.Date_Import == dateImport
                         select stu_cour_log.Roll).Distinct().Count();
            }
            return count;
        }

        private double percent(int a, int b)
        {
            double per;
            if (b > 0)
            {
                per = Math.Round(((double)a / (double)b) * 100);
            }
            else
            {
                per = 0;
            }
            return per;
        }

        private int Campus(string subjectId, string campus, string semester)
        {
            var context = new MSSEntities();
            var student = 0;
            if (subjectId == "" && campus != "")
            {
                student = (from sub in context.Subjects
                               join sub_stu in context.Subject_Student on sub.Subject_ID equals sub_stu.Subject_ID
                               join stu in context.Students on sub_stu.Roll equals stu.Roll
                               where stu.Campus_ID == campus && stu.Semester_ID == semester && sub.Subject_Active == true && sub_stu.Semester_ID == semester
                               select stu.Roll).Count();
            }
            else if(subjectId == "" && campus == "")
            {
                student = (from sub in context.Subjects
                           join sub_stu in context.Subject_Student on sub.Subject_ID equals sub_stu.Subject_ID
                           join stu in context.Students on sub_stu.Roll equals stu.Roll
                           where stu.Semester_ID == semester && sub.Subject_Active == true && sub_stu.Semester_ID == semester
                           select stu.Roll).Count();
            }
            else
            {
                student = (from sub in context.Subjects
                               join sub_stu in context.Subject_Student on sub.Subject_ID equals sub_stu.Subject_ID
                               join stu in context.Students on sub_stu.Roll equals stu.Roll
                               where subjectId == sub.Subject_ID && stu.Campus_ID == campus && stu.Semester_ID == semester && sub.Subject_Active == true && sub_stu.Semester_ID == semester
                           select stu.Roll).Count();
            }
            
            return student;
        }
        private List<SelectListItem> Date(string SelectSemester)
        {
            var context = new MSSEntities();
            List<SelectListItem> selectDate = new List<SelectListItem>();
            var starDate = (from semes in context.Semesters
                            where semes.Semester_ID == SelectSemester
                            select semes.Start_Date).FirstOrDefault();
            var endDate = (from semes in context.Semesters
                           where semes.Semester_ID == SelectSemester
                           select semes.End_Date).FirstOrDefault();


            var listDate = (from stu_cour_log in context.Student_Course_Log
                            where (stu_cour_log.Date_Import <= endDate && stu_cour_log.Date_Import >= starDate)
                            select stu_cour_log.Date_Import).Distinct().ToList();

            var orderedListDate = listDate.OrderByDescending(x => x).ToList();
            foreach (var a in orderedListDate)
            {
                selectDate.Add(new SelectListItem
                {
                    Text = Convert.ToDateTime(a).ToString("dd/MM/yyyy"),
                    Value = a.ToString()
                });
            }
            return selectDate;
        }

        private List<SelectListItem> Sub()
        {
            var context = new MSSEntities();
            var listSubject = (from sub in context.Subjects
                               where sub.Subject_Active == true
                               select sub).ToList();
            List<SelectListItem> selectSj = new List<SelectListItem>();
            selectSj.Add(new SelectListItem
            {
                Text = "--Select Subject--",
                Value = ""
            });
            foreach (var a in listSubject)
            {
                selectSj.Add(new SelectListItem
                {
                    Text = a.Subject_Name,
                    Value = a.Subject_ID
                });
            }
            return selectSj;
        }
    }
}
