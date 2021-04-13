using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS0001.engine
{
    class Batting
    {
        public string Scenario { get; private set; }
        public string Team { get; private set; }
        public int Count { get; set; }
        public Batting(string scenario, string team)
        {
            Scenario = scenario;
            Team = team;
            Count = 1;
        }
    }
    class Temporada
    {
        private Dictionary<string, int[]> resultados;
        public Dictionary<string, int[]> Resultados { get { return new Dictionary<string, int[]>(resultados); } }
        private List<Jornada> jornadas;
        public List<Jornada> Jornadas { get { return new List<Jornada>(jornadas); } }
        private List<Batting> resumen;
        public List<Batting> Resumen { get { return new List<Batting>(resumen); } }
        public Temporada(List<TeamData> teams)
        {
            jornadas = new List<Jornada>();
            for (int i = 0; i < 54; i++)
            {
                List<TeamData[]> serie = Combinacion(teams);
                for (int j = 0; j < 3; j++)
                {
                    jornadas.Add(new Jornada(new List<TeamData[]>(serie)));
                }
            }
            resultados = new Dictionary<string, int[]>();
            foreach (var item in teams)
            {
                resultados.Add(item.Nombre, new int[] { 0, 0 });
            }
            var res = jornadas.Select(j => j.Resultados);
            foreach (var dicc in res)
            {
                foreach (var item in dicc)
                {
                    var r = resultados[item.Key];
                    if (item.Value) resultados[item.Key][0] = r[0] + 1;
                    else resultados[item.Key][1] = r[1] + 1;
                }
            }

            resumen = new List<Batting>();
            foreach (var jornada in jornadas)
            {
                foreach (var game in jornada.Games)
                {
                    foreach (var simulacion in game.Simulaciones)
                    {
                        var item = resumen.FirstOrDefault(
                            r => r.Team == simulacion.Team
                            && r.Scenario == simulacion.Scenario);
                        if(item == null)
                        {
                            resumen.Add(new Batting(simulacion.Scenario, simulacion.Team));
                        }
                        else
                        {
                            item.Count++;
                        }
                    }
                }
            }
        }
        public List<TeamData[]> Combinacion(List<TeamData> equipos)
        {
            List<TeamData[]> combs = new List<TeamData[]>(equipos.Count / 2);
            List<TeamData> copia = new List<TeamData>(equipos);
            Random random = new Random(DateTime.Now.Millisecond);
            while(copia.Count > 0)
            {
                int i1 = 0;
                int i2 = 0;
                while(i1 == i2)
                {
                    i1 = random.Next(0, copia.Count);
                    i2 = random.Next(0, copia.Count);
                }
                combs.Add(new TeamData[] { copia[i1], copia[i2] });
                if(i1 > i2)
                {
                    copia.RemoveAt(i1);
                    copia.RemoveAt(i2);
                }
                else
                {
                    copia.RemoveAt(i2);
                    copia.RemoveAt(i1);
                }
            }
            return combs;
        }
    }
}
