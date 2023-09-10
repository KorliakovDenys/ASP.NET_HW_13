using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("Book")]
public class Book {
    [Key] public int Id { get; set; }

    [Required] [MaxLength(100)] public string? Title { get; set; }

    [Required] public int AuthorId { get; set; }

    [ForeignKey("AuthorId")] public Author? Author { get; set; }

    public DateTime PublicationDate { get; set; }
}