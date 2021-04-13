using MS0001.engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MS0001
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Ingresar Carpeta");
            string carpeta = Console.ReadLine();
            string time = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string archivo = carpeta + "\\MS" + time + ".csv";
            Engine e = new Engine(100);
            int i = 1;
            File.AppendAllLines(archivo, new string[] { "Corrida,Equipo,Escenario,Cuenta" }, Encoding.UTF8);
            foreach (var tem in e.Temporadas)
            {
                Console.WriteLine($"Temporada {i}: G/P");
                foreach (var item in tem.Resultados)
                {
                    Console.WriteLine($"{item.Key}\t{item.Value[0]}\t{item.Value[1]}");
                }
                List<string> lines = new List<string>();
                foreach (var item in tem.Resumen)
                {
                    lines.Add($"{i},{item.Team},{item.Scenario},{item.Count}");
                }
                File.AppendAllLines(archivo, lines, Encoding.UTF8);
                i++;
            }
            Console.ReadLine();
        }
    }
}
