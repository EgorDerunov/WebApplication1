using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class AuthorForUpdateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string DateBirth { get; set; }
        public IEnumerable<BookForUpdateDto>? Books { get; set; }
    }
}
