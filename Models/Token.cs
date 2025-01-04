using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class Token
{   
    [Column("token_id")]
    public int TokenId { get; set; }
    [Column("account_id")]
    public int AccountId { get; set; }
    [Column("token1")]
    public string Token1 { get; set; } = null!;
    [Column("token_type")]
    public string TokenType { get; set; } = null!;
    [Column("created_at")]

    public DateTime? CreatedAt { get; set; }
    [Column("expires_at")]

    public DateTime? ExpiresAt { get; set; }

    [Column("is_revoked")]
    public bool? IsRevoked { get; set; }

    public virtual Account Account { get; set; } = null!;
}
