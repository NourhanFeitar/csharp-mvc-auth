using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MVC_Lab_7.Models
{
    public class Department
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeptId { get; set; }
        [Required, StringLength(15, MinimumLength = 3)]
        public string DeptName { get; set; }
        public int Capacity { get; set; }

        public Department()
        {
            Students = new HashSet<Student>();
        }
        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
    }
}
