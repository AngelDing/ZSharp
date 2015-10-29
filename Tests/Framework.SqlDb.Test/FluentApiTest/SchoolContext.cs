using System.Data.Entity; 
using System.Data.Entity.ModelConfiguration.Conventions; 
// add a reference to System.ComponentModel.DataAnnotations DLL 
using System.ComponentModel.DataAnnotations; 
using System.Collections.Generic; 
using System;

namespace Framework.SqlDb.Test
{
    public class SchoolContext : DbContext
    {
        public SchoolContext()
            : this(ConstHelper.SchoolDB)
        { 
        }
        public SchoolContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>(); //全局關閉級聯善刪除
            //modelBuilder.Entity<Course>()
            //    .Map<Course>(m => m.Requires("Type").HasValue<byte>(1))
            //    .Map<OnsiteCourse>(m => m.Requires("Type").HasValue<byte>(2))
            //    .Map<OnlineCourse>(m => m.Requires("Type").HasValue<byte>(3));
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<OnsiteCourse>().ToTable("OnsiteCourse");
            modelBuilder.Entity<OnlineCourse>().ToTable("OnlineCourse");
        }
    }

    public class Department
    {
        public Department()
        {
            this.Courses = new HashSet<Course>();
        }
        // Primary key 
        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public System.DateTime StartDate { get; set; }
        public int? Administrator { get; set; }

        // Navigation property 
        public virtual ICollection<Course> Courses { get; private set; }
    }

    public class Course
    {
        public Course()
        {
            this.Instructors = new HashSet<Instructor>();
        }
        // Primary key 
        public int CourseID { get; set; }

        public string Title { get; set; }
        public int Credits { get; set; }

        // Foreign key 
        public int DepartmentID { get; set; }

        // Navigation properties 
        public virtual Department Department { get; set; }
        public virtual ICollection<Instructor> Instructors { get; private set; }
    }

    public partial class OnlineCourse : Course
    {
        public string URL { get; set; }
    }

    public partial class OnsiteCourse : Course
    {
        public OnsiteCourse()
        {
            Details = new Details();
        }

        public Details Details { get; set; }
    }

    public class Details
    {
        public System.DateTime Time { get; set; }
        public string Location { get; set; }
        public string Days { get; set; }
    }

    public class Instructor
    {
        public Instructor()
        {
            this.Courses = new List<Course>();
        }

        // Primary key 
        public int InstructorID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public System.DateTime HireDate { get; set; }

        // Navigation properties 
        public virtual ICollection<Course> Courses { get; private set; }
    }

    public class OfficeAssignment
    {
        // Specifying InstructorID as a primary 
        [Key()]
        public Int32 InstructorID { get; set; }

        public string Location { get; set; }

        // When the Entity Framework sees Timestamp attribute 
        // it configures ConcurrencyCheck and DatabaseGeneratedPattern=Computed. 
        [Timestamp]
        public Byte[] Timestamp { get; set; }

        // Navigation property 
        public virtual Instructor Instructor { get; set; }
    }
}