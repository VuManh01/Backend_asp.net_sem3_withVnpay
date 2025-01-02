using System;
using System.Collections.Generic;
namespace project3api_be.Models;

public partial class PaymentMember
{
    public int PaymentMemberId { get; set; }

    public int? AccountId { get; set; }

    public string? PaymentType { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Account? Account { get; set; }
}
