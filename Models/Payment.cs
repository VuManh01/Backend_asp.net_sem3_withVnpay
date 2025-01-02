using System;

namespace project3api_be.Models;

public partial class Payment
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; } = "online"; // Giá trị mặc định
    public string PaymentStatus { get; set; } = "pending"; // Giá trị mặc định
    public DateTime? PaidAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}