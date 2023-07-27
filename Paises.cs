using Personajes;
using System;
using System.Collections.Generic;
namespace Limites
{

public class Pais
{
    public int Id { get; set;}
    public string Nombre { get; set; }
    public List<string> PaisesLimitrofes { get; set; }
    public int Soldados{get; set;}
    public string? Capital {get; set;}
    public string? duenio{get; set;}
    public void AgregarPaisLimitrofe(string pais)
    {
        PaisesLimitrofes.Add(pais);
    }
    public Pais(string nombre, int soldados, string capital)
    {
        Nombre=nombre;
        Soldados=soldados;
        Capital = capital;
        duenio = null;
        PaisesLimitrofes = new List<string>();
    }
    public void MostrarPaisesLimitrofes(List<Pais> Paises)
    {
        foreach (var pais in Paises)
        {
            Console.WriteLine("Paises limitrofes de:"+pais.Nombre,":");
            foreach (var limite in pais.PaisesLimitrofes)
            {
            Console.WriteLine("El limite es: "+limite);
            }
        }
    }

}
    public class generadorPais
    {
        public List<Pais> GenerarPaises()
            {

                List<Pais> Paises = new List<Pais>();
                // Crear instancias de países
                Pais argentina = new Pais("Argentina",0,"Buenos Aires");
                Pais bolivia = new Pais("Bolivia",0,"La Paz");
                Pais brasil = new Pais("Brasil",0,"Brasília");
                Pais chile = new Pais("Chile",0,"Santiago");
                Pais colombia = new Pais("Colombia",0,"Bogotá");
                Pais ecuador = new Pais("Ecuador",0,"Quito");
                Pais guyana = new Pais("Guyana",0,"George Town");
                Pais paraguay = new Pais("Paraguay",0,"Asunción");
                Pais peru = new Pais("Peru",0,"Lima");
                Pais surinam = new Pais("Surinam",0,"Paramaribo");
                Pais uruguay = new Pais("Uruguay",0,"Montevideo");
                Pais venezuela = new Pais("Venezuela",0,"Capito");
                // Definir los países limítrofes de cada país
                argentina.AgregarPaisLimitrofe(bolivia.Nombre);
                argentina.AgregarPaisLimitrofe(brasil.Nombre);
                argentina.AgregarPaisLimitrofe(chile.Nombre);
                argentina.AgregarPaisLimitrofe(paraguay.Nombre);
                argentina.AgregarPaisLimitrofe(uruguay.Nombre);

                bolivia.AgregarPaisLimitrofe(argentina.Nombre);
                bolivia.AgregarPaisLimitrofe(brasil.Nombre);
                bolivia.AgregarPaisLimitrofe(chile.Nombre);
                bolivia.AgregarPaisLimitrofe(paraguay.Nombre);
                bolivia.AgregarPaisLimitrofe(peru.Nombre);

                brasil.AgregarPaisLimitrofe(argentina.Nombre);
                brasil.AgregarPaisLimitrofe(bolivia.Nombre);
                brasil.AgregarPaisLimitrofe(colombia.Nombre);
                brasil.AgregarPaisLimitrofe(guyana.Nombre);
                brasil.AgregarPaisLimitrofe(paraguay.Nombre);
                brasil.AgregarPaisLimitrofe(peru.Nombre);
                brasil.AgregarPaisLimitrofe(surinam.Nombre);
                brasil.AgregarPaisLimitrofe(uruguay.Nombre);
                brasil.AgregarPaisLimitrofe(venezuela.Nombre);

                chile.AgregarPaisLimitrofe(argentina.Nombre);
                chile.AgregarPaisLimitrofe(bolivia.Nombre);
                chile.AgregarPaisLimitrofe(peru.Nombre);

                colombia.AgregarPaisLimitrofe(brasil.Nombre);
                colombia.AgregarPaisLimitrofe(ecuador.Nombre);
                colombia.AgregarPaisLimitrofe(venezuela.Nombre);
                colombia.AgregarPaisLimitrofe(peru.Nombre);

                ecuador.AgregarPaisLimitrofe(colombia.Nombre);
                ecuador.AgregarPaisLimitrofe(peru.Nombre);

                guyana.AgregarPaisLimitrofe(brasil.Nombre);
                guyana.AgregarPaisLimitrofe(surinam.Nombre);
                guyana.AgregarPaisLimitrofe(venezuela.Nombre);

                paraguay.AgregarPaisLimitrofe(argentina.Nombre);
                paraguay.AgregarPaisLimitrofe(bolivia.Nombre);
                paraguay.AgregarPaisLimitrofe(brasil.Nombre);

                peru.AgregarPaisLimitrofe(bolivia.Nombre);
                peru.AgregarPaisLimitrofe(brasil.Nombre);
                peru.AgregarPaisLimitrofe(chile.Nombre);
                peru.AgregarPaisLimitrofe(colombia.Nombre);
                peru.AgregarPaisLimitrofe(ecuador.Nombre);

                surinam.AgregarPaisLimitrofe(brasil.Nombre);
                surinam.AgregarPaisLimitrofe(guyana.Nombre);

                uruguay.AgregarPaisLimitrofe(argentina.Nombre);
                uruguay.AgregarPaisLimitrofe(brasil.Nombre);

                venezuela.AgregarPaisLimitrofe(brasil.Nombre);
                venezuela.AgregarPaisLimitrofe(guyana.Nombre);
                //CREO LA LISTA DE TODOS LOS PAISES
                Paises.Add(argentina);
                Paises.Add(brasil);
                Paises.Add(surinam);
                Paises.Add(chile);
                Paises.Add(ecuador);
                Paises.Add(bolivia);
                Paises.Add(guyana);
                Paises.Add(peru);
                Paises.Add(colombia);
                Paises.Add(paraguay);
                Paises.Add(uruguay);
                Paises.Add(venezuela);
                //CARGA DE ID
                int id=1;
                foreach (var pais in Paises)
                {
                    pais.Id=id;
                    id++;
                }
                //RETORNO LA LISTA
                return Paises;
            }
    }
}