using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shippery.Models.Resources
{
    public class Iterator
    {
        int start;
        int i;

        public Iterator() { start = -1; i = start; }
        public Iterator(int iteration) { start = iteration-1; i = start; }

        public int Iterate() { i++; return i; }
        public int Iterate(int iterations) { i += iterations; return i; }
        public void Reset() { i = start; }

        public int Position { get => i; }
    }
}
