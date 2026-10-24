using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MiniSupermarket.API.Models
{
    [Table("Brands")]
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Tên thương hiệu không được để trống!")]
        [StringLength(100, ErrorMessage = "Tên thương hiệu không vượt quá 100 ký tự!")]
        public string BrandName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        // Quan hệ 1 - N: Một thương hiệu có nhiều sản phẩm
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
    }
}