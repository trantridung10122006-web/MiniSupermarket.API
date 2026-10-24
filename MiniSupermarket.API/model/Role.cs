using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MiniSupermarket.API.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Tên vai trò không được để trống!")]
        [StringLength(50)]
        public string RoleName { get; set; } = string.Empty; // e.g. "Admin", "Thu Ngân", "Khách Hàng"

        [StringLength(255)]
        public string? Description { get; set; }

        [JsonIgnore]
        public virtual ICollection<User>? Users { get; set; }
    }
}