using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MS0001.engine
{
    class Accion
    {
        public string Llave { get; private set; }
        public int Contador { get; private set; }
        public double ValorA { get; set; }
        public double ValorB { get; set; }
        public Accion(string llave, int contador)
        {
            Llave = llave;
            Contador = contador;
        }
    }
    class TeamData
    {
        public string Nombre { get; private set; }
        private readonly List<Accion> contadores;
        public Dictionary<string,Accion> Contadores
        {
            get
            {
                return contadores.ToDictionary(c => c.Llave);
            }
        }

        private List<Rango> rangoA;
        public List<Rango> RangoA
        {
            get
            {
                return new List<Rango>(rangoA);
            }
        }
        private List<Rango> rangoB;
        public List<Rango> RangoB
        {
            get
            {
                return new List<Rango>(rangoB);
            }
        }

        public TeamData(teamStats teamStats)
        {
            Nombre = teamStats.TeamId;
            contadores = new List<Accion>
            {
                new Accion(nameof(teamStats.BaseOnBalls), teamStats.BaseOnBalls),
                new Accion(nameof(teamStats.DoublePlayed), teamStats.DoublePlayed),
                new Accion(nameof(teamStats.Doubles), teamStats.Doubles),
                new Accion(nameof(teamStats.FGOuts), teamStats.FGOuts),
                new Accion(nameof(teamStats.HitByPitch), teamStats.HitByPitch),
                new Accion(nameof(teamStats.HomeRuns), teamStats.HomeRuns),
                new Accion(nameof(teamStats.Sacrifice), teamStats.Sacrifice),
                new Accion(nameof(teamStats.Singles), teamStats.Singles),
                new Accion(nameof(teamStats.StrikeOut), teamStats.StrikeOut),
                new Accion(nameof(teamStats.Triple), teamStats.Triple)
            };
            double fulldiv(int num, int dem) => Convert.ToDouble(num) / Convert.ToDouble(dem);
            foreach (var item in contadores)
            {
                item.ValorA = fulldiv(item.Contador, teamStats.Plates);
                if (item.Llave == nameof(teamStats.Sacrifice) || item.Llave == nameof(teamStats.DoublePlayed))
                {
                    item.ValorB = 0;
                }
                else
                {
                    item.ValorB = fulldiv(item.Contador, teamStats.Plates - teamStats.Sacrifice - teamStats.DoublePlayed);
                }
            }
            double counter = 0;
            rangoA = new List<Rango>();
            var listaA = contadores.Select(c => new { c.Llave, c.ValorA })
                .OrderByDescending(o => o.ValorA);
            foreach (var item in listaA)
            {
                Rango rango = new Rango(counter, item.ValorA, item.Llave);
                rangoA.Add(rango);
                counter += item.ValorA;
            }
            counter = 0;
            rangoB = new List<Rango>();
            var listaB = contadores.Select(c => new { c.Llave, c.ValorB })
                .Where(c => c.Llave != nameof(teamStats.Sacrifice))
                .Where(c => c.Llave != nameof(teamStats.DoublePlayed))
                .OrderByDescending(o => o.ValorB);
            foreach (var item in listaB)
            {
                Rango rango = new Rango(counter, item.ValorB, item.Llave);
                rangoB.Add(rango);
                counter += item.ValorB;
            }
        }
    }
    class Rango
    {
        private readonly double Inicio;
        private readonly double Cierre;
        public string Accion { get; private set; }
        public Rango(double bas, double espacio, string accion)
        {
            Accion = accion;
            Inicio = bas;
            Cierre = bas + espacio;
        }
        public bool EnRango(double d)
        {
            return (Inicio <= d) && (d < Cierre);
        }
        public override string ToString()
        {
            return $"{Accion} | {Inicio} | {Cierre}";
        }
    }
}
