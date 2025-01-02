using System;
using System.Collections.Generic;

namespace project3api_be.Models;

public partial class Discount
{
    public int DiscountId { get; set; }

    public decimal Amount { get; set; }

    public DateTime? Expires { get; set; }

    public int? Quantity { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
