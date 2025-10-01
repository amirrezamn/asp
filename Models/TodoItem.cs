using System.ComponentModel.DataAnnotations;

namespace asp.Models;

public class TodoItem
{
    
    public string Title { get; set; }=string.Empty;
    public string Description { get; set; }=string.Empty;
    [Key]
    public long Id { get; set; }

    public bool Selected { get; set; } = false;

    public bool Completed { get; set; } = false;

    // category name (string) — simpler than FK for quick use
    [Required]
    public string Category { get; set; } = "General";

}