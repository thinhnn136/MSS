﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MSS_DEMO.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MSSEntities : DbContext
    {
        public MSSEntities()
            : base("name=MSSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Campu> Campus { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Class_Student> Class_Student { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Mentor> Mentors { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Semester> Semesters { get; set; }
        public virtual DbSet<Specification> Specifications { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Student_Course_Log> Student_Course_Log { get; set; }
        public virtual DbSet<Student_Specification_Log> Student_Specification_Log { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }
        public virtual DbSet<Subject_Student> Subject_Student { get; set; }
        public virtual DbSet<User_Role> User_Role { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
