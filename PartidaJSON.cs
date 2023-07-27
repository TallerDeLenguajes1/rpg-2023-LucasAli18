using System.Text.Json;
using System.Text.Json.Serialization;
using Personajes;
using Limites;

namespace PartidaJSON
{
    public class PersonajesJson
    {
        public void GuardarPersonaje(Personaje player)
        {
            // Serializar el personaje a JSON
            string json = JsonSerializer.Serialize(player, new JsonSerializerOptions { WriteIndented = true });
            // Guardar el JSON en un archivo
            string filePath = "PersonajeGuardado.json";
            try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.Write(json);
                    }

                    Console.WriteLine("JUGADOR GUARDADO.");
                }
            catch (Exception ex)
                {
                    Console.WriteLine($"Error al escribir en el archivo: {ex.Message}");
                }
        }
        public void GuardarVillano(Personaje villano)
        {
            // Serializar el personaje a JSON
            string json = JsonSerializer.Serialize(villano, new JsonSerializerOptions { WriteIndented = true });
            // Guardar el JSON en un archivo
            string filePath = "VillanoGuardado.json";
            try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.Write(json);
                    }

                    Console.WriteLine("VILLANO GUARDADO.");
                }
            catch (Exception ex)
                {
                    Console.WriteLine($"Error al escribir en el archivo: {ex.Message}");
                }
        }
        public Personaje CargarPersonaje()
        {
            Personaje cargar = new Personaje();
            Personaje devolver = new Personaje();
            string filePath = "PersonajeGuardado.json";
        try
        {
            // Leer el contenido del archivo JSON
            string json;
            using (StreamReader reader = new StreamReader(filePath))
            {
                json = reader.ReadToEnd();
            }
            // Deserializar el contenido JSON a una lista de clientes
            cargar = JsonSerializer.Deserialize<Personaje>(json)!;
            if (cargar!=null)
            {
                devolver = cargar;
            }else
            {
                Console.WriteLine("No hay personaje guardado");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer el archivo JSON: {ex.Message}");
        }
        return devolver;
        }
        public Personaje CargarVillano()
        {

            Personaje cargar = new Personaje();
            Personaje devolver = new Personaje();
            string filePath = "VillanoGuardado.json";
        try
        {
            // Leer el contenido del archivo JSON
            string json;
            using (StreamReader reader = new StreamReader(filePath))
            {
                json = reader.ReadToEnd();
            }
            // Deserializar el contenido JSON a una lista de clientes
            cargar = JsonSerializer.Deserialize<Personaje>(json)!;
            if (cargar!=null)
            {
                devolver = cargar;
            }else
            {
                Console.WriteLine("No hay personaje guardado");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer el archivo JSON: {ex.Message}");
        }
        return devolver;
        }
    }

    public class PartidaGuardada
    {
        public void GuardarPartida(List<Pais> Paises)
        {

            // Serializar la lista de PAISES a JSON
            string json = JsonSerializer.Serialize(Paises, new JsonSerializerOptions { WriteIndented = true });
            // Guardar el JSON en un archivo
            string filePath = "PartidaGuardada.json";
            try
                {
                    using (StreamWriter writer = new StreamWriter(filePath))
                    {
                        writer.Write(json);
                    }

                    Console.WriteLine("PARTIDA GUARDADA.");
                }
            catch (Exception ex)
                {
                    Console.WriteLine($"Error al escribir en el archivo: {ex.Message}");
                }
        }

        public List<Pais> CargarPartida()
        {
            string filePath = "PartidaGuardada.json";
            List<Pais> Guardado = new List<Pais>();
        try
        {
            // Leer el contenido del archivo JSON
            string json;
            using (StreamReader reader = new StreamReader(filePath))
            {
                json = reader.ReadToEnd();
            }
            // Deserializar el contenido JSON a una lista de clientes
            List<Pais> Paises = JsonSerializer.Deserialize<List<Pais>>(json)!;
            if (Paises!=null)
            {
                Guardado = Paises;
            }else
            {
                Console.WriteLine("No hay paises guardados");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al leer el archivo JSON: {ex.Message}");
        }
        return Guardado;
        }
    }
}