using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Giá bán phải lớn hơn 0!")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Tồn kho không được âm!")]
        public int StockQuantity { get; set; }

        public int CategoryId { get; set; }
        public int? BrandId { get; set; }      
        public int? SupplierId { get; set; }    
        [StringLength(100)]
        public string? Barcode { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand? Brand { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier? Supplier { get; set; }
    }
}