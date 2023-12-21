using Entities.DataTransferObjects.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects.Author
{
    public class AuthorForManipulation
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DateBirth { get; set; }
        public IEnumerable<BookForManipulation>? Books { get; set; }
    }
}
