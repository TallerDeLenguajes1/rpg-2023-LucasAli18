using Personajes;
using Fabrica;

int cantidadPj;
FabricaDePj[] heroes;

Console.WriteLine("Cuantos heroes desea usar? ");
int.TryParse(Console.ReadLine(),out cantidadPj);
Console.WriteLine("La cantidad de pj elegidas es: "+cantidadPj);


FabricaDePj nuevo = new FabricaDePj();
Personaje pj = new Personaje();
pj=nuevo.CrearPersonaje();
Console.WriteLine("Nombre: "+pj.Nombre);
Console.WriteLine("Apodo: "+pj.Apodo);
Console.WriteLine("Tipo: "+pj.Tipo);