using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class OrderDetail
{   
    [Column("order_detail_id")]
    public int OrderDetailId { get; set; }
    [Column("order_id")]

    public int OrderId { get; set; }
    [Column("book_id")]

    public int BookId { get; set; }
    [Column("quantity")]

    public int Quantity { get; set; }
    [Column("price")]

    public decimal Price { get; set; }
    [Column("created_at")]

    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]

    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]

    public DateTime? DeletedAt { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
