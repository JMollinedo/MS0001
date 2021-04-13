using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS0001.engine
{
    class Jornada
    {
        private List<Game> games;
        public List<Game> Games { get { return new List<Game>(games); } }
        private Dictionary<string, bool> resultados;
        public Dictionary<string,bool> Resultados { get { return new Dictionary<string, bool>(resultados); } }

        public Jornada(List<TeamData[]> datas)
        {
            resultados = new Dictionary<string, bool>();
            games = new List<Game>(datas.Count);
            foreach (var item in datas)
            {
                TeamData A = item[0];
                TeamData B = item[1];
                games.Add(new Game(A, B));
            }
            foreach (var item in games)
            {
                if (item.CarrerasA > item.CarrerasB)
                {
                    resultados.Add(item.EquipoA.Nombre, true);
                    resultados.Add(item.EquipoB.Nombre, false);
                }
                else
                {
                    resultados.Add(item.EquipoA.Nombre, false);
                    resultados.Add(item.EquipoB.Nombre, true);
                }
            }
        }
    }
}
