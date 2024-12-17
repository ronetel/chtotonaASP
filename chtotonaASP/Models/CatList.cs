using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace chtotonaASP.Models
{
    public partial class CatList
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Изображение обязательно для заполнения.")]
        public string Img { get; set; } = null!;

        [Required(ErrorMessage = "Название товара обязательно для заполнения.")]
        [StringLength(100, ErrorMessage = "Название товара не может превышать 100 символов.")]
        public string NameProduct { get; set; } = null!;

        [Required(ErrorMessage = "Описание товара обязательно для заполнения.")]
        public string DescProduct { get; set; } = null!;

        [Required(ErrorMessage = "Цена обязательна для заполнения.")]
        [Range(1, int.MaxValue, ErrorMessage = "Цена должна быть положительной.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Тип продукта обязателен для заполнения.")]
        public int? IdproductType { get; set; }

        public virtual ProductType? IdproductTypeNavigation { get; set; }

        public virtual ICollection<ProductsInOrder> ProductsInOrders { get; set; } = new List<ProductsInOrder>();

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }


    public class CatListViewModel
    {
        public List<CatList> CatList { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public string Filter { get; set; }
    }
}