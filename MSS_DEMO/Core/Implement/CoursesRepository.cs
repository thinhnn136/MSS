﻿using MSS_DEMO.Models;
using MSS_DEMO.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSS_DEMO.Core.Components
{
    public class CoursesRepository : BaseRepository<Course>
    {
        public CoursesRepository(MSSEntities context)
           : base(context)
        {
        }
        public List<Cours_Spec> GetPageList()
        {
            List<Cours_Spec> cour = new List<Cours_Spec>();
            using (MSSEntities db = new MSSEntities())
            {
                cour = (from cou in db.Courses
                        join dl in db.Specifications on cou.Specification_ID equals dl.Specification_ID into empSpec
                        from ed in empSpec.DefaultIfEmpty()
                        select new Cours_Spec { Course_ID = cou.Course_ID, Course_Name = cou.Course_Name, Specification_ID = cou.Specification_ID, Specification_Name = ed.Specification_Name == null ? "Not Map" : ed.Specification_Name }).ToList();
                cour = (from o in cour
                        orderby o.Course_ID descending
                        select o)
                          .ToList();
                return cour;
            }

        }

        public Cours_Spec GetListByID(int id)
        {
            return GetPageList().Where(s => s.Course_ID == id).FirstOrDefault();
        }

        public List<Course_Spec_Sub> GetListID()
        {
            List<Course_Spec_Sub> cour = new List<Course_Spec_Sub>();
            using (MSSEntities db = new MSSEntities())
            {
                cour = (from cou in db.Courses
                       join sp in db.Specifications on cou.Specification_ID equals sp.Specification_ID
                       join su in db.Subjects on sp.Subject_ID equals su.Subject_ID
                       select new Course_Spec_Sub { Course_ID = cou.Course_ID, Subject_ID = su.Subject_ID }).ToList();
                return cour;
            }
        }
    }
    public class Course_Spec_Sub : Course
    {
        public string Subject_ID { get; set; }
    }
    public class Cours_Spec : Course
    {
        public string Specification_Name { get; set; }

    }
}