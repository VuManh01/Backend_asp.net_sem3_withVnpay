using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class Flavor
{   
    [Column("flavor_id")]
    public int FlavorId { get; set; }
    [Column("flavor_name")]

    public string FlavorName { get; set; } = null!;
    [Column("description")]

    public string? Description { get; set; }
    [Column("created_at")]

    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]

    public DateTime? UpdatedAt { get; set; }
    [Column("deleted_at")]

    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<RecipeFlavor> RecipeFlavors { get; set; } = new List<RecipeFlavor>();


    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
