using System.ComponentModel.DataAnnotations;

namespace chtotonaASP.Models
{
    public partial class News
    {
        public int NewId { get; set; }

        [Required(ErrorMessage = "Заголовок обязателен для заполнения.")]
        [StringLength(200, ErrorMessage = "Заголовок не может превышать 200 символов.")]
        public string? Caption { get; set; }

        [Required(ErrorMessage = "Ссылка обязательна для заполнения.")]
        [Url(ErrorMessage = "Некорректный URL.")]
        public string? Link { get; set; }
    }
}
