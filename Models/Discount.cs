using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class Discount
{   
    [Column("discount_id")]
    public int DiscountId { get; set; }
    [Column("amount")]

    public decimal Amount { get; set; }
    [Column("expires")]

    public DateTime? Expires { get; set; }
    [Column("quantity")]

    public int? Quantity { get; set; }
    [Column("description")]

    public string? Description { get; set; }
    [Column("created_at")]

    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]

    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<OrderMembership> OrderMemberships  { get; set; } = new List<OrderMembership>();

}
