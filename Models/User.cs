using System.Collections.Generic;

namespace MVC_Lab_7.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public ICollection<Role> Roles { get; set; } = new HashSet<Role>();


    }
}
