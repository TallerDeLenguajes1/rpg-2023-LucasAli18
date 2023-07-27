using System.Text.Json;
namespace API
{
public class ApiClima
{
    public class PaisApi
    {
        public string? Nombre { get; set; }
        public string? Capital { get; set; }
    }

    public class Clima
    {
        public string? Ciudad { get; set; }
        public string? Descripcion { get; set; }
        public double Temperatura { get; set; }
    }

    public class DatosApi
    {
        public string? city_name { get; set; }
        public Weather? weather { get; set; }
        public double temp { get; set; }
    }

    public class Root
    {
        public List<DatosApi>? data { get; set; }
    }

    public class Weather
    {
        public string? description { get; set; }
    }

    public static async Task<List<Clima>> ObtenerClimasDePaisesAsync()
    {
        string apiKey = "c6bd1b87039e48638b6c7c546faf94f1"; // Reemplaza esto con tu propia API key de Weatherbit
        List<PaisApi> paises = RealizarListaDePaisesConCapitales();
        List<Clima> climas = new List<Clima>();
        using (HttpClient httpClient = new HttpClient())
        {
            foreach (var pais in paises)
            {
                string? ciudad = pais.Capital;
                string url = $"https://api.weatherbit.io/v2.0/current?key={apiKey}&city={Uri.EscapeDataString(ciudad!)}&lang=es";

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    //DESERIALIZO Y GUARDO TODO EN UNA LISTA
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Root weatherData = JsonSerializer.Deserialize<Root>(responseBody)!;

                    if (weatherData != null && weatherData.data != null && weatherData.data.Count > 0)
                    {
                        DatosApi climaActual = weatherData.data[0];
                        Clima clima = new Clima
                        {
                            Ciudad = climaActual.city_name!,
                            Descripcion = climaActual.weather!.description!,
                            Temperatura = climaActual.temp
                        };

                        climas.Add(clima);
                    }
                    string json = JsonSerializer.Serialize(climas, new JsonSerializerOptions { WriteIndented = true });
                    string filePath = "climas.json";
                    await File.WriteAllTextAsync(filePath, json);
                }
                catch (HttpRequestException)
                {
                    //Console.WriteLine($"Error de solicitud a la API: {ex.Message}");
                }
            }
        }
            if (climas.Count()==0)
            {
                Console.WriteLine("No se pudo acceder a la API");
                Random tem = new Random();
                foreach (var pais in paises)
                {
                    Clima clima = new Clima
                        {
                            Ciudad = pais.Capital,
                            Descripcion = "",
                            Temperatura = tem.Next(0,10)
                        };
                        if (!climas.Contains(clima))
                        {
                            climas.Add(clima);
                        }
                }
            }else
            {
                Console.WriteLine("Se accedio correctamente a la API");
            }

        return climas;
    }

    private static List<PaisApi> RealizarListaDePaisesConCapitales()
    {
        return new List<PaisApi>
        {
            new PaisApi { Nombre = "Argentina", Capital = "Buenos Aires" },
            new PaisApi { Nombre = "Brasil", Capital = "Brasília" },
            new PaisApi { Nombre = "Chile", Capital = "Santiago" },
            new PaisApi { Nombre = "Uruguay", Capital = "Montevideo" },
            new PaisApi { Nombre = "Bolivia", Capital = "La Paz" },
            new PaisApi { Nombre = "Paraguay", Capital = "Asunción" },
            new PaisApi { Nombre = "Colombia", Capital = "Bogotá" },
            new PaisApi { Nombre = "Surinam", Capital = "Paramaribo" },
            new PaisApi { Nombre = "Guyana", Capital = "Georgetown" },
            new PaisApi { Nombre = "Venezuela", Capital = "Capito" },
            new PaisApi { Nombre = "Ecuador", Capital = "Quito" },
            new PaisApi { Nombre = "Peru", Capital = "Lima" },
        };
    }
}
}
