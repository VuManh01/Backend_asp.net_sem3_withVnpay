using System;
using System.Collections.Generic;
namespace project3api_be.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public decimal TotalPrice { get; set; }

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string DeliveryAddress { get; set; } = null!;

    public string Status { get; set; } = "pending";

    public int? DiscountId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

}
