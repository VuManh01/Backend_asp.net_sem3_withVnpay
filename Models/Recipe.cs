using System;
using System.Collections.Generic;

namespace project3api_be.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public string RecipeName { get; set; } = null!;

    public int Servings { get; set; }

    public string Difficulty { get; set; } = "Easy";

    public decimal ActiveTime { get; set; }

    public decimal InactiveTime { get; set; }

    public string Ingredients { get; set; } = null!;

    public string PreparationMethod { get; set; } = null!;

    public string SubmittedBy { get; set; } = null!;

    public string Status { get; set; } = "pending";

    public bool? IsPost { get; set; }

    public int Rating { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Flavor> Flavors { get; set; } = new List<Flavor>();

    public ICollection<ImageRecipe> ImageRecipes { get; set; } = new List<ImageRecipe>();
}
