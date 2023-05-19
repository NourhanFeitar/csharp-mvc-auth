using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MVC_Lab_7.Models
{
    public class Student
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string ImgName { get; set; }

        [ForeignKey("Department")]
        public int? DeptNo { get; set; }

        public virtual Department? Department { get; set; }


        public virtual ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();
    }
}
