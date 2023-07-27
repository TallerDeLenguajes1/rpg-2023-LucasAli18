using Personajes;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Limites;
using PartidaJSON;
using asciiArt;
using API;
using Menu;

namespace Mecanicas
{
    public class Mecanica
    {
    HashSet<string> paisesLibres=new HashSet<string>{"Argentina","Brasil","Chile","Paraguay","Peru","Uruguay","Bolivia","Ecuador","Guyana","Colombia","Venezuela","Surinam"};
    HashSet<string> paisesOcupados=new HashSet<string>();
    public void TurnoRival(Personaje V, Personaje P, List<Pais> Paises,List<ApiClima.Clima> Climas)
    {
        //Primero seleccionar el pais incial
        Random num = new Random();
        int rand = num.Next(0,12);
        if (V.Turno==0)
        {
            V.Turno++;
            while (Paises[rand].duenio!=null)
            {
                rand = num.Next(0,11);
            }
            Paises[rand].duenio= V.Nombre;
            V.AgregarPais(Paises[rand].Nombre);
            Console.WriteLine("El pais agregado es: "+V.Paises![0]);
            V.CondicionParaGanar++;
        }else //Una vez pasado el primer turno comienza la hora de atacar
        {
            AtaqueVillano(P, V, Paises, Climas);
        }
    }
    public void Turno(Personaje P, Personaje V,List<Pais> Paises,List<ApiClima.Clima> Climas)
    {
        OpcionesMenu mostrar=new OpcionesMenu();
        Mecanica juego = new Mecanica();
        bool exit = false;
        foreach (var pais in P.Paises!)
            {
                paisesOcupados.Add(pais);
                paisesLibres.Remove(pais);
            }
            foreach (var pais in V.Paises!)
            {
                paisesOcupados.Add(pais);
                paisesLibres.Remove(pais);
            }    
            while (!exit)
            {
                if (P.CondicionParaGanar==12)
                {
                    return;
                }
                mostrar.MostrarMenuSecundario();
                string? input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                            //ELEGIR PAIS DE COMIENZO PERSONAJE Y VILLANO
                            if (P.Turno==0)
                            {
                                string? paisSeleccionado;
                                paisSeleccionado=ElegirPais(Paises);
                                P.Turno++;
                                if (paisSeleccionado!=null)
                                {
                                paisesOcupados.Add(paisSeleccionado);
                                paisesLibres.Remove(paisSeleccionado);
                                }
                                Paises.Find(pais=>pais.Nombre==paisSeleccionado)!.duenio=P.Nombre;
                                Paises.Find(pais=>pais.Nombre==paisSeleccionado)!.Soldados=3;
                                P.AgregarPais(Paises.Find(pais=>pais.Nombre==paisSeleccionado)!.Nombre);  
                                P.CondicionParaGanar++;
                            }else
                            {
                                if (P.CantidadSoldados!=0)
                                {
                                    Console.WriteLine("Ya elegiste el pais inicial, tienes soldados para agregar, elija opcion 4");
                                }else
                                {
                                    Console.WriteLine("Ya elegiste el pais inicial, no tienes soldados para agregar, elija opcion 2");
                                }
                            }
                            if (V.Turno==0)
                            {
                                Random num = new Random();
                                int rand = num.Next(0,12);
                                V.Turno=1;
                                while (Paises[rand].duenio!=null)
                                {
                                    rand = num.Next(0,11);
                                }
                                Paises[rand].duenio= V.Nombre;
                                Paises[rand].Soldados= 3;
                                V.AgregarPais(Paises[rand].Nombre);
                                paisesOcupados.Add(Paises[rand].Nombre);
                                paisesLibres.Remove(Paises[rand].Nombre);
                                V.CondicionParaGanar++;
                            }

                        break;
                    case "2":
                                if (P.Turno==0)
                                {
                                    Console.WriteLine("No elegiste el pais de inicio");
                                }else
                                {
                                    Ataque(P,V,Paises,Climas);
                                }
                        break;
                    case "3":
                                PartidaGuardada guardado = new PartidaGuardada();
                                PersonajesJson PJguardado = new PersonajesJson();
                                if (P.Turno==0)
                                    {
                                        Console.WriteLine("No elegiste el pais de inicio");
                                    }else
                                    {
                                        Console.WriteLine("Para evitar la ventaja contra el villano, debe realizar el ultimo ataque");
                                        AtaqueVillano(P,V,Paises, Climas);
                                        Console.WriteLine("Y cargar los soldados que desea");
                                        FindelTurno(P,V,Paises);
                                        PJguardado.GuardarPersonaje(P);
                                        PJguardado.GuardarVillano(V);
                                        guardado.GuardarPartida(Paises);
                                        Console.WriteLine("¡PARTIDA GUARDADA!");
                                        P.CantidadSoldados=100;
                                        exit=true;
                                    }
                                
                        break;
                    case "4":
                                if (P.Turno==0)
                                    {
                                        Console.WriteLine("No elegiste el pais de inicio");
                                    }else
                                    {
                                        Console.WriteLine("¡Termino tu turno!");
                                        exit = true;
                                    }
                        break;
                    default:
                        Console.WriteLine("Opción inválida. Por favor, selecciona una opción válida.");
                        Console.ReadLine();
                        break;
                }
        }
    }
    public void CargarSoldados(Personaje P, List<Pais> Paises)
    {
        int cantidadCarga=0;
        string? paisElegido;
        if (P.Paises!.Count()>1)
        {
            Console.WriteLine(P.Nombre+" tienes "+P.CantidadSoldados+" soldados para agregar.");

            Console.WriteLine("Tenemos una lista de los paises obtenidos para poder agregar los soldados");
            Console.WriteLine();
            foreach (var pais in P.Paises!)
            {
                Console.WriteLine("El pais conquistado es "+pais);
                Console.WriteLine();
            }
            //ELEGIMOS EL PAIS DEL CUAL PASAMOS LOS SOLDADOS
            do
            {
                Console.WriteLine("Elegir el pais en el cual agregar soldados");
                paisElegido=ElegirPais(Paises);
            } while (Paises.Find(pais=>pais.Nombre==paisElegido)!.duenio!=P.Nombre);
            do
            {
                Console.WriteLine("Si la pregunta se repite es porque excede la cantidad de soldados");
                Console.WriteLine("La cantidad de soldados es "+P.CantidadSoldados);
                Console.WriteLine("Seleccionar la cantidad de soldados a cargar");
                int.TryParse(Console.ReadLine(),out cantidadCarga);
            } while ((P.CantidadSoldados-cantidadCarga)<0);
            Paises.Find(pais=>pais.Nombre==paisElegido)!.Soldados+=cantidadCarga;
            P.CantidadSoldados=P.CantidadSoldados-cantidadCarga;
            if (P.CantidadSoldados==0)
            {
                Console.WriteLine("Colocaste todos los que tenias");
            }else if(P.CantidadSoldados<0)
            {
                Console.WriteLine("Colocaste mas de los que tenias");
            }else
            {
                int opcion;
                Console.WriteLine("Te faltaron colocar "+P.CantidadSoldados+" soldados.");
                Console.WriteLine("Deseas volver a colocar los que faltan?");
                Console.WriteLine("1.SI  2.NO");
                int.TryParse(Console.ReadLine(),out opcion);
                if (opcion==1)
                {
                    CargarSoldados(P,Paises);
                    return;
                }
                
            }
        }else
        {
            if (P.CantidadSoldados==0)
            {
                Console.WriteLine("No tienes ningun soldado");
            }else
            {
                    Paises.Find(pais=>pais.Nombre==P.Paises![0])!.Soldados+=P.CantidadSoldados;
                    P.CantidadSoldados = 0;
            }
        }
    }
    public void CargarSoldadosRival(Personaje V,List<Pais> Paises)
    {
        Random num = new Random();
        int rand = num.Next(0,12);

        if (V.Paises!=null)
        {
            if (V.Paises.Count()>1)
            {
                while (V.CantidadSoldados!=0)
                {
                    rand = num.Next(0,12);
                    while (Paises[rand].duenio!=V.Nombre)
                    {
                        rand = num.Next(0,12);
                    }
                    Paises[rand].Soldados+=1;
                    V.CantidadSoldados--;
                    Console.WriteLine("Agrego un soldado a "+Paises[rand].Nombre);
                }
            }else
            {
                Paises.Find(pais=>pais.Nombre==V.Paises[0])!.Soldados+=V.CantidadSoldados;
                V.CantidadSoldados=0;
            }
        }else
        {
            return;
        }
        
    }

    public void Ataque(Personaje P,Personaje V, List<Pais> Paises, List<ApiClima.Clima> Climas)
    {
        if (P.CondicionParaGanar==12)
        {
            return;
        }
        if (Decision() == 2)
        {
            Console.WriteLine("Termino tu ataque");
        }else
        {
        string paisAtaque;
        string paisDefensa;
        Console.WriteLine("RECUERDA QUE DEBE QUEDAR POR LO MENOS UN SOLDADO EN EL PAIS");
        Console.WriteLine("RECUERDA QUE AL ATACAR UN PAIS CON TEMPERATURAS HELADAS PIERDES UN SOLDADO");
        Console.WriteLine("Tenemos una lista de los paises obtenidos con los limites para poder pensar el ataque");
        foreach (var pais in P.Paises!)
        {
            Console.WriteLine("El pais "+Paises.Find(p=>p.Nombre==pais)!.Nombre);
            Console.WriteLine("El pais tiene "+Paises.Find(p=>p.Nombre==pais)!.Soldados+" soldados.");
            foreach (var limites in Paises.Find(p=>p.Nombre==pais)!.PaisesLimitrofes)
            {
                Console.WriteLine("Tiene el limite con "+limites);
            }
            Console.WriteLine();
        }
        Console.WriteLine("AHORA LOS PAISES ENEMIGOS: ");
        Console.WriteLine();
        foreach (var pais in V.Paises!)
        {
            Console.WriteLine("El pais "+Paises.Find(p=>p.Nombre==pais)!.Nombre);
            Console.WriteLine("El pais tiene "+Paises.Find(p=>p.Nombre==pais)!.Soldados+" soldados.");
            foreach (var limites in Paises.Find(p=>p.Nombre==pais)!.PaisesLimitrofes)
            {
                Console.WriteLine("Tiene el limite con "+limites);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine("-----------------------ELECCION DEL PAIS DESDE EL CUAL VAMOS A ATACAR------------------------");
        do
        {
            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.WriteLine("Si la pregunta se repite significa que el pais no es de su propiedad o no posee soldados suficientes");
            Console.WriteLine("Elegir el pais desde el cual desea atacar");
            paisAtaque=ElegirPais(Paises);
            if (Paises.Find(pais=>pais.Nombre==paisAtaque)!.duenio==P.Nombre && Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados<1)
            {
                Console.WriteLine("No posee soldados suficientes para un ataque");
                continue;
            }
            if (Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados==1)
            {
                Console.WriteLine("No puedes atacar si tienes 1 soldado.");
                return;
            }
           
        } while (Paises.Find(pais=>pais.Nombre==paisAtaque)!.duenio!=P.Nombre);
        Console.WriteLine("--------------------------------------------------------------------------------------");
        //VERIFICO SI EL PAIS A ATACAR ES LIMITE
        do
        {
            Console.WriteLine("-----------------------ELECCION DEL PAIS A CONQUISTAR------------------------");
            Console.WriteLine("Si la pregunta se repite significa que el pais no es limite de sus propiedades");
            Console.WriteLine("Elegir el pais al cual desea atacar");
            paisDefensa=ElegirPais(Paises);
            if (P.Paises.Contains(paisDefensa))
            {
                int opcion;
                while (P.Paises.Contains(paisDefensa))
                {
                    do
                    {
                        Console.WriteLine("El pais que elegiste es tuyo, selecciona otro");
                        Console.WriteLine("Tienes mas paises para atacar desde este pais?");
                        Console.WriteLine("1. Si 2.No");

                        if (int.TryParse(Console.ReadLine(), out opcion))
                        {
                            if (opcion == 1 || opcion == 2)
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Respuesta incorrecta. Por favor, ingrese 1 o 2.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Respuesta no aceptada. No se aceptan caracteres, ingrese un número.");
                        }

                    } while (true);
                    if (opcion==2)
                    {
                        return;
                    }
                    paisDefensa=ElegirPais(Paises);
                }
            }
            
        } while (!Paises[Paises.FindIndex(p=>p.Nombre==paisAtaque)].PaisesLimitrofes.Contains(paisDefensa));   
        Console.WriteLine("--------------------------------------------------------------------------------------");
        //VERIFICO SI EL PAIS NO TIENE DUEÑO
        if (paisesLibres.Contains(paisDefensa))
        {
            int continuar,verTemp;
            verTemp=VerificarTemperatura(paisDefensa,Paises,Climas);
            if (verTemp==0 && Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados>2)
            {
                Console.WriteLine("MALAS NOTICIAS");
                Console.WriteLine("EL PAIS QUE DESEA CONQUISTAR ESTA PASANDO POR UN CLIMA FRIO, PERDERAS 1 SOLDADO");
                Console.WriteLine("DESEAS CONTINUAR?");
                Console.WriteLine("1.SI 2.NO");
                int.TryParse(Console.ReadLine(),out continuar);
                if (continuar == 1)
                {
                    Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados-=1;
                }
            }else if(verTemp==1){
                Console.WriteLine("Hermoso dia para conquistar un pais");
            }else
            {
                Console.WriteLine("No puedes conquistar este pais debido al clima y la cantidad de soldados, espera al otro turno");
                return;
            }
            Console.WriteLine("ENHORABUENA, EL PAIS ELEGIDO PARA CONQUISTAR NO TIENE DUEÑO");
            dibujos ganaste = new dibujos();
            ganaste.victoria();
            Console.WriteLine("-----------TRASPASO DE SOLDADOS AL NUEVO PAIS----------");
            int agregar;
            Console.WriteLine("En "+paisAtaque+" tienes "+Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados+" soldados");
            Console.WriteLine("Cuantos deseas agregar a "+paisDefensa);
            int.TryParse(Console.ReadLine(),out agregar);
            //VERIFICO QUE NO SE EXCEDA DE LA CANTIDAD DE SOLDADOS QUE POSEE
            while ((Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados - agregar) <=0)
            {
                Console.WriteLine("Cuantos deseas agregar a "+paisDefensa);
                int.TryParse(Console.ReadLine(),out agregar);
                Console.WriteLine();
            }
            //GUARDAR PAIS EN NOMBRE DEL JUGADOR Y AGREGAR UN SOLDADO
            Paises.Find(pais=>pais.Nombre==paisDefensa)!.duenio=P.Nombre;
            Paises.Find(pais=>pais.Nombre==paisDefensa)!.Soldados+=agregar;
            Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados-=agregar;
            //GUARDAR NOMBRE DEL PAIS EN EL PERFIL DEL JUGADOR
            P.AgregarPais(Paises.Find(pais=>pais.Nombre==paisDefensa)!.Nombre);
            paisesOcupados.Add(paisDefensa);
            paisesLibres.Remove(paisDefensa);
            P.CondicionParaGanar++;
            Console.WriteLine("--------------------------------------------------------------");
        }else
                {
                    //VERIFICO SI EL PAIS ELEGIDO ES MIO O NO

                    int eleccion, verificarTemp;
                    //VERIFICO LA TEMPERATURA
                    verificarTemp = VerificarTemperatura(paisDefensa, Paises, Climas);

                    if (verificarTemp == 0)
                    {
                        if (Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados > 2)
                        {
                            Console.WriteLine("El ataque es en un pais helado, perderas un soldado en el camino deseas continuar?");
                            Console.WriteLine("Deseas seguir?");
                            Console.WriteLine("1.SI 2.NO");
                            int.TryParse(Console.ReadLine(), out eleccion);
                            if (eleccion == 1)
                            {
                                Console.WriteLine("Haz elegido continuar se perdio un soldado pero el ataque continua");
                                Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                                Console.WriteLine(paisDefensa + " pais de " + V.Nombre + " tiene " + Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados + " soldados");
                                Console.WriteLine(paisAtaque + " pais de " + P.Nombre + " tiene " + Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados + " soldados");
                                AtaqueDados(P, V, Paises, paisAtaque, paisDefensa);
                                return;
                            }
                            else { return; }
                        }
                        else
                        {
                            Console.WriteLine("No posees los soldados suficientes para atacar un pais helado");
                            return;
                        }
                    }


                    Console.WriteLine(paisAtaque + " pais de " + P.Nombre + " tiene " + (Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados) + " soldados");
                    Console.WriteLine(paisDefensa + " pais de " + V.Nombre + " tiene " + Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados + " soldados");
                    if (Decision() == 1)
                    {
                        AtaqueDados(P, V, Paises, paisAtaque, paisDefensa);
                    }
                }
            }
    }

        private static int Decision()
        {
            int opcion;
            do
            {
                Console.WriteLine("Desea atacar?");
                Console.WriteLine("1. Si 2.No");
                if (int.TryParse(Console.ReadLine(), out opcion))
                {
                    if (opcion == 1 || opcion == 2)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Respuesta incorrecta. Por favor, ingrese 1 o 2.");
                    }
                }
                else
                {
                    Console.WriteLine("Respuesta incorrecta. No se admiten letras, ingrese un número.");
                }
            } while (true);
            return opcion;
        }

        private static void AtaqueDados(Personaje P, Personaje V, List<Pais> Paises, string paisAtaque, string paisDefensa)
        {
            Random dado = new Random();
            int dadoAtaque1, dadoAtaque2, dadoAtaque3, dadoDefensa1, dadoDefensa2, dadoDefensa3;
            dadoAtaque1 = dado.Next(1, 7);
            dadoAtaque2 = dado.Next(1, 7);
            dadoAtaque3 = dado.Next(1, 7);
            dadoDefensa1 = dado.Next(1, 7);
            dadoDefensa2 = dado.Next(1, 7);
            dadoDefensa3 = dado.Next(1, 7);
            if (Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados > 3)
            {
                Console.WriteLine("--------EL ATAQUE SERA CON TRES DADOS-------------");
                if (Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados > 2)
                {
                    int[] valoresAtaque = { dadoAtaque1, dadoAtaque2, dadoAtaque3 };
                    Array.Sort(valoresAtaque);
                    Array.Reverse(valoresAtaque);
                    int[] valoresDefensa = { dadoDefensa1, dadoDefensa2, dadoDefensa3 };
                    Array.Sort(valoresDefensa);
                    Array.Reverse(valoresDefensa);
                    Console.WriteLine("--------DEFIENDE CON TRES DADOS-------------");
                    for (int i = 0; i < 3; i++)
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine("RONDA NUMERO " + (i+1));
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre!.ToUpper());
                        Console.WriteLine("El dado atacante es....");
                        GiroDelDado(dado);
                        Console.WriteLine(valoresAtaque[i]);
                        Thread.Sleep(1000);
                        Console.WriteLine("COMENZAMOS CON LA DEFENSA DE " + V.Nombre!.ToUpper());
                        Console.WriteLine("El dado defensor es....");
                        GiroDelDado(dado);
                        Console.WriteLine(valoresDefensa[i]);
                        if (valoresAtaque[i] <= valoresDefensa[i])
                        {
                            Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                        }
                        else
                        {
                            Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                        }
                    }
                    Console.WriteLine("---------FIN DEL ATAQUE--------------");
                }
                else if (Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados > 1)
                {
                    Console.WriteLine("--------DEFIENDE CON DOS DADOS-------------");
                    int[] valoresAtaque = { dadoAtaque1, dadoAtaque2, dadoAtaque3 };
                    Array.Sort(valoresAtaque);
                    Array.Reverse(valoresAtaque);
                    int[] valoresDefensa = { dadoDefensa2, dadoDefensa3 };
                    Array.Sort(valoresDefensa);
                    Array.Reverse(valoresDefensa);
                    Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre!.ToUpper());
                    for (int i = 0; i < 2; i++)
                    {
                        Console.WriteLine("RONDA NUMERO " + (i+1));
                        Console.WriteLine("El primer dado es....");
                        GiroDelDado(dado);
                        Thread.Sleep(3000);
                        Console.WriteLine(valoresAtaque[i]);
                        Thread.Sleep(1000);
                        Console.WriteLine("COMENZAMOS CON LA DEFENSA DE " + V.Nombre!.ToUpper());
                        Console.WriteLine("Y el dado defensor es....");
                        GiroDelDado(dado);
                        Console.WriteLine(valoresDefensa[i]);
                        if (valoresAtaque[i] <= valoresDefensa[i])
                        {
                            Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                        }
                        else
                        {
                            Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                        }
                    }
                    Console.WriteLine("---------FIN DEL ATAQUE--------------");
                }
                else
                {
                    Console.WriteLine("--------DEFIENDE CON UN DADOS-------------");
                    Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre!.ToUpper());
                    if (dadoAtaque1 > dadoDefensa3)
                    {
                        Console.WriteLine("RONDA NUMERO 1");
                        Console.WriteLine("El primer dado es....");
                        GiroDelDado(dado);
                        Console.WriteLine(dadoAtaque1);
                        Thread.Sleep(1000);
                        Console.WriteLine("COMENZAMOS CON LA DEFENSA DE " + V.Nombre!.ToUpper());
                        Console.WriteLine("Y el dado defensor es....");
                        GiroDelDado(dado);
                        Console.WriteLine(dadoDefensa3);
                        Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                    }
                    else if (dadoAtaque2 > dadoDefensa3)
                    {
                        Console.WriteLine("El dado atacante perdio la ronda 1, comenzamos con la segunda");
                        Console.WriteLine("RONDA NUMERO 2");
                        Console.WriteLine("El dado del defensor es: " + dadoDefensa3);
                        Console.WriteLine("El segundo dado del atacante es....");
                        GiroDelDado(dado);
                        Console.WriteLine(dadoAtaque2);
                        Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                    }
                    else if (dadoAtaque3 > dadoDefensa3)
                    {
                        Console.WriteLine("El dado atacante perdio la ronda 1 y 2, ahora se define");
                        Console.WriteLine("RONDA NUMERO 3");
                        Console.WriteLine("El dado del defensor es: " + dadoDefensa3);
                        Console.WriteLine("El tercer dado es....");
                        GiroDelDado(dado);
                        Console.WriteLine(dadoAtaque3);
                        Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                    }
                    else
                    {
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre.ToUpper());
                        Console.WriteLine("RONDA NUMERO 1");
                        Console.WriteLine("El primer dado es....");
                        GiroDelDado(dado);
                        Console.WriteLine(dadoAtaque1);
                        Thread.Sleep(1000);
                        Console.WriteLine("El segundo dado es....");
                        GiroDelDado(dado);
                        Console.WriteLine(dadoAtaque2);
                        Thread.Sleep(1000);
                        Console.WriteLine("El tercer dado es....");
                        GiroDelDado(dado);
                        Console.WriteLine(dadoAtaque3);
                        Thread.Sleep(1000);
                        Console.WriteLine("COMENZAMOS CON LA DEFENSA DE " + V.Nombre!.ToUpper());
                        Console.WriteLine("Y el dado defensor es....");
                        GiroDelDado(dado);
                        Console.WriteLine(dadoDefensa3);
                        Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                    }
                    Console.WriteLine("---------FIN DEL ATAQUE--------------");
                }
            }
            else if (Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados > 2)
            {
                Console.WriteLine("--------EL ATAQUE SERA CON DOS DADOS-------------");
                //ATAQUE CON 2 SOLDADOS
                if (Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados > 2)
                {
                    //ORDENO DE MAYOR A MENOR ATAQUE
                    int[] valoresAtaque = { dadoAtaque2, dadoAtaque3 };
                    Array.Sort(valoresAtaque);
                    Array.Reverse(valoresAtaque);
                    int[] valoresDefensa = { dadoDefensa1, dadoDefensa2, dadoDefensa3 };
                    Array.Sort(valoresDefensa);
                    Array.Reverse(valoresDefensa);
                    Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre!.ToUpper());
                    Console.WriteLine("--------DEFIENDE CON TRES DADOS-------------");
                    for (int i = 0; i < 2; i++)
                    {
                        Console.WriteLine("RONDA NUMERO " + (i + 1));
                        Console.WriteLine("El dado atacante es....");
                        GiroDelDado(dado);
                        Console.WriteLine(valoresAtaque[i]);
                        Thread.Sleep(1000);
                        Console.WriteLine("COMENZAMOS CON LA DEFENSA DE " + V.Nombre!.ToUpper());
                        Console.WriteLine("Y el dado defensor es....");
                        GiroDelDado(dado);
                        Console.WriteLine(valoresDefensa[i]);
                        if (valoresAtaque[i] <= valoresDefensa[i])
                        {
                            Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                        }
                        else
                        {
                            Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                        }
                    }
                    Console.WriteLine("---------FIN DEL ATAQUE--------------");
                }
                else if (Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados > 1)
                {
                    int[] valoresAtaque = { dadoAtaque2, dadoAtaque3 };
                    Array.Sort(valoresAtaque);
                    Array.Reverse(valoresAtaque);
                    int[] valoresDefensa = { dadoDefensa2, dadoDefensa3 };
                    Array.Sort(valoresDefensa);
                    Array.Reverse(valoresDefensa);
                    Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre!.ToUpper());
                    Console.WriteLine("--------DEFIENDE CON DOS DADOS-------------");
                    for (int i = 0; i < 2; i++)
                    {
                        Console.WriteLine("RONDA NUMERO " + (i + 1));
                        Console.WriteLine("El dado atacante es....");
                        GiroDelDado(dado);
                        Console.WriteLine(valoresAtaque[i]);
                        Thread.Sleep(1000);
                        Console.WriteLine("COMENZAMOS CON LA DEFENSA DE " + V.Nombre!.ToUpper());
                        Console.WriteLine("Y el dado defensor es....");
                        GiroDelDado(dado);
                        Console.WriteLine(valoresDefensa[i]);
                        if (valoresAtaque[i] <= valoresDefensa[i])
                        {
                            Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                        }
                        else
                        {
                            Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                        }
                    }
                    Console.WriteLine("---------FIN DEL ATAQUE--------------");
                }
                else
                {
                    Console.WriteLine("--------DEFIENDE CON UN DADO-------------");
                    Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre!.ToUpper());
                    Console.WriteLine("El primer dado es....");
                    GiroDelDado(dado);
                    Console.WriteLine(dadoAtaque2);
                    Thread.Sleep(1000);
                    Console.WriteLine("COMENZAMOS CON LA DEFENSA DE " + V.Nombre!.ToUpper());
                    Console.WriteLine("Y el dado defensor es....");
                    GiroDelDado(dado);
                    Console.WriteLine(dadoDefensa3);
                    if (dadoAtaque2 > dadoDefensa3)
                    {
                        Thread.Sleep(1000);
                        Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                    }
                    else if (dadoAtaque3 > dadoDefensa3)
                    {
                        Console.WriteLine("El dado atacante es....");
                        GiroDelDado(dado);
                        Console.WriteLine(dadoAtaque3);
                        Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                    }
                    else
                    {
                        Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                        Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                    }
                    Console.WriteLine("---------FIN DEL ATAQUE--------------");
                }
            }
            else if (Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados > 1)
            {
                Console.WriteLine("------EL ATAQUE SERA CON UN SOLO DADO-------");
                //ATAQUE CON 1 SOLDADOS
                if (Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados > 2)
                {
                    Console.WriteLine("--------DEFIENDE CON TRES DADOS-------------");
                    //ORDENO DE MAYOR A MENOR ATAQUE
                    int[] valoresDefensa = { dadoDefensa1, dadoDefensa2, dadoDefensa3 };
                    Array.Sort(valoresDefensa);
                    Array.Reverse(valoresDefensa);
                    Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre!.ToUpper());
                    Console.WriteLine("El valor del dado atacante es....");
                    GiroDelDado(dado);
                    Console.WriteLine(dadoAtaque3);
                    for (int i = 0; i < 3; i++)
                    {
                        Console.WriteLine("El valor del dado defensor es....");
                        GiroDelDado(dado);
                        Console.WriteLine(valoresDefensa[i]);

                        if (dadoAtaque3 <= valoresDefensa[i])
                        {
                            Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                            break;
                        }
                        else
                        {
                            Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                        }
                    }
                    Console.WriteLine("---------FIN DEL ATAQUE--------------");
                }
                else if (Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados > 1)
                {
                    int[] valoresDefensa = { dadoDefensa2, dadoDefensa3 };
                    Array.Sort(valoresDefensa);
                    Array.Reverse(valoresDefensa);
                    Console.WriteLine("--------DEFIENDE CON DOS DADOS-------------");
                    Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre!.ToUpper());
                    Console.WriteLine("El valor del dado atacante es....");
                    GiroDelDado(dado);
                    Console.WriteLine(dadoAtaque3);
                    Console.WriteLine("COMENZAMOS CON LA DEFENSA DE " + V.Nombre!.ToUpper());
                    for (int i = 0; i < 2; i++)
                    {
                        Console.WriteLine("El valor del dado defensor es....");
                        GiroDelDado(dado);
                        Console.WriteLine(valoresDefensa[i]);
                        if (dadoAtaque3 <= valoresDefensa[i])
                        {
                            Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                            break;
                        }
                        else
                        {
                            Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                        }
                    }
                    Console.WriteLine("---------FIN DEL ATAQUE--------------");
                }
                else
                {
                    Console.WriteLine("--------DEFIENDE CON UN DADOS-------------");
                    Console.WriteLine("COMENZAMOS CON EL ATAQUE DE " + P.Nombre!.ToUpper());
                    Console.WriteLine("El valor del dado atacante es....");
                    GiroDelDado(dado);
                    Console.WriteLine(dadoAtaque3);
                    Console.WriteLine("COMENZAMOS CON LA DEFENSA DE " + V.Nombre!.ToUpper());
                    Console.WriteLine("Y el dado defensor es....");
                    GiroDelDado(dado);
                    Console.WriteLine(dadoDefensa3);
                    if (dadoAtaque3 > dadoDefensa3)
                    {
                        Paises.Find(pais => pais.Nombre == paisDefensa)!.Soldados -= 1;
                    }
                    else
                    {
                        Paises.Find(pais => pais.Nombre == paisAtaque)!.Soldados -= 1;
                    }
                    Console.WriteLine("---------FIN DEL ATAQUE--------------");
                }
            }
            if (Paises.Find(pais=>pais.Nombre==paisDefensa)!.Soldados==0)
                        {
                            Console.WriteLine("El pais defensor perdio la guerra.");
                            Console.WriteLine(P.Nombre!.ToUpper()+" HA GANADO LA GUERRA Y CONQUISTO "+Paises.Find(pais=>pais.Nombre==paisDefensa)!.Nombre);
                            Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados-=1;
                            Paises.Find(pais=>pais.Nombre==paisDefensa)!.Soldados+=1;
                            Paises.Find(pais=>pais.Nombre==paisDefensa)!.duenio = P.Nombre;
                            P.AgregarPais(Paises.Find(pais=>pais.Nombre==paisDefensa)!.Nombre);
                            V.BorrarPais(Paises.Find(pais=>pais.Nombre==paisDefensa)!.Nombre);
                            P.CondicionParaGanar++;
                            V.CondicionParaGanar--;
                            return;
                        }else if (Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados==1)
                        {
                            Console.WriteLine("El pais atacante perdio la guerra.");
                            Console.WriteLine(P.Nombre!.ToUpper()+" HA PERDIDO LA GUERRA");
                            return;
                        }else
                        {
                            Console.WriteLine("El equipo de "+P.Nombre+" quedo con "+Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados+" soldados.");
                            Console.WriteLine("El equipo de "+V.Nombre+" quedo con "+Paises.Find(pais=>pais.Nombre==paisDefensa)!.Soldados+" soldados.");
                            return;
                        }
        }

    private static void GiroDelDado(Random dado)
        {
            Console.WriteLine(dado.Next(1, 7));
            Console.WriteLine("Sigue girando....");
            Console.WriteLine(dado.Next(1, 7));
            Thread.Sleep(2000);
            Console.WriteLine("Sigue...");
            Console.WriteLine(dado.Next(1, 7));
            Console.WriteLine("....");
            Thread.Sleep(1500);
            Console.WriteLine(dado.Next(1, 7));
            Console.WriteLine("....");
            Thread.Sleep(1000);
            Console.WriteLine("....");
            Thread.Sleep(3000);
        }

    public void AtaqueVillano(Personaje P, Personaje V, List<Pais> Paises, List<ApiClima.Clima> Climas)
    {
            Random num = new Random();
            int rand, lim,verificarSoldados=0;
            //PRIMERO SELECCIONA UN PAIS DESDE EL CUAL ATACA
            rand = num.Next(0,12);
            foreach (var pais in V.Paises!)
            {
                if (Paises.Find(p=>p.Nombre==pais)!.Soldados==1)
                {
                    verificarSoldados++; //SABER SI TIENE PAISES PARA ATACAR
                }
            }
            if (verificarSoldados==V.Paises.Count())
            {
                Console.WriteLine("El villano no pudo realizar un ataque debido a la falta de soldados");
                return;
            }
            while (Paises[rand].duenio!=V.Nombre)
            {
                rand = num.Next(0,12);
                if (Paises[rand].Soldados<1)
                {
                    continue;
                }
            }
            //BUSCAMOS UN PAIS PARA ATACAR DENTRO DE LOS LIMITES DEL PAIS DE ATAQUE
            lim = num.Next(0,Paises[rand].PaisesLimitrofes.Count());
            string paisAtaque=Paises[rand].Nombre;
            string paisDefensa= Paises[rand].PaisesLimitrofes[lim];
            int bandera=0;
            //verificar si los paises limitrofes son del villano
            Console.WriteLine("-----------------COMIENZA EL ATAQUE----------------------------");
            //VERIFICO SI TIENE POSIBILIDAD DE ATAQUE
            if (V.Paises.Contains(paisDefensa))
            {
                foreach (var pais in V.Paises)
                {
                    paisAtaque=pais;
                    Console.WriteLine(paisAtaque+" es el pais ataque");
                    if (Paises.Find(p=>p.Nombre==paisAtaque)!.Soldados<2)
                    {
                        Console.WriteLine(Paises.Find(p=>p.Nombre==paisAtaque)!.Nombre+" tiene menos de 2 soldados");
                    }else
                    {
                        for (int i = 0; i < (Paises.Find(p=>p.Nombre==paisAtaque)!.PaisesLimitrofes.Count()-1); i++)
                        {
                            if (V.Paises.Contains(Paises.Find(p=>p.Nombre==paisAtaque)!.PaisesLimitrofes[i]))
                            {
                                Console.WriteLine(Paises.Find(p=>p.Nombre==paisAtaque)!.PaisesLimitrofes[i]+" esta contenido");
                            }else
                            {
                                paisDefensa=Paises.Find(p=>p.Nombre==paisAtaque)!.PaisesLimitrofes[i];
                                bandera=100;
                                break;
                            }
                        }
                    }
                    if (bandera==100)
                    {
                        break;
                    }
                    bandera++;
                }
                if (bandera==V.Paises.Count())
                {
                    Console.WriteLine("El villano no tiene paises para realizar un ataque");
                    return;
                }
            }
            Console.WriteLine("PAIS ATAQUE "+paisAtaque);
            Console.WriteLine("PAIS DEFENSA "+paisDefensa);
            int verTemperatura;
            verTemperatura=VerificarTemperatura(paisDefensa,Paises,Climas);
            if (verTemperatura==0 && Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados>2)
            {
                Console.WriteLine("El pais atacante perdio un soldado en el camino");
                Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados-=1;
            }else if(verTemperatura==1){
                Console.WriteLine("Un hermoso dia para ser conquistado :'(");
            }else
            {
                Console.WriteLine("El villano no pudo conquistar el pais por el clima frio y la falta de soldados");
                return;
            }
            //PREGUNTAR SI EL PAIS SELECCIONADO ES DEL RIVAL
            if (Paises.Find(pais=>pais.Nombre==paisDefensa)!.duenio==P.Nombre)
            {
                Console.WriteLine("COMIENZA EL ATAQUE DEL VILLANO "+V.Nombre!.ToUpper()+" A "+P.Nombre!.ToUpper());
                Console.WriteLine("EL VILLANO ATACARA DESDE "+Paises[rand].Nombre+" QUE TIENE "+Paises[rand].Soldados+" soldados");
                paisAtaque = Paises[rand].Nombre;
                AtaqueDados(V,P,Paises,paisAtaque,paisDefensa);
            }else //EN EL CASO QUE EL PAIS NO TENGA DUEÑO
            {
                if (Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados>1)
                {
                    Console.WriteLine("El Villano conquisto un pais");
                    Paises.Find(pais=>pais.Nombre==paisDefensa)!.duenio=V.Nombre;
                    V.AgregarPais(Paises.Find(pais=>pais.Nombre==paisDefensa)!.Nombre);
                    Paises.Find(pais=>pais.Nombre==paisDefensa)!.Soldados+=1;
                    Paises.Find(pais=>pais.Nombre==paisAtaque)!.Soldados-=1;
                    paisesLibres.Remove(Paises.Find(pais=>pais.Nombre==paisDefensa)!.Nombre);
                    paisesOcupados.Add(Paises.Find(pais=>pais.Nombre==paisDefensa)!.Nombre);
                    V.CondicionParaGanar++;
                }else
                {
                    Console.WriteLine("El enemigo intento pero no puedo conquistar "+Paises.Find(pais=>pais.Nombre==paisDefensa)!.Nombre);
                }    
    }
}
    public string ElegirPais(List<Pais> Paises)
        {
            int sel=0;
            string? input;
            foreach (var pais in Paises)
            {
                Console.WriteLine(pais.Id + ". " + pais.Nombre);
            }
            do
            {
                Console.WriteLine("Que numero de pais desea elegir?");
                input = Console.ReadLine();
                if (int.TryParse(input, out sel))
                {
                    if (sel >= 1 && sel <= Paises.Count)
                    {
                        break;
                    }
                }
                Console.WriteLine("Opción incorrecta. Por favor, ingrese un número válido.");
            } while (true);
            return Paises.Find(pais=>pais.Id==sel)!.Nombre;
        }
    public void FindelTurno(Personaje P, Personaje V, List<Pais> Paises)
    {
        int villano=0,player=0;
        foreach (var pais in P.Paises!)
        {
            player++;
        }
        foreach (var pais in V.Paises!)
        {
            villano++;
        }
        P.Turno++;
        V.Turno++;
        P.CantidadSoldados=player;
        V.CantidadSoldados=villano;
        Console.WriteLine("A "+P.Nombre+" le quedan "+Math.Abs(P.CondicionParaGanar-Paises.Count())+" paises para conquistar.");
        Console.WriteLine("A "+V.Nombre+" le quedan "+Math.Abs(V.CondicionParaGanar-Paises.Count())+" paises para conquistar.");
        Console.WriteLine("A "+P.Nombre+" se le agrego "+player+" soldados.");
        Console.WriteLine("A "+V.Nombre+" se le agrego "+villano+" soldados.");
        CargarSoldados(P, Paises);
        CargarSoldadosRival(V, Paises);
    }
    public int VerificarTemperatura(string paisElegir, List<Pais> Paises, List<ApiClima.Clima> Climas)
    {
        int verificar;
        string capital=Paises.Find(pais=>pais.Nombre==paisElegir)!.Capital!;
        Console.WriteLine("La capital de "+paisElegir+" es "+capital);
        Console.WriteLine("La temperatura en la capital es de "+Climas.Find(pais=>pais.Ciudad==capital)!.Temperatura);
        if (Climas.Find(pais=>pais.Ciudad==capital)!.Temperatura<=10)
        {
            verificar=0;
        }else
        {
            verificar=1;
        }
        return verificar;
    }
    }
}