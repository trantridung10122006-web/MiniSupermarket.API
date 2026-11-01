using System.ComponentModel.DataAnnotations;

namespace MiniSupermarket.API.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? Barcode { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int? BrandId { get; set; }
        public int? SupplierId { get; set; }

        public static ProductDto FromEntity(Product product) => new()
        {
            ProductId = product.ProductId,
            Barcode = product.Barcode,
            ProductName = product.ProductName,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.CategoryName,
            BrandId = product.BrandId,
            SupplierId = product.SupplierId
        };
    }

    public class ProductRequestDto
    {
        [StringLength(100)]
        public string? Barcode { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống!")]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Giá bán phải lớn hơn 0!")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Tồn kho không được âm!")]
        public int StockQuantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Danh mục sản phẩm không hợp lệ!")]
        public int CategoryId { get; set; }

        public int? BrandId { get; set; }
        public int? SupplierId { get; set; }
    }
}
