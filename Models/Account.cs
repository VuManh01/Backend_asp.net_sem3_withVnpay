using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class Account
{      
    [Column("account_id")]
    public int AccountId { get; set; }
    [Column("email")]
    public string Email { get; set; } = string.Empty;
    [Column("password")]

    public string? Password { get; set; }
    [Column("full_name")]

    public string? FullName { get; set; }
    [Column("role_id")]

    public int RoleId { get; set; }
    [Column("is_active")]

    public bool? IsActive { get; set; }
    [Column("created_at")]

    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]

    public DateTime? UpdatedAt { get; set; }
    [Column("order_membership_id")]
    public int? OrderMembershipId { get; set; }

    // Quan hệ n-1 với bảng Roles
    [ForeignKey("RoleId")]
    public virtual Role? Role { get; set; }
    // Quan hệ 1-n với bảng Subscriptions
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    // Quan hệ 1-n với bảng Tokens
    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
    // Quan hệ n-1 với bảng OrderMemberships
    [ForeignKey("OrderMembershipId")]
    public virtual OrderMembership? OrderMembership { get; set; }
    
}
