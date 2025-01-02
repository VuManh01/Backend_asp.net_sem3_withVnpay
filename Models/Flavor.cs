using System;
using System.Collections.Generic;

namespace project3api_be.Models;

public partial class Flavor
{
    public int FlavorId { get; set; }

    public string FlavorName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
