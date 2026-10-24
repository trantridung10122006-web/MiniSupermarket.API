using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MiniSupermarket.API.Models
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        // Bổ sung thuộc tính Phone để tương thích với WinForms CustomerDto
        public string? Phone
        {
            get => PhoneNumber;
            set => PhoneNumber = value;
        }

        public string? Email { get; set; }
        public string? Address { get; set; }
        public int? RewardPoints { get; set; }

        // Bổ sung thuộc tính Points để tương thích với WinForms
        public int Points
        {
            get => RewardPoints ?? 0;
            set => RewardPoints = value;
        }

        public string? MembershipRank { get; set; }
        public DateTime CreatedAt { get; set; }

        public static CustomerDto FromEntity(Customer c)
        {
            return new CustomerDto
            {
                CustomerId = c.CustomerId,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Address = c.Address,
                RewardPoints = c.RewardPoints,
                MembershipRank = c.MembershipRank ?? "Đồng",
                CreatedAt = c.CreatedAt
            };
        }
    }

    public class CustomerRequest
    {
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên khách hàng!")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(15)]
        public string? PhoneNumber { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(255)]
        public string? Email { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        public int? RewardPoints { get; set; }
        public int? Points { get; set; }

        public string? MembershipRank { get; set; }

        // Lấy số điện thoại dù client gửi bằng Phone hay PhoneNumber
        public string? GetPhoneNumber()
        {
            if (!string.IsNullOrWhiteSpace(Phone)) return Phone.Trim();
            if (!string.IsNullOrWhiteSpace(PhoneNumber)) return PhoneNumber.Trim();
            return null;
        }

        // Lấy điểm thưởng dù client gửi bằng Points hay RewardPoints
        public int GetRewardPoints()
        {
            return Points ?? RewardPoints ?? 0;
        }
    }

    public class OrderHistoryDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CashierUsername { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
