using System.Collections.Generic;

namespace MVC_Lab_7.Models
{
    public class Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public ICollection<User> Users { get; set; } = new HashSet<User>();
    }
}