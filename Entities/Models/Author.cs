using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Author
    {
        [Column("AuthorId")]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Author name is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string Name { get; set; }

        
        [Required(ErrorMessage = "Author surname is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for the Surname is 60 characters.")]
        public string Surname { get; set; }


        [Required(ErrorMessage = "Author date of birth is a required field.")]
        [MaxLength(60, ErrorMessage = "Maximum length for rhe DateBirth is 11 characte")]
        public string DateBirth { get; set; }


        public ICollection<Book>? Books { get; set; }
    }
}
