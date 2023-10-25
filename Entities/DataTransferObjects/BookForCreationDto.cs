using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class BookForCreationDto
    {
        public string? Name { get; set; }
        public int YearIssue { get; set; }
        public string? Qenre { get; set; }
    }
}
