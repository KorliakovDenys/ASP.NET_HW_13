using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("Author")]
public class Author {
    [Key] public int Id { get; set; }

    [Required] [MaxLength(50)] public string? FirstName { get; set; }

    [Required] [MaxLength(50)] public string? LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public ICollection<Book>? Books { get; set; } = new List<Book>();
}