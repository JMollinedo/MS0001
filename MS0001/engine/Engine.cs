using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS0001.engine
{
    class Engine
    {
        private List<Temporada> temporadas;
        public List<Temporada> Temporadas { get { return new List<Temporada>(temporadas); } }
        public Engine(int simulaciones)
        {
            temporadas = new List<Temporada>();
            var db = new modelacionEntities();
            var stats = db.teamStats.ToList();
            List<TeamData> data = new List<TeamData>();
            foreach (var item in stats)
            {
                data.Add(new TeamData(item));
            }
            for (int i = 0; i < simulaciones; i++)
            {
                temporadas.Add(new Temporada(data));
            }
        }
    }
}
