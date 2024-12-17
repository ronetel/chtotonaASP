using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace chtotonaASP.Models;

public partial class User
{
    public int IdUser { get; set; }

    [Required(ErrorMessage = "Полное имя обязательно для заполнения.")]
    [StringLength(100, ErrorMessage = "Полное имя не может превышать 100 символов.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Email обязателен для заполнения.")]
    [EmailAddress(ErrorMessage = "Некорректный адрес электронной почты.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Телефон обязателен для заполнения.")]
    [Phone(ErrorMessage = "Некорректный номер телефона.")]
    public string Phone { get; set; } = null!;

    public string? PasswordHash { get; set; }

    [Required(ErrorMessage = "Роль обязательна для заполнения.")]
    public int? RoleId { get; set; }

    public DateTime? DateJoined { get; set; }

    public virtual ICollection<ProductsInOrder> ProductsInOrders { get; set; } = new List<ProductsInOrder>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual Role? Role { get; set; }
}
