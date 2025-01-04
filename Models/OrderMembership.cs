using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace project3api_be.Models;
[Table("order_membership")]
public partial class OrderMembership
{   
    [Column("order_membership_id")]
    public int OrderMembershipId { get; set; }
    [Column("membership_service_id")]
    public int MembershipServiceId { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("status")]
    public string? Status { get; set; } = "pending";
    [Column("order_status")]
    public string? OrderStatus { get; set; } = "pending";
    [Column("discount_id")]
    public int? DiscountId { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public virtual MembershipService MembershipService { get; set; } = null!;
    public virtual Discount? Discount { get; set; }
}