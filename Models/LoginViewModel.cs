using System.ComponentModel.DataAnnotations;

namespace MVC_Lab_7.Models
{
    public class LoginViewModel
    {
        public string Name { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
