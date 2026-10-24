using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MiniSupermarket.API.Models
{
    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống!")]
        [StringLength(150)]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ContactName { get; set; }

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        // Quan hệ 1 - N: Một nhà cung cấp có nhiều sản phẩm
        [JsonIgnore]
        public virtual ICollection<Product>? Products { get; set; }
    }
}