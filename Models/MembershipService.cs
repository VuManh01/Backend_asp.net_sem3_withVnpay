using System;
using System.Collections.Generic;

namespace project3api_be.Models;

public partial class MembershipService
{
    public int MembershipServiceId { get; set; }

    public string Name { get; set; } = null!;

    public int DurationInDay { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
