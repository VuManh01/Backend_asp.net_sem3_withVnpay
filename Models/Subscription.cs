using System;
using System.Collections.Generic;

namespace project3api_be.Models;

public partial class Subscription
{
    public int SubId { get; set; }

    public int AccountId { get; set; }

    public int MembershipServiceId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string Status { get; set; } = "active";

    public decimal Price { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual MembershipService MembershipService { get; set; } = null!;
}
