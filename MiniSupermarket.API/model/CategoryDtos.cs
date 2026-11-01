using System.ComponentModel.DataAnnotations;

namespace MiniSupermarket.API.Models
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public static CategoryDto FromEntity(Category category) => new()
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description
        };
    }

    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Tên nhóm hàng không được để trống!")]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }
    }
}
