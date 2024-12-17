using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace chtotonaASP.Models;

public partial class ProductsInOrder
{
    public int PinorId { get; set; }

    [Required(ErrorMessage = "Пользователь обязателен для заполнения.")]
    public int? UserId { get; set; }

    [Required(ErrorMessage = "Товар обязателен для заполнения.")]
    public int? ProductId { get; set; }

    [Required(ErrorMessage = "Количество обязательно для заполнения.")]
    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть положительным числом.")]
    public int Quantity { get; set; }

    public virtual CatList? Product { get; set; }

    public virtual User? User { get; set; }
}
public class PinorViewModel
{
    public List<ProductsInOrder> ProductsInOrder { get; set; }

    public int TotalSum { get; set; }
}
