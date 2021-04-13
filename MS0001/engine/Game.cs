using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS0001.engine
{
    class BatsResult
    {
        public string Scenario { get; private set; }
        public string Team { get; private set; }
        public BatsResult(string scenario, string team)
        {
            Scenario = scenario;
            Team = team;
        }
    }
    class Game
    {
        private static Dictionary<string, BatScenario> GetBatScenarios()
        {
            var ts = new teamStats();
            List<BatScenario> scenarios = new List<BatScenario>
            {
                new BatScenario(nameof(ts.Singles), 1, 0)
                ,new BatScenario(nameof(ts.Doubles), 2, 0)
                ,new BatScenario(nameof(ts.Triple), 3, 0)
                ,new BatScenario(nameof(ts.HomeRuns), 4, 0)
                ,new BatScenario(nameof(ts.BaseOnBalls), 1, 0)
                ,new BatScenario(nameof(ts.HitByPitch), 1, 0)
                ,new BatScenario(nameof(ts.Sacrifice), 1, 1)
                ,new BatScenario(nameof(ts.StrikeOut), 0, 1)
                ,new BatScenario(nameof(ts.DoublePlayed), 0, 2)
                ,new BatScenario(nameof(ts.FGOuts), 0, 1)
            };
            return scenarios.ToDictionary(s => s.Name);
        }
        public TeamData EquipoA { get; private set; }
        public TeamData EquipoB { get; private set; }
        private List<Inning> ResultadosA { get; set; }
        private List<Inning> ResultadosB { get; set; }

        private List<BatsResult> simulaciones;
        public List<BatsResult> Simulaciones { get { return new List<BatsResult>(simulaciones); } }
        
        public int CarrerasA
        {
            get
            {
                return ResultadosA.Sum(a => a.Carreras);
            }
        }
        public int CarrerasB
        {
            get
            {
                return ResultadosB.Sum(b => b.Carreras);
            }
        }

        public Game(TeamData a, TeamData b)
        {
            EquipoA = a;
            EquipoB = b;
            ResultadosA = new List<Inning>(9);
            ResultadosB = new List<Inning>(9);
            simulaciones = new List<BatsResult>();
            while (ResultadosA.Count < 9 && ResultadosB.Count < 9)
            {
                ResultadosA.Add(PlayInning(EquipoA));
                ResultadosB.Add(PlayInning(EquipoB));
            }
            int ei = 11;
            while (CarrerasA == CarrerasB && ei > 0)
            {
                ResultadosA.Add(PlayInning(EquipoA));
                ResultadosB.Add(PlayInning(EquipoB));
                ei--;
            }
            if(CarrerasA == CarrerasB)
            {
                Random random = new Random(DateTime.Now.Millisecond);
                double d = random.NextDouble();
                if (d < 0.5)
                {
                    ResultadosA.Add(Inning.OneRun(true));
                    ResultadosB.Add(Inning.OneRun(false));
                }
                else
                {
                    ResultadosA.Add(Inning.OneRun(false));
                    ResultadosB.Add(Inning.OneRun(true));
                }
            }
        }
        private Inning PlayInning(TeamData data)
        {
            var scenarios = GetBatScenarios();
            Random random = new Random(DateTime.Now.Millisecond);
            Inning inning = new Inning();
            while (inning.IsActive && inning.Carreras < 9)
            {
                double r = random.NextDouble();
                string accion;
                if (inning.HaveRunners) accion = data.RangoA.FirstOrDefault(d => d.EnRango(r)).Accion;
                else accion = data.RangoB.FirstOrDefault(d => d.EnRango(r)).Accion;
                var scenario = scenarios[accion];
                inning = inning.AddPlate();
                inning = inning.Out(scenario.Outs);
                inning = inning.Move(scenario.Moves);
                simulaciones.Add(new BatsResult(scenario.Name, data.Nombre));
            }
            while (inning.IsActive)
            {
                inning = inning.Out(1);
            }
            return inning;
        }
    }
}
