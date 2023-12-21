using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    public class BookParameters : RequestFeatures
    {
        public uint MinYear { get; set; }
        public uint MaxYear { get; set; } = 2023;
        public bool ValidYearRange => MaxYear > MinYear;
        public string SearchTerm { get; set; }
    }
}
