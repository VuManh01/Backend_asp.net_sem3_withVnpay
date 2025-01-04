using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace project3api_be.Models;

public partial class PaymentMember
{   
    [Column("payment_member_id")]
    public int PaymentMemberId { get; set; }
    [Column("order_membership_id")]
    public int OrderMembershipId { get; set; }
    // public int? AccountId { get; set; } //<-xem lại
    [Column("payment_type")]        
    public string? PaymentType { get; set; }
    [Column("paid_at")]
    public DateTime? PaidAt { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
    [ForeignKey("OrderMembershipId")]
    public virtual OrderMembership OrderMembership { get; set; } = null!;

}
