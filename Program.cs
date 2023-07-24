using Personajes;
using Mecanicas;
using Limites;
using PartidaJSON;

        //UN ARREGLO DE LOS PAISES DISPONIBLES
        List<string> paisesLibres=new List<string>{"Argentina","Brasil","Chile","Paraguay","Peru","Uruguay","Bolivia","Ecuador","Guyana","Colombia","Venezuela"};
        List<string> paisesOcupados=new List<string>();
        List<Pais> Paises = new List<Pais>();
        Personaje player = new Personaje("");
        Personaje villano = new Personaje("");
        Mecanica jugar = new Mecanica();
        PartidaGuardada guardado = new PartidaGuardada();
        PersonajesJson PJguardado = new PersonajesJson();
        generadorPais generadorPaises = new generadorPais();
        Paises = generadorPaises.GenerarPaises();
        //EL MENU DEL JUEGO
        bool salir = false;
        while (!salir)
        {
            Console.WriteLine("╔══════════════════════════╗");
            Console.WriteLine("║        Menú Principal    ║");
            Console.WriteLine("╠══════════════════════════╣");
            Console.WriteLine("║ 1. Juego Nuevo           ║");
            Console.WriteLine("║ 2. Continuar             ║");
            Console.WriteLine("║ 3. Salir                 ║");
            Console.WriteLine("╚══════════════════════════╝");

            Console.Write("Ingrese el número de opción: ");
            string? opcion = Console.ReadLine();


            switch (opcion)
            {
                case "1":
                            villano = villano.GenerarVillano();
                            player=jugar.ComenzarJuego();
                            while (player.CondicionParaGanar!=11)
                            {
                                jugar.Turno(player,villano, Paises);
                                if (player.CantidadSoldados==100)
                                {
                                    break;
                                }
                                //FINALIZAR EL JUEGO
                                if (player.CondicionParaGanar==11)
                                {
                                    Console.WriteLine("Acabas de cumplir el objetivo, toda sudameria es tuya");
                                    Console.WriteLine("FELICIDADES "+player.Nombre.ToUpper());
                                    break;
                                }else //TURNO DEL RIVAL
                                {
                                    if (villano.CondicionParaGanar==11)
                                    {
                                        Console.WriteLine("PERDISTE TODA SUDAMERICA :'( ");
                                        Console.WriteLine("PERDISTE CONTRA "+villano.Nombre.ToUpper());
                                        break;
                                    }
                                    Console.WriteLine("¡EMPIEZA EL TURNO DEL RIVAL!");
                                    jugar.TurnoRival(villano,player, Paises);
                                }
                                Console.WriteLine("¡FIN DEL TURNO DEL RIVAL!");
                                if (player.Paises.Count()==0)
                                {
                                    Console.WriteLine("El villano logro eliminar todas tus tropas");                                    
                                    Console.WriteLine("Perdiste esta vez, juega de nuevo para lograr la conquista");                                    
                                    break;
                                }
                                if (villano.Paises.Count()==0)
                                {
                                    Console.WriteLine("Lograste eliminar todas las tropas del villano");                                    
                                    Console.WriteLine("Ganaste la partida!");                                    
                                    break;
                                }
                                jugar.FindelTurno(player,villano,Paises);

                            }
                    break;
                
                case "2":
                        player=PJguardado.CargarPersonaje();
                        villano=PJguardado.CargarVillano();
                        Paises=guardado.CargarPartida();
                        while (player.CondicionParaGanar!=11)
                        {
                            jugar.Turno(player,villano, Paises);
                            //FINALIZAR EL JUEGO
                            if (player.CondicionParaGanar==11)
                            {
                                Console.WriteLine("Acabas de cumplir el objetivo, toda sudameria es tuya");
                                Console.WriteLine("FELICIDADES "+player.Nombre.ToUpper());
                                break;
                            }else if(player.CondicionParaGanar==0)
                            {
                                Console.WriteLine("EL ENEMIGO CONQUISTO DE TODOS TUS PAISES");
                                Console.WriteLine("FELICIDADES "+villano.Nombre.ToUpper());
                                Console.WriteLine("VUELVE A COMENZAR LA AVENTURA Y VENCERAS "+player.Nombre.ToUpper());
                                break;
                            }
                            else //TURNO DEL RIVAL
                            {
                                if (villano.CondicionParaGanar==11)
                                {
                                    Console.WriteLine("PERDISTE TODA SUDAMERICA :'( ");
                                    Console.WriteLine("PERDISTE CONTRA "+villano.Nombre.ToUpper());
                                    break;
                                }else if (villano.CondicionParaGanar==0)
                                {
                                    Console.WriteLine("GANASTE");
                                    Console.WriteLine("DERROTASTE TODOS LOS PAISES DE "+villano.Nombre.ToUpper());
                                    break;
                                }
                                Console.WriteLine("¡EMPIEZA EL TURNO DEL RIVAL!");
                                jugar.TurnoRival(villano,player, Paises);
                            }
                            Console.WriteLine("¡FIN DEL TURNO DEL RIVAL!");
                            jugar.FindelTurno(player,villano,Paises);

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