using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class Book
{   
    [Column("book_id")]
    public int BookId { get; set; }
    [Column("book_name")]
    public string BookName { get; set; } = null!;
    [Column("description")]
    public string? Description { get; set; }
    [Column("price")]
    public decimal Price { get; set; }
    [Column("image_url")]
    public string? ImageUrl { get; set; }
    [Column("stock_quantity")]
    public int StockQuantity { get; set; }
    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

}
