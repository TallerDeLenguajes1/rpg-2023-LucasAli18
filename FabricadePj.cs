using Personajes;
using Limites;
namespace Fabrica
{
    public class FabricaPJ
    {
        public Personaje GenerarPersonaje(string nombre)
        {
            Personaje pj = new Personaje();
            pj.Nombre=nombre;
            pj.Turno=0;
            pj.CondicionParaGanar=0;
            pj.CantidadSoldados=0;
            pj.Paises = new List<string>();
            return pj;
        }

        public Personaje GenerarVillano()
        {
            Random al = new Random();
            string[] nombres = {"Juan","Leandro","Agustin"};
            Personaje villano = new Personaje();
            villano.Nombre=nombres[al.Next(0,nombres.Count())];
            villano.Paises = new List<string>();
            villano.CantidadSoldados=0;
            villano.CondicionParaGanar=0;
            villano.Turno=0;
            return villano;
        } 
    }
}