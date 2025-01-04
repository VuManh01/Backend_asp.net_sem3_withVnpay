using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace project3api_be.Models;

public partial class MembershipService
{   
    [Column("membership_service_id")]
    public int MembershipServiceId { get; set; }
    [Column("name")]
    public string Name { get; set; } = null!;
    [Column("duration_in_day")]
    public int DurationInDay { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public virtual ICollection<OrderMembership> OrderMemberships  { get; set; } = new List<OrderMembership>();

}
