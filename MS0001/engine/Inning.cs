using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS0001.engine
{
    class Inning
    {
        private readonly bool[] bases;
        public int Carreras { get; private set; }
        public int Outs { get; private set; }
        public bool IsActive
        {
            get
            {
                return Outs < 3;
            }
        }
        public bool HaveRunners
        {
            get
            {
                int c = bases.Count(b => b == true);
                return c > 0;
            }
        }
        public Inning()
        {
            bases = new bool[5];
            Carreras = 0;
            Outs = 0;
        }

        public static Inning OneRun(bool isWinner)
        {
            var ini = new Inning();
            if (isWinner)
            {
                ini = ini.Move(4);
            }
            for (int i = 0; i < 3; i++)
            {
                ini = ini.Out();
            }
            return ini;
        }

        private Inning Copy()
        {
            var copy = new Inning
            {
                Carreras = Carreras,
                Outs = Outs
            };
            bases.CopyTo(copy.bases, 0);
            return copy;
        }
        private Inning Move()
        {
            var copy = Copy();
            for (int i = 4; i >= 1; i--)
            {
                copy.bases[i] = copy.bases[i - 1];
            }
            copy.bases[0] = false;
            if (copy.bases[4])
            {
                copy.Carreras++;
                copy.bases[4] = false;
            }
            return copy;
        }
        public Inning Move(int n)
        {
            var copy = Copy();
            while(n > 0)
            {
                copy = copy.Move();
                n--;
            }
            return copy;
        }
        private Inning Out()
        {
            var copy = Copy();
            copy.bases[0] = false;
            copy.Outs++;
            return copy;
        }
        private Inning DoublePlay()
        {
            var copy = Out();
            if (copy.bases[3]) copy.bases[3] = false;
            else if (copy.bases[2]) copy.bases[2] = false;
            else copy.bases[1] = false;
            return copy;
        }
        public Inning Out(int n)
        {
            if (n == 1) return Out();
            if (n == 2) return DoublePlay();
            return Copy();
        }
        public Inning AddPlate()
        {
            var copy = Copy();
            copy.bases[0] = true;
            return copy;
        }
    }
}
