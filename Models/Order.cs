using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace project3api_be.Models;

public partial class Order
{   
    [Column("order_id")]
    public int OrderId { get; set; }
    [Column("total_price")]

    public decimal TotalPrice { get; set; }
    [Column("email")]

    public string Email { get; set; } = null!;
    [Column("phone_number")]

    public string PhoneNumber { get; set; } = null!;
    [Column("full_name")]

    public string FullName { get; set; } = null!;
    [Column("delivery_address")]

    public string DeliveryAddress { get; set; } = null!;
    [Column("status")]

    public string Status { get; set; } = "pending";
    [Column("discount_id")]

    public int? DiscountId { get; set; }
    [Column("created_at")]

    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]

    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]

    public DateTime? DeletedAt { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

}
