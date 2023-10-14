using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Book
    {
        [Column("BookId")]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Book name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Book qenre is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Qenre is 30 characters.")]
        public string Qenre { get; set; }


        [Required(ErrorMessage = "Year of issue is a required field.")]
        [MaxLength(5, ErrorMessage = "Maximum length for the YearIssue is 5 characters.")]
        public int YearIssue { get; set; }


        [ForeignKey(nameof(Author))]
        public Guid AuthorId { get; set; }


        public Author? Author { get; set; }
    }
}
