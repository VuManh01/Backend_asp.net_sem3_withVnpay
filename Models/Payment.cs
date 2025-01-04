using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class Payment
{   
    [Column("payment_id")]
    public int PaymentId { get; set; }
    [Column("order_id")]
    public int OrderId { get; set; }
    [Column("payment_method")]
    public string PaymentMethod { get; set; } = "online"; // Giá trị mặc định
    [Column("payment_status")]
    public string PaymentStatus { get; set; } = "pending"; // Giá trị mặc định
    [Column("paid_at")]
    public DateTime? PaidAt { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}