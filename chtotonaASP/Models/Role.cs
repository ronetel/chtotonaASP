using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace chtotonaASP.Models
{
    public partial class Role
    {
        public int IdRole { get; set; }

        [Required(ErrorMessage = "Название роли обязательно для заполнения.")]
        [StringLength(100, ErrorMessage = "Название роли не может превышать 100 символов.")]
        public string NameRole { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
