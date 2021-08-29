using System.Collections;
using System.Collections.Generic;

namespace Disc_Store.Entities
{
    public class Label
    {
        public int id { get; set; }

        public string name { get; set; }

        public ICollection<Disc> Discs { get; set; }
    }
}