using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS0001.engine
{
    class BatScenario
    {
        public string Name { get; private set; }
        public int Moves { get; private set; }
        public int Outs { get; private set; }
        public BatScenario(string name, int moves, int outs)
        {
            Name = name;
            Moves = moves;
            Outs = outs;
        }
    }
}
