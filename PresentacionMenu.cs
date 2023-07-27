using Personajes;
using Mecanicas;

namespace Menu
{
    public class OpcionesMenu
    {
        public void MostrarMenuPrincipal()
        {
                Console.WriteLine("╔══════════════════════════╗");
                Console.WriteLine("║        Menú Principal    ║");
                Console.WriteLine("╠══════════════════════════╣");
                Console.WriteLine("║ 1. Juego Nuevo           ║");
                Console.WriteLine("║ 2. Continuar             ║");
                Console.WriteLine("║ 3. Salir                 ║");
                Console.WriteLine("╚══════════════════════════╝");
        }
        public void MostrarMenuSecundario()
        {
            Console.WriteLine("***************************************************");
            Console.WriteLine("*                 MENÚ DE OPCIONES                *");
            Console.WriteLine("***************************************************");
            Console.WriteLine();
            Console.WriteLine("1. Seleccionar Pais de inicio");
            Console.WriteLine("2. Atacar");
            Console.WriteLine("3. Guardar Partida(Recuerda atacar antes de guardar");
            Console.WriteLine("caso contrario, el rival tendra ventaja con respecto a vos)");
            Console.WriteLine("4. Finalizar Turno");
            Console.WriteLine();
            Console.Write("Selecciona una opción: ");
        }
        public void MostrarPresentacion()
        {
            Console.WriteLine("***************************************************");
            Console.WriteLine("*                BIENVENIDO A SHARPTEG            *");
            Console.WriteLine("***************************************************");
            Console.WriteLine();
            Console.WriteLine("EMPEZARAS CON 3 SOLDADOS");
            Console.WriteLine("DEBERÁS ESCOGER UN PAÍS INICIAL PARA COMENZAR TU CONQUISTA");
            Console.WriteLine();
            Console.WriteLine("EL OBJETIVO DEL JUEGO ES EVITAR QUE EL VILLANO SE QUEDE CON TODA SUDAMÉRICA");
            Console.WriteLine("DEBERÁS FORMAR UN EJÉRCITO Y ESTRATEGIAS PARA LOGRARLO");
            Console.WriteLine();
            Console.WriteLine("GANA LA PRIMERA PERSONA EN OBTENER TODOS LOS PAÍSES");
            Console.WriteLine("O LA PRIMERA PERSONA QUE DEJA AL RIVAL SIN PAISES");
            Console.WriteLine();
            Console.WriteLine("¡PREPÁRATE PARA LA BATALLA Y DEMUESTRA TU HABILIDAD COMO ESTRATEGA!");
            Console.WriteLine();
            Console.WriteLine("***************************************************");
        }
    }
    
}