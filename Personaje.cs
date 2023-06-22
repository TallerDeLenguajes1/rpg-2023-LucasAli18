namespace Personajes
{

    public class Personaje
    {   
        //DATOS DEL PERSONAJE
        private string? nombre;
        private string? tipo;
        private string? apodo;
        private int edad;
        private DateTime fdn;

        //CARACTERISTICAS DEL PERSONAJE
        private int velocidad;
        private int destreza;
        private int fuerza;
        private int nivel;
        private int armadura;
        private int salud;

        //METODOS PARA LOS DATOS
        public string? Nombre { get => nombre; set => nombre = value; }
        public string? Tipo { get => tipo; set => tipo = value; }
        public string? Apodo { get => apodo; set => apodo = value; }
        public int Edad { get => edad; set => edad = value; }
        public DateTime Fdn { get => fdn; set => fdn = value; }

        //METODOS PARA LAS CARACTERISTICAS
        public int Velocidad { get => velocidad; set => velocidad = value; }
        public int Destreza { get => destreza; set => destreza = value; }
        public int Fuerza { get => fuerza; set => fuerza = value; }
        public int Nivel { get => nivel; set => nivel = value; }
        public int Armadura { get => armadura; set => armadura = value; }
        public int Salud { get => salud; set => salud = 100; }

    }






}