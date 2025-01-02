using System;
using System.Collections.Generic;

namespace project3api_be.Models;

public partial class OrderMembership
{
    public int OrderMembershipId { get; set; }
    public int MembershipServiceId { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = "pending";
    public int? DiscountId { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public virtual MembershipService MembershipService { get; set; } = null!;
    public virtual Discount? Discount { get; set; }
}