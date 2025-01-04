using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace project3api_be.Models;

public partial class ImageRecipe
{   
    [Column("image_recipe_id")]
    public int ImageRecipeId { get; set; }
    [Column("recipe_id")]

    public int RecipeId { get; set; }
    [Column("image_link")]

    public string? ImageLink { get; set; }
    [Column("created_at")]

    public DateTime? CreatedAt { get; set; }
    [Column("updated_at")]

    public DateTime? UpdatedAt { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
}
