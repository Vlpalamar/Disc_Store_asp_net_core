using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Disc_Store.Entities
{
    public class Musician
    {
        public int id { get; set; }

        public string name { get; set; }

        public ICollection<Band> bands { get; set; }

        public Role roleInGroup { get; set; }
    }
}
