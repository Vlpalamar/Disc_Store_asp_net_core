using System.Collections;
using System.Collections.Generic;

namespace Disc_Store.Entities
{
    public class Band
    {
        public int id { get; set; }

        public string name { get; set; }

        public ICollection<Musician> musicians { get; set; }

        public ICollection<Disc> discs{ get; set; }


    }
}