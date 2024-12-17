using System;
using System.ComponentModel.DataAnnotations;

namespace chtotonaASP.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Товар обязателен для заполнения.")]
        public int? ProductId { get; set; }

        [Required(ErrorMessage = "Пользователь обязателен для заполнения.")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Рейтинг обязателен для заполнения.")]
        [Range(1, 5, ErrorMessage = "Рейтинг должен быть от 1 до 5.")]
        public int? Rating { get; set; }

        [Required(ErrorMessage = "Комментарий обязателен для заполнения.")]
        public string? Comment { get; set; }

        public DateTime? Created { get; set; }

        public virtual CatList? Product { get; set; }

        public virtual User? User { get; set; }
    }


    public class ReviewsViewModel
    {
        public List<Review>? Review { get; set; }
    }
}