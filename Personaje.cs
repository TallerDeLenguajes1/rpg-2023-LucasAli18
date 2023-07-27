namespace Personajes
{

    public class Personaje
    {   
        //DATOS DEL PERSONAJE
        private int turno;
        private int cantidadSoldados;
        private string? nombre;
        private List<string>? paises;
        private int condicionParaGanar;

        public int CantidadSoldados { get => cantidadSoldados; set => cantidadSoldados = value; }
        public string? Nombre { get => nombre; set => nombre = value; }
        public List<string>? Paises { get => paises; set => paises = value; }
        public int CondicionParaGanar { get => condicionParaGanar; set => condicionParaGanar = value; }
        public int Turno { get => turno; set => turno = value; }
        public void AgregarPais(string pais)
            {
                Paises!.Add(pais);
            }
        public void BorrarPais(string pais)
            {
                Paises!.Remove(pais);
            }
    
    } 

}