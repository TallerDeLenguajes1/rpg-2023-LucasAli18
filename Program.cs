using System.Text.Json;
using System.Text.Json.Serialization;
using Personajes;
using Mecanicas;
using Limites;
using PartidaJSON;
using Fabrica;
using Menu;
using API;

        //DEFINO LAS VARIABLES QUE VOY A UTILIZAR
        List<Pais> Paises = new List<Pais>();
        ApiClima ConsumoApi = new ApiClima();
        FabricaPJ fabrica = new FabricaPJ();
        Personaje player = new Personaje();
        Personaje villano = new Personaje();
        Mecanica jugar = new Mecanica();
        OpcionesMenu mostrar = new OpcionesMenu();
        PartidaGuardada guardado = new PartidaGuardada();
        PersonajesJson PJguardado = new PersonajesJson();
        generadorPais generadorPaises = new generadorPais();
        Paises = generadorPaises.GenerarPaises();
        List<ApiClima.Clima> Climas= await ApiClima.ObtenerClimasDePaisesAsync();
        foreach (var pais in Climas)
        {
            Console.WriteLine("En "+pais.Ciudad+" hace "+pais.Temperatura+" y tenemos "+pais.Descripcion);
        }
        //---------COMIENZA EL MENU DEL JUEGO-----------------
        bool salir = false;
        string? nombre;
        
        while (!salir)
        {
            mostrar.MostrarMenuPrincipal();
            Console.WriteLine("Ingrese el número de opción: ");
            string? opcion = Console.ReadLine();
            switch (opcion)
            {
                case "1":
                    mostrar.MostrarPresentacion();
                    do
                    {
                        Console.WriteLine("Ingrese el nombre del jugador: ");
                        nombre = Console.ReadLine()!.Trim(); // Elimino espacios en blanco
                        if (string.IsNullOrEmpty(nombre))
                        {
                            Console.WriteLine("El nombre no puede estar vacío. Escribir un nombre");
                        }
                    } while (string.IsNullOrEmpty(nombre));
                    player = fabrica.GenerarPersonaje(nombre);
                    villano = fabrica.GenerarVillano();
                    while (player.CondicionParaGanar != 12)
                    {
                        jugar.Turno(player, villano, Paises, Climas);
                        if (player.CantidadSoldados == 100)
                        {
                            break;
                        }
                        //----------------FINALIZAR EL JUEGO-------------------
                        if (player.CondicionParaGanar == 12)
                        {
                            Console.WriteLine("Acabas de cumplir el objetivo, toda sudameria es tuya");
                            Console.WriteLine("FELICIDADES " + player.Nombre!.ToUpper());
                            break;
                        }
                        else //-------------------TURNO DEL RIVAL-------------
                        {
                            if (villano.CondicionParaGanar == 12)
                            {
                                Console.WriteLine("PERDISTE TODA SUDAMERICA :'( ");
                                Console.WriteLine("PERDISTE CONTRA " + villano.Nombre!.ToUpper());
                                break;
                            }
                            Console.WriteLine("¡EMPIEZA EL TURNO DEL RIVAL!");
                            jugar.TurnoRival(villano, player, Paises, Climas);
                        }
                        Console.WriteLine("¡FIN DEL TURNO DEL RIVAL!");
                        if (player.Paises!.Count() == 0)
                        {
                            Console.WriteLine("El villano logro eliminar todas tus tropas");
                            Console.WriteLine("Perdiste esta vez, juega de nuevo para lograr la conquista");
                            break;
                        }
                        if (villano.Paises!.Count() == 0)
                        {
                            Console.WriteLine("Lograste eliminar todas las tropas del villano");
                            Console.WriteLine("Ganaste la partida!");
                            Console.WriteLine("FELICIDADES " + player.Nombre!.ToUpper());
                            break;
                        }
                        jugar.FindelTurno(player, villano, Paises);

                    }
                    break;

                case "2":
                    if (File.Exists("PartidaGuardada.json") && File.Exists("PersonajeGuardado.json") && File.Exists("VillanoGuardado.json"))
                    {
                        player = PJguardado.CargarPersonaje();
                        villano = PJguardado.CargarVillano();
                        Paises = guardado.CargarPartida();
                        while (player.CondicionParaGanar != 12)
                        {
                            jugar.Turno(player, villano, Paises, Climas);
                            //FINALIZAR EL JUEGO
                            if (player.CondicionParaGanar == 12)
                            {
                                Console.WriteLine("Acabas de cumplir el objetivo, toda sudameria es tuya");
                                Console.WriteLine("FELICIDADES " + player.Nombre!.ToUpper());
                                break;
                            }
                            else if (player.CondicionParaGanar == 0)
                            {
                                Console.WriteLine("EL ENEMIGO CONQUISTO DE TODOS TUS PAISES");
                                Console.WriteLine("FELICIDADES " + villano.Nombre!.ToUpper());
                                Console.WriteLine("VUELVE A COMENZAR LA AVENTURA Y VENCERAS " + player.Nombre!.ToUpper());
                                break;
                            }
                            else //TURNO DEL RIVAL
                            {
                                if (villano.CondicionParaGanar == 12)
                                {
                                    Console.WriteLine("PERDISTE TODA SUDAMERICA :'( ");
                                    Console.WriteLine("PERDISTE CONTRA " + villano.Nombre!.ToUpper());
                                    break;
                                }
                                else if (villano.CondicionParaGanar == 0)
                                {
                                    Console.WriteLine("GANASTE");
                                    Console.WriteLine("DERROTASTE TODOS LOS PAISES DE " + villano.Nombre!.ToUpper());
                                    break;
                                }
                                Console.WriteLine("¡EMPIEZA EL TURNO DEL RIVAL!");
                                jugar.TurnoRival(villano, player, Paises, Climas);
                            }
                            Console.WriteLine("¡FIN DEL TURNO DEL RIVAL!");
                            jugar.FindelTurno(player, villano, Paises);

                        }
                    }
                    else
                    {
                        Console.WriteLine("No existe ninguna partida guardada");
                    }
                    break;
                case "3":
                    salir = true;
                    break;
                default:
                    Console.WriteLine("Opción inválida. Por favor, ingrese un número de opción válido.");
                    Console.WriteLine("Presiona cualquier tecla para continuar...");
                    Console.ReadKey();
                    break;
            }
        }
        Console.WriteLine("¡Hasta luego!");