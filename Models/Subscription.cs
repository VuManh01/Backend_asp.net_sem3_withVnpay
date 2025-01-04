using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class Subscription
{   
    [Column("sub_id")]
    public int SubId { get; set; }
    [Column("account_id")]
    public int AccountId { get; set; }
    [Column("membership_service_id")]
    public int MembershipServiceId { get; set; }
    [Column("start_date")]
    public DateTime StartDate { get; set; }
    [Column("end_date")]
    public DateTime EndDate { get; set; }
    [Column("status")]
    public string Status { get; set; } = "active";
    [Column("price")]
    public decimal Price { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("update_at")]
    public DateTime? UpdatedAt { get; set; }
    public virtual Account Account { get; set; } = null!;

    public virtual MembershipService MembershipService { get; set; } = null!;
}
