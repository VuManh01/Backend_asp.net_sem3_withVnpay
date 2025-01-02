using System;
using System.Collections.Generic;

namespace project3api_be.Models;

public partial class ImageRecipe
{
    public int ImageRecipeId { get; set; }

    public int RecipeId { get; set; }

    public string? ImageLink { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;
}
