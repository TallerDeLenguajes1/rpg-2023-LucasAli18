using Personajes;
namespace Fabrica
{
    public class FabricaDePj
    {
        public Personaje CrearPersonaje()
        {
        int num;
        string[] nombres = { "Juan", "María", "Carlos", "Laura","Lucas","Leandro","Jorge","Rafael" };
        string[] apodos = { "La bestia", "El enano", "El famoso", "El ladron","El toro","El dark","El rey","El tonto" };
        string[] tipos = { "Gigante", "Elfo", "Duende", "Mago","Guerrero","Arquero","Fantasma","Espiritu" };
        Random aleatorio = new Random();
        Random al = new Random();
        Random alea = new Random();
        Personaje nuevo = new Personaje();
        num = aleatorio.Next(0,8);
        nuevo.Nombre=nombres[num];
        num = al.Next(0,8);
        nuevo.Apodo=apodos[num];
        num = alea.Next(0,7);
        nuevo.Tipo=tipos[num];
        /* Console.WriteLine("Nombre: "+nuevo.Nombre);
        Console.WriteLine("Apodo: "+nuevo.Apodo);
        Console.WriteLine("Tipo: "+nuevo.Tipo); */
        return nuevo;
        }
        
    }
}