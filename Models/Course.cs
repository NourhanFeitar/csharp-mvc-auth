using System.Collections.Generic;

namespace MVC_Lab_7.Models
{
    public class Course
    {
        public int Crs_Id { get; set; }
        public string Crs_Name { get; set; }
        public int Lect_Hours { get; set; }
        public int Lab_Hours { get; set; }

        public virtual ICollection<Department> Departments { get; set; } = new HashSet<Department>();
        public virtual ICollection<StudentCourse> courseStudents { get; set; } = new HashSet<StudentCourse>();
    }
}
