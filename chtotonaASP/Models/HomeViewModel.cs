using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;

namespace chtotonaASP.Models
{
    public class HomeViewModel
    {
        public List<CatList> ?CatList { get; set; }
        public List<News> ?News { get; set; }
    }
    
        public class LoginViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public class RegisterViewModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Пароли не совпадают")]
            public string ConfirmPassword { get; set; }

            [Required]
            public string FullName { get; set; }

            [Required]
            [Phone]
            public string Phone { get; set; }
        }
    

}
