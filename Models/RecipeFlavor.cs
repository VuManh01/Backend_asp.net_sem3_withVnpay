using System;
using System.ComponentModel.DataAnnotations;

namespace project3api_be.Models;

public partial class RecipeFlavor
{   
    [Key]
    public int RecipeId { get; set; }
    public int FlavorId { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
    public virtual Flavor Flavor { get; set; } = null!;
} 