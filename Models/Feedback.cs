using System;
using System.Collections.Generic;

namespace project3api_be.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }
}
