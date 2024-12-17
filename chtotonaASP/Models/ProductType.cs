using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace chtotonaASP.Models
{
    public partial class ProductType
    {
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "Название типа продукта обязательно для заполнения.")]
        [StringLength(100, ErrorMessage = "Название типа продукта не может превышать 100 символов.")]
        public string? ProductType1 { get; set; }

        public virtual ICollection<CatList> CatLists { get; set; } = new List<CatList>();
    }
}
