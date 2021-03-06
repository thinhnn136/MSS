﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSS_DEMO.Models
{
    public class Report
    {
        public string Sub { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Total { get; set; }
        public double Study { get; set; }

        public List<double> Cmp { get; set; }
        public List<Report> rp1 { get; set; }
        public List<Report> rp2 { get; set; }
    }

    public class ListStudent
    {
        public int STT { get; set; }
        public string Email { get; set; }
        public string Roll { get; set; }
        public string Full_Name { get; set; }
        public string Campus { get; set; }
        public string Subject { get; set; }
        public string Semester_ID { get; set; }
        public List<string> ListSubject { get; set; }
        public virtual Campu Campuss { get; set; }
        public List<ListStudent> ls1 { get; set; }
    }
}