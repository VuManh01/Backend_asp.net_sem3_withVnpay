using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string? Password { get; set; }

    public string? FullName { get; set; }

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    [Column("order_membership_id")]
    public int? OrderMembershipId { get; set; }

    public virtual ICollection<PaymentMember> PaymentMembers { get; set; } = new List<PaymentMember>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
