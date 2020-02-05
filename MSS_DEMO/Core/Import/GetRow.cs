﻿using MSS_DEMO.Core.Interface;
using MSS_DEMO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSS_DEMO.Core.Import
{
    public class GetRow : IGetRow
    {
        public Student GetStudent(String[] row)
        {
            return new Student
            {
                Email = row[1].ToString(),
                Roll = row[0].ToString(),
            };

        }
        public Class_Student GetClassStudent(String[] row)
        {
            return new Class_Student
            {
                Roll = row[1].ToString(),
                Class_ID = row[2].ToString(),
            };

        }
        public Subject_Student GetSubjectStudent(String[] row)
        {
            return new Subject_Student
            {
                Roll = row[0].ToString(),
                Subject_ID = row[2].ToString().Split('-')[0],
            };

        }
        public Student_Specification_Log GetStudentSpec(String[] row)
        {
            return new Student_Specification_Log
            {
                //Roll = row[2].ToString().Split('-')[2],
                Roll = row[0].ToString(),
                Subject_ID = row[2].ToString().Split('-')[0],
                Campus = row[2].ToString().Split('-')[1],
                Specialization = row[3].ToString(),
                Specialization_Slug = row[4].ToString(),
                University = row[5].ToString(),
                Specialization_Enrollment_Time = row[6].ToString() != "" ? DateTime.Parse(row[6].ToString()) : DateTime.Parse("01/01/1970"),
                Last_Specialization_Activity_Time = row[7].ToString() != "" ? DateTime.Parse(row[7].ToString()) : DateTime.Parse("01/01/1970"),
                Completed = bool.Parse(ChangeBoolean(row[8].ToString())),
                Status = bool.Parse(ChangeBoolean(row[9].ToString())),
                Program_Slug = row[10].ToString(),
                Program_Name = row[11].ToString(),
                Specialization_Completion_Time = row[13].ToString() != "" ? DateTime.Parse(row[13].ToString()) : DateTime.Parse("01/01/1970"),

            };
        }
        public Student_Course_Log GetStudentCourse(String[] row)
        {

            Student_Course_Log log1 = new Student_Course_Log
            {
                // Roll = row[2].ToString().Split('-')[2],
                Roll = row[0].ToString(),
                Course_Enrollment_Time = row[7].ToString() != "" ? DateTime.Parse(row[7].ToString()) : DateTime.Parse("01/01/1970"),
                Course_Start_Time = row[8].ToString() != "" ? DateTime.Parse(row[8].ToString()) : DateTime.Parse("01/01/1970"),
                Last_Course_Activity_Time = row[9].ToString() != "" ? DateTime.Parse(row[9].ToString()) : DateTime.Parse("01/01/1970"),
                Overall_Progress = Double.Parse(row[10].ToString()),
                Estimated = Double.Parse(row[11].ToString()),
                Completed = Boolean.Parse(ChangeBoolean(row[12].ToString())),
                Status = Boolean.Parse(ChangeBoolean(row[13].ToString())),
                Program_Slug = row[14].ToString(),
                Program_Name = row[15].ToString(),
                Completion_Time = row[17].ToString() != "" ? DateTime.Parse(row[17].ToString()) : DateTime.Parse("01/01/1970"),
                Course_Grade = Double.Parse(row[18].ToString()),
            };
            return log1;
        }
        public String ChangeBoolean(string name)
        {
            if (name.ToLower() == "yes") return "True";
            else return "False";
        }

    }
}