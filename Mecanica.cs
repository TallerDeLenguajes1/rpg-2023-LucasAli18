using Personajes;
using System.Text.Json;
using System.Text.Json.Serialization;
using Limites;
using PartidaJSON;

namespace Mecanicas
{
    public class Mecanica
    {
    List<string> paisesLibres=new List<string>{"Argentina","Brasil","Chile","Paraguay","Peru","Uruguay","Bolivia","Ecuador","Guyana","Colombia","Venezuela","Surinam"};
    List<string> paisesOcupados=new List<string>();
    public Personaje ComenzarJuego()
    {
        //CADA JUGADOR EMPEZARA CON 11 SOLDADOS Y DEBERA ESCOGER UN PAIS INICIAL
        //LA IDEA DEL JUEGO ES EVITAR QUE EL VILLANO SE QUEDE CON TODA SUDAMERICA
        //GANA LA PRIMERA PERSONA EN OBTENER TODOS LOS PAISES, O LA PERSONA QUE TENGA MAS PAISES CONQUISTADOS
        Console.WriteLine("Escribir el nombre del personaje");
        string? nombrePj=Console.ReadLine();
        Personaje jugador1 = new Personaje(nombrePj);
        Console.WriteLine("***************************************************");
        Console.WriteLine("*                BIENVENIDO A SHARPTEG            *");
        Console.WriteLine("***************************************************");
        Console.WriteLine();
        Console.WriteLine("EMPEZARAS CON 3 SOLDADOS");
        Console.WriteLine("DEBERÁS ESCOGER UN PAÍS INICIAL PARA COMENZAR TU CONQUISTA");
        Console.WriteLine();
        Console.WriteLine("EL OBJETIVO DEL JUEGO ES EVITAR QUE EL VILLANO SE QUEDE CON TODA SUDAMÉRICA");
        Console.WriteLine("DEBERÁS FORMAR UN EJÉRCITO, ESTRATEGIAS Y ALIANZAS PARA LOGRARLO");
        Console.WriteLine();
        Console.WriteLine("GANA LA PRIMERA PERSONA EN OBTENER TODOS LOS PAÍSES");
        Console.WriteLine("O LA PERSONA QUE TENGA MÁS PAÍSES CONQUISTADOS AL FINAL DEL JUEGO");
        Console.WriteLine();
        Console.WriteLine("¡PREPÁRATE PARA LA BATALLA Y DEMUESTRA TU HABILIDAD COMO ESTRATEGA!");
        Console.WriteLine();
        Console.WriteLine("***************************************************");

        jugador1.CantidadSoldados=0;
        jugador1.CondicionParaGanar=0;
        jugador1.Turno=0;
        return jugador1;
    }
    public void TurnoRival(Personaje V, Personaje P, List<Pais> Paises)
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
            Console.WriteLine("El pais agregado es: "+V.Paises[0]);
            V.CondicionParaGanar++;
        }else //Una vez pasado el primer turno comienza la hora de atacar
        {
            AtaqueVillano(P, V, Paises);
        }
    }
    public void Turno(Personaje P, Personaje V,List<Pais> Paises)
    {
        Mecanica juego = new Mecanica();
        bool exit = false;

            while (!exit)
            {
                Console.WriteLine("***************************************************");
                Console.WriteLine("*                 MENÚ DE OPCIONES                *");
                Console.WriteLine("***************************************************");
                Console.WriteLine();
                Console.WriteLine("1. Seleccionar Pais de inicio");
                Console.WriteLine("2. Atacar");
                Console.WriteLine("3. Guardar Partida");
                Console.WriteLine("4. Finalizar Turno");
                Console.WriteLine();
                Console.Write("Selecciona una opción: ");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":

                            if (P.Turno==0)
                            {
                                P.Turno++;
                                string? paisSeleccionado;
                                Console.WriteLine("En que pais desea empezar?");
                                paisSeleccionado=Console.ReadLine();

                                paisesOcupados.Add(paisSeleccionado);
                                paisesLibres.Remove(paisSeleccionado);;
                                Paises.Find(pais=>pais.Nombre==paisSeleccionado).duenio=P.Nombre;
                                Paises.Find(pais=>pais.Nombre==paisSeleccionado).Soldados=3;
                                P.AgregarPais(Paises.Find(pais=>pais.Nombre==paisSeleccionado).Nombre);  
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
                                if (V.Turno==0)
                                {
                                    Console.WriteLine("No elegiste el pais de inicio");
                                }else
                                {
                                Ataque(P,V,Paises);
                                }
                        break;
                    case "3":
                                Console.WriteLine("Para evitar la ventaja contra el villano, debe realizar el ultimo ataque");
                                AtaqueVillano(P,V,Paises);
                                Console.WriteLine("Y cargar los soldados que desea");
                                FindelTurno(P,V,Paises);
                                PartidaGuardada guardado = new PartidaGuardada();
                                PersonajesJson PJguardado = new PersonajesJson();
                                PJguardado.GuardarPersonaje(P);
                                PJguardado.GuardarVillano(V);
                                guardado.GuardarPartida(Paises);
                                Console.WriteLine("¡PARTIDA GUARDADA!");
                                P.CantidadSoldados=100;
                                exit=true;
                                
                        break;
                    case "4":
                        Console.WriteLine("¡Termino tu turno!");
                        exit = true;
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
        string? paisTraspaso;
        string? paisElegido;
        if (P.Paises.Count()>1)
        {
            Console.WriteLine(P.Nombre+" tienes "+P.CantidadSoldados+" soldados para agregar.");

            Console.WriteLine("Tenemos una lista de los paises obtenidos con los limites para poder pasar los soldados");
            foreach (var pais in P.Paises)
            {
                Console.WriteLine("El pais que tenes es "+pais);
                Console.WriteLine();
            }
            //ELEGIMOS EL PAIS DEL CUAL PASAMOS LOS SOLDADOS
            do
            {
                Console.WriteLine("Elegir el pais en el cual agregar soldados");
                paisElegido=Console.ReadLine();
                //FALTA VERIFICAR QUE TENGA SOLDADOS SUFICIENTES
            } while (Paises.Find(pais=>pais.Nombre==paisElegido).duenio!=P.Nombre);
            do
            {
                Console.WriteLine("Si la pregunta se repite es porque excede la cantidad de soldados");
                Console.WriteLine("La cantidad de soldados es "+P.CantidadSoldados);
                Console.WriteLine("Seleccionar la cantidad de soldados a cargar");
                int.TryParse(Console.ReadLine(),out cantidadCarga);
            } while ((P.CantidadSoldados-cantidadCarga)<0);
            Paises.Find(pais=>pais.Nombre==paisElegido).Soldados+=cantidadCarga;
            //RESTO LOS SOLDADOS
            P.CantidadSoldados=P.CantidadSoldados-cantidadCarga;
            //PREGUNTO SI DESEA SEGUIR AGREGANDO
            
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
                    Paises.Find(pais=>pais.Nombre==P.Paises[0]).Soldados+=P.CantidadSoldados;
                    P.CantidadSoldados = 0;
            }
        }
    }
    public void CargarSoldadosRival(Personaje V,List<Pais> Paises)
    {
        string? paisTraspaso;
        string? paisElegido;
        Random num = new Random();
        int rand = num.Next(0,12);

        if (V.Paises!=null)
        {
            if (V.Paises.Count()>1)
            {
                while (V.CantidadSoldados!=0)
                {
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
                Paises.Find(pais=>pais.Nombre==V.Paises[0]).Soldados+=V.CantidadSoldados;
                V.CantidadSoldados=0;
            }
        }else
        {
            return;
        }
        
    }

    public void Ataque(Personaje P,Personaje V, List<Pais> Paises)
    {
        int atacar;
        Console.WriteLine("Desea atacar?");
        Console.WriteLine("1.Si 2.No");
        int.TryParse(Console.ReadLine(),out atacar);
        if (atacar == 2)
        {
            Console.WriteLine("Termino tu turno");
        }else
        {
        string paisAtaque;
        string paisDefensa;
        Console.WriteLine("RECUERDA QUE DEBE QUEDAR POR LO MENOS UN SOLDADO EN EL PAIS");
        Console.WriteLine("Tenemos una lista de los paises obtenidos con los limites para poder pensar el ataque");
        foreach (var pais in P.Paises)
        {
            Console.WriteLine("El pais "+Paises.Find(p=>p.Nombre==pais).Nombre);
            Console.WriteLine("El pais tiene "+Paises.Find(p=>p.Nombre==pais).Soldados+" soldados.");
            foreach (var limites in Paises.Find(p=>p.Nombre==pais).PaisesLimitrofes)
            {
                Console.WriteLine("Tiene el limite con "+limites);
            }
            Console.WriteLine();
        }
        Console.WriteLine("AHORA LOS PAISES: ");
        Console.WriteLine();
        foreach (var pais in V.Paises)
        {
            Console.WriteLine("El pais "+Paises.Find(p=>p.Nombre==pais).Nombre);
            Console.WriteLine("El pais tiene "+Paises.Find(p=>p.Nombre==pais).Soldados+" soldados.");
            foreach (var limites in Paises.Find(p=>p.Nombre==pais).PaisesLimitrofes)
            {
                Console.WriteLine("Tiene el limite con "+limites);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
        //VERIFICO SI EL PAIS ES DE SU PROPIEDAD
        //LA SELECCION DE PAISES SEA POR NUMERO! CAMBIAR ESO
        do
        {
            Console.WriteLine("Si la pregunta se repite significa que el pais no es de su propiedad o no posee soldados suficientes");
            Console.WriteLine("Elegir el pais desde el cual desea atacar");
            paisAtaque=Console.ReadLine();
            if (Paises.Find(pais=>pais.Nombre==paisAtaque).duenio==P.Nombre && Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados<1)
            {
                Console.WriteLine("No posee soldados suficientes para un ataque");
                continue;
            }
            if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados==1)
            {
                Console.WriteLine("No puedes atacar si tienes 1 soldado.");
                return;
            }
        } while (Paises.Find(pais=>pais.Nombre==paisAtaque).duenio!=P.Nombre);
        
        //VERIFICO SI EL PAIS A ATACAR ES LIMITE
        do
        {
            Console.WriteLine("Si la pregunta se repite significa que el pais no es limite de sus propiedades");
            Console.WriteLine("Elegir el pais al cual desea atacar");
            paisDefensa=Console.ReadLine();
        } while (!Paises[Paises.FindIndex(p=>p.Nombre==paisAtaque)].PaisesLimitrofes.Contains(paisDefensa));

        int cantidadSoldadosPaisAtaque=Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados;
        int cantidadSoldadosPaisDefensa=Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados;
        
        //VERIFICO SI EL PAIS NO TIENE DUEÑO
        if (paisesLibres.Contains(paisDefensa))
        {
            int agregar;
            Console.WriteLine("En "+paisAtaque+" tienes "+cantidadSoldadosPaisAtaque+" soldados");
            Console.WriteLine("Cuantos deseas agregar a "+paisDefensa);
            int.TryParse(Console.ReadLine(),out agregar);
            //VERIFICO QUE NO SE EXCEDA DE LA CANTIDAD DE SOLDADOS QUE POSEE
            while ((Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados - agregar) <=0)
            {
                Console.WriteLine("Cuantos deseas agregar a "+paisDefensa);
                int.TryParse(Console.ReadLine(),out agregar);
            }
            //GUARDAR PAIS EN NOMBRE DEL JUGADOR Y AGREGAR UN SOLDADO
            Paises.Find(pais=>pais.Nombre==paisDefensa).duenio=P.Nombre;
            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados+=agregar;
            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=agregar;
            //GUARDAR NOMBRE DEL PAIS EN EL PERFIL DEL JUGADOR
            P.AgregarPais(Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
            paisesOcupados.Add(paisDefensa);
            paisesLibres.Remove(paisDefensa);
            P.CondicionParaGanar++;
            
        }else
        {
            //VERIFICO SI EL PAIS ELEGIDO ES MIO O NO
            if (P.Paises.Contains(paisDefensa))
            {   //BUSCO UN PAIS CUYO DUEÑO SEA EL VILLANO
                while (Paises.Find(pais=>pais.Nombre==paisDefensa).duenio!=V.Nombre)
                {
                    Console.WriteLine("El pais que elegiste es tuyo, selecciona otro");
                    paisDefensa=Console.ReadLine();
                }   
            }
            Console.WriteLine(paisDefensa+" pais de "+V.Nombre+" tiene "+Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados+" soldados");
            Console.WriteLine(paisAtaque+" pais de "+P.Nombre+" tiene "+Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados+" soldados");
            Console.WriteLine("Desea atacar?");
            Console.WriteLine("1.Si 2.No");
            int opcion;
            int.TryParse(Console.ReadLine(),out opcion);
            if (opcion==1)
            {
                Random dado = new Random();
                int dadoAtaque1,dadoAtaque2,dadoAtaque3,dadoDefensa1,dadoDefensa2,dadoDefensa3;
                if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados>3)
                {
                    
                    Console.WriteLine("El ataque sera con tres dados");
                    if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>2)
                    {
                            //ATAQUE CON 3 DADOS DEFENSA DE 3 DADOS
                            dadoAtaque1 = dado.Next(1,7);
                            dadoAtaque2 = dado.Next(1,7);
                            dadoAtaque3 = dado.Next(1,7);
                            dadoDefensa1 = dado.Next(1,7);
                            dadoDefensa2 = dado.Next(1,7);
                            dadoDefensa3 = dado.Next(1,7);
                            //ORDENO DE MAYOR A MENOR ATAQUE
                            int[] valoresAtaque = { dadoAtaque1, dadoAtaque2, dadoAtaque3 };
                            Array.Sort(valoresAtaque);
                            Array.Reverse(valoresAtaque);
                            int[] valoresDefensa = { dadoDefensa1, dadoDefensa2, dadoDefensa3 };
                            Array.Sort(valoresDefensa);
                            Array.Reverse(valoresDefensa);
                            Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());
                            for (int i = 0; i < 3; i++)
                            {
                                Console.WriteLine("RONDA NUMERO "+i+1);
                                Console.WriteLine("El primer dado es....");
                                Console.WriteLine(dado.Next(1,7));
                                Console.WriteLine("Sigue girando....");
                                Console.WriteLine(dado.Next(1,7));
                                Thread.Sleep(2000);
                                Console.WriteLine("....");
                                Thread.Sleep(3000);
                                Console.WriteLine(valoresAtaque[i]);
                                Thread.Sleep(1000);
                                Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+V.Nombre.ToUpper());
                                Console.WriteLine("Y el dado defensor es....");
                                Thread.Sleep(1000);
                                Console.WriteLine("....");
                                Thread.Sleep(1000);
                                Console.WriteLine("....");
                                Thread.Sleep(1000);
                                Console.WriteLine(valoresDefensa[i]);
                                Thread.Sleep(1000);
                                if (valoresAtaque[i]<=valoresDefensa[i])
                                {
                                    Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                                }else
                                {
                                    Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                                }
                            }

                    }else if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>1)
                    {
                        dadoAtaque1 = dado.Next(1,7);
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        int[] valoresAtaque = { dadoAtaque1, dadoAtaque2, dadoAtaque3 };
                        Array.Sort(valoresAtaque);
                        Array.Reverse(valoresAtaque);
                        int[] valoresDefensa = { dadoDefensa2,dadoDefensa3};
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());
                        for (int i = 0; i < 2; i++)
                        {
                            Console.WriteLine("RONDA NUMERO "+i+1);
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(valoresAtaque[i]);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+V.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(valoresDefensa[i]);
                            Thread.Sleep(1000);
                            if (valoresAtaque[i]<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            }else
                            {
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else
                    {
                        dadoAtaque1 = dado.Next(1,7);
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());
                        if(dadoAtaque1>dadoDefensa3)
                        {
                            Console.WriteLine("RONDA NUMERO 1");
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque1);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+V.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(dadoDefensa3);
                            Thread.Sleep(1000);
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            Console.WriteLine("El enemigo fue derrotado");
                        }else if (dadoAtaque2>dadoDefensa3)
                        {
                            Console.WriteLine("RONDA NUMERO 2");
                            Console.WriteLine("El dado del defensor es: "+dadoDefensa3);
                            Console.WriteLine("El segundo dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque2);
                            Thread.Sleep(1000);
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            Console.WriteLine("El enemigo fue derrotado");
                        }else if(dadoAtaque3>dadoDefensa3)
                        {
                            Console.WriteLine("RONDA NUMERO 3");
                            Console.WriteLine("El dado del defensor es: "+dadoDefensa3);
                            Console.WriteLine("El tercer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque2);
                            Thread.Sleep(1000);
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            Console.WriteLine("El enemigo fue derrotado");
                        }else
                        {
                            Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());
                            Console.WriteLine("RONDA NUMERO 1");
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque1);
                            Thread.Sleep(1000);
                            Console.WriteLine("El segundo dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque2);
                            Thread.Sleep(1000);
                            Console.WriteLine("El tercer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque3);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+V.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(dadoDefensa3);
                            Thread.Sleep(1000);
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            Console.WriteLine("El enemigo gano esta vez, no te des por vencido");
                        }
                    }

                }else if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados>2)
                {
                    Console.WriteLine("El ataque sera con dos dados");
                    //ATAQUE CON 2 SOLDADOS
                    if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>2)
                    {
                        //DADO 1 MENOR ATAQUE
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa1 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        //ORDENO DE MAYOR A MENOR ATAQUE
                        int[] valoresAtaque = {dadoAtaque2, dadoAtaque3 };
                        Array.Sort(valoresAtaque);
                        Array.Reverse(valoresAtaque);
                        int[] valoresDefensa = { dadoDefensa1, dadoDefensa2, dadoDefensa3 };
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());                        
                        for (int i = 0; i < 2; i++)
                        {
                            Console.WriteLine("RONDA NUMERO "+(i+1));
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(valoresAtaque[i]);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+V.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(valoresDefensa[i]);
                            Thread.Sleep(1000);
                            if (valoresAtaque[i]<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            }else
                            {
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>1)
                    {
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        int[] valoresAtaque = { dadoAtaque2, dadoAtaque3 };
                        Array.Sort(valoresAtaque);
                        Array.Reverse(valoresAtaque);
                        int[] valoresDefensa = { dadoDefensa2,dadoDefensa3};
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());
                        for (int i = 0; i < 2; i++)
                        {
                            Console.WriteLine("RONDA NUMERO "+(i+1));
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(valoresAtaque[i]);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+V.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(valoresDefensa[i]);
                            Thread.Sleep(1000);
                            if (valoresAtaque[i]<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            }else
                            {
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else
                    {
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());
                        Console.WriteLine("El primer dado es....");
                        Thread.Sleep(2000);
                        Console.WriteLine(dadoAtaque2);
                        Thread.Sleep(1000);
                        Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+V.Nombre.ToUpper());
                        Console.WriteLine("Y el dado defensor es....");
                        Thread.Sleep(1000);
                        Console.WriteLine("....");
                        Thread.Sleep(1000);
                        Console.WriteLine("....");
                        Thread.Sleep(1000);
                        Console.WriteLine(dadoDefensa3);
                        Thread.Sleep(1000);
                        if(dadoAtaque2>dadoDefensa3)
                        {
                            Thread.Sleep(1000);
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            Console.WriteLine("El enemigo fue derrotado");
                        }else if (dadoAtaque3>dadoDefensa3)
                        {
                            Console.WriteLine("El segundo dado es....");
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(dadoAtaque3);
                            Thread.Sleep(2000);
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            Console.WriteLine("El enemigo fue derrotado");
                        }else
                        {
                            Thread.Sleep(2000);
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            Console.WriteLine("El enemigo gano esta vez, no te des por vencido");
                        }
                    }

                }else if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados>1)
                {
                    Console.WriteLine("El ataque sera con un dado");
                    //ATAQUE CON 1 SOLDADOS
                    if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>2)
                    {
                        Console.WriteLine("El defensa utiliza 3 dados");
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa1 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        //ORDENO DE MAYOR A MENOR ATAQUE
                        int[] valoresDefensa = { dadoDefensa1, dadoDefensa2, dadoDefensa3 };
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());
                        Console.WriteLine("El valor del dado atacante es....");
                        Thread.Sleep(2000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Sigue girando....");
                        Thread.Sleep(1000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Sigue girando....");
                        Console.WriteLine("Final....");
                        Thread.Sleep(1000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        for (int i = 0; i < 3; i++)
                        {
                            Console.WriteLine("El valor del dado defensor es....");
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Console.WriteLine(valoresDefensa[i]);

                            if (dadoAtaque3<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                                Console.WriteLine("Perdiste");
                                break;
                            }else
                            {
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>1)
                    {
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        int[] valoresDefensa = { dadoDefensa2,dadoDefensa3};
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());
                        Console.WriteLine("El valor del dado atacante es....");
                        Thread.Sleep(2000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Sigue girando....");
                        Thread.Sleep(1000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Sigue girando....");
                        Console.WriteLine("Final....");
                        Thread.Sleep(1000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        for (int i = 0; i < 2; i++)
                        {
                            Console.WriteLine("El valor del dado defensor es....");
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Console.WriteLine(valoresDefensa[i]);
                            if (dadoAtaque3<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                                break;
                            }else
                            {
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else
                    {
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+P.Nombre.ToUpper());
                        Console.WriteLine("El valor del dado es....");
                        Thread.Sleep(2000);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Y el dado defensor es....");
                        Console.WriteLine("....");
                        Console.WriteLine("....");
                        Thread.Sleep(1000);
                        Console.WriteLine(dadoDefensa3);
                        if (dadoAtaque3>dadoDefensa3)
                        {
                            Console.WriteLine("El enemigo fue derrotado");
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                        }else
                        {
                            Console.WriteLine("El enemigo gano esta vez, no te des por vencido");
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                        }
                    }
                }

            }
            if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados==0)
                        {
                            Console.WriteLine("El pais defensor perdio la guerra.");
                            Console.WriteLine(P.Nombre.ToUpper()+" HA GANADO LA GUERRA Y CONQUISTO "+Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados+=1;
                            Paises.Find(pais=>pais.Nombre==paisDefensa).duenio = P.Nombre;
                            P.AgregarPais(Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
                            V.BorrarPais(Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
                            P.CondicionParaGanar++;
                            V.CondicionParaGanar--;
                            return;
                        }else if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados==1)
                        {
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados=1;
                            Console.WriteLine("El pais atacante perdio la guerra.");
                            Console.WriteLine(P.Nombre.ToUpper()+" HA PERDIDO LA GUERRA");
                            return;
                        }
        }
        }
    }
//VERIFICAR EL ATAQUE DEL RIVAL NO FUNCIONA BIEN
    public void AtaqueVillano(Personaje P, Personaje V, List<Pais> Paises)
    {
            Random num = new Random();
            int rand, lim;
            V.Turno++;
            //PRIMERO SELECCIONA UN PAIS DESDE EL CUAL ATACA
            rand = num.Next(0,12);
            //FALTA LA OPCION CUANDO NO TENGA PAIS CON MAS DE 1 SOLDADO
            //FALTA LA FUNCION DE AGREGAR PAIS
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
            Console.WriteLine("PAIS ATAQUE "+paisAtaque);
            string paisDefensa= Paises[rand].PaisesLimitrofes[lim];
            Console.WriteLine("PAIS DEFENSA "+paisDefensa);
            //PREGUNTAR SI EL PAIS SELECCIONADO ES DEL RIVAL
            if (Paises.Find(pais=>pais.Nombre==paisDefensa).duenio==P.Nombre)
            {
                Console.WriteLine("COMIENZA EL ATAQUE DEL VILLANO "+V.Nombre.ToUpper()+" A "+P.Nombre.ToUpper());
                Console.WriteLine("EL VILLANO ATACARA DESDE "+Paises[rand].Nombre+" QUE TIENE "+Paises[rand].Soldados);
                ///
                paisAtaque = Paises[rand].Nombre;
                Random dado = new Random();
                int dadoAtaque1,dadoAtaque2,dadoAtaque3,dadoDefensa1,dadoDefensa2,dadoDefensa3;
                if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados>3)
                {
                    
                    Console.WriteLine("El ataque sera con tres dados");
                    Console.WriteLine("El defensa sera con "+Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados+" dados");
                    if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>2)
                    {
                            Console.WriteLine("Defensa con 3 soldados");
                            //ATAQUE CON 3 DADOS DEFENSA DE 3 DADOS
                            dadoAtaque1 = dado.Next(1,7);
                            dadoAtaque2 = dado.Next(1,7);
                            dadoAtaque3 = dado.Next(1,7);
                            dadoDefensa1 = dado.Next(1,7);
                            dadoDefensa2 = dado.Next(1,7);
                            dadoDefensa3 = dado.Next(1,7);
                            //ORDENO DE MAYOR A MENOR ATAQUE
                            int[] valoresAtaque = { dadoAtaque1, dadoAtaque2, dadoAtaque3 };
                            Array.Sort(valoresAtaque);
                            Array.Reverse(valoresAtaque);
                            int[] valoresDefensa = { dadoDefensa1, dadoDefensa2, dadoDefensa3 };
                            Array.Sort(valoresDefensa);
                            Array.Reverse(valoresDefensa);
                            Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());
                            for (int i = 0; i < 3; i++)
                            {
                                Console.WriteLine("RONDA NUMERO "+(i+1));
                                Console.WriteLine("El primer dado es....");
                                Console.WriteLine(dado.Next(1,7));
                                Console.WriteLine("Sigue girando....");
                                Console.WriteLine(dado.Next(1,7));
                                Thread.Sleep(2000);
                                Console.WriteLine("....");
                                Thread.Sleep(3000);
                                Console.WriteLine(valoresAtaque[i]);
                                Thread.Sleep(1000);
                                Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+P.Nombre.ToUpper());
                                Console.WriteLine("Y el dado defensor es....");
                                Thread.Sleep(1000);
                                Console.WriteLine("....");
                                Thread.Sleep(1000);
                                Console.WriteLine("....");
                                Thread.Sleep(1000);
                                Console.WriteLine(valoresDefensa[i]);
                                Thread.Sleep(1000);
                                if (valoresAtaque[i]<=valoresDefensa[i])
                                {
                                    Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                                }else
                                {
                                    Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                                }
                            }

                    }else if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>1)
                    {
                        Console.WriteLine("Defensa con 2 soldados");
                        dadoAtaque1 = dado.Next(1,7);
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        int[] valoresAtaque = { dadoAtaque1, dadoAtaque2, dadoAtaque3 };
                        Array.Sort(valoresAtaque);
                        Array.Reverse(valoresAtaque);
                        int[] valoresDefensa = { dadoDefensa2,dadoDefensa3};
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());
                        for (int i = 0; i < 2; i++)
                        {
                            Console.WriteLine("RONDA NUMERO "+(i+1));
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(valoresAtaque[i]);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+P.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(valoresDefensa[i]);
                            Thread.Sleep(1000);
                            if (valoresAtaque[i]<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            }else
                            {
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else
                    {
                        Console.WriteLine("Defensa con 1 soldados");
                        dadoAtaque1 = dado.Next(1,7);
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());
                        if(dadoAtaque1>dadoDefensa3)
                        {
                            Console.WriteLine("RONDA NUMERO 1");
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque1);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+P.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(dadoDefensa3);
                            Thread.Sleep(1000);
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            Console.WriteLine("El enemigo fue derrotado");
                        }else if (dadoAtaque2>dadoDefensa3)
                        {
                            Console.WriteLine("RONDA NUMERO 2");
                            Console.WriteLine("El dado del defensor es: "+dadoDefensa3);
                            Console.WriteLine("El segundo dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque2);
                            Thread.Sleep(1000);
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            Console.WriteLine("El enemigo fue derrotado");
                        }else if(dadoAtaque3>dadoDefensa3)
                        {
                            Console.WriteLine("RONDA NUMERO 3");
                            Console.WriteLine("El dado del defensor es: "+dadoDefensa3);
                            Console.WriteLine("El tercer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque2);
                            Thread.Sleep(1000);
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            Console.WriteLine("El enemigo fue derrotado");
                        }else
                        {
                            Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());
                            Console.WriteLine("RONDA NUMERO 1");
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque1);
                            Thread.Sleep(1000);
                            Console.WriteLine("El segundo dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque2);
                            Thread.Sleep(1000);
                            Console.WriteLine("El tercer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(dadoAtaque3);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+P.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(dadoDefensa3);
                            Thread.Sleep(1000);
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            Console.WriteLine("Ganaste, el enemigo no logro vencer tu ejercito");
                        }
                    }

                }else if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados>2)
                {
                    Console.WriteLine("El ataque sera con dos dados");
                    Console.WriteLine("El pais defensor "+Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre+" tiene "+Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados+" soldados.");
                    //ATAQUE CON 2 SOLDADOS
                    if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>2)
                    {
                        Console.WriteLine("Defensa con 3 soldados");
                        //DADO 1 MENOR ATAQUE
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa1 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        //ORDENO DE MAYOR A MENOR ATAQUE
                        int[] valoresAtaque = {dadoAtaque2, dadoAtaque3 };
                        Array.Sort(valoresAtaque);
                        Array.Reverse(valoresAtaque);
                        int[] valoresDefensa = { dadoDefensa1, dadoDefensa2, dadoDefensa3 };
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());                        
                        for (int i = 0; i < 2; i++)
                        {
                            Console.WriteLine("RONDA NUMERO "+(i+1));
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(valoresAtaque[i]);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+P.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(valoresDefensa[i]);
                            Thread.Sleep(1000);
                            if (valoresAtaque[i]<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            }else
                            {
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>1)
                    {
                        Console.WriteLine("Defensa con 2 soldados");
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        int[] valoresAtaque = { dadoAtaque2, dadoAtaque3 };
                        Array.Sort(valoresAtaque);
                        Array.Reverse(valoresAtaque);
                        int[] valoresDefensa = { dadoDefensa2,dadoDefensa3};
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());
                        for (int i = 0; i < 2; i++)
                        {
                            Console.WriteLine("RONDA NUMERO "+i+1);
                            Console.WriteLine("El primer dado es....");
                            Console.WriteLine(dado.Next(1,7));
                            Console.WriteLine("Sigue girando....");
                            Console.WriteLine(dado.Next(1,7));
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(3000);
                            Console.WriteLine(valoresAtaque[i]);
                            Thread.Sleep(1000);
                            Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+P.Nombre.ToUpper());
                            Console.WriteLine("Y el dado defensor es....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine(valoresDefensa[i]);
                            Thread.Sleep(1000);
                            if (valoresAtaque[i]<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            }else
                            {
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else
                    {
                        Console.WriteLine("Defensa con 1 soldados");
                        dadoAtaque2 = dado.Next(1,7);
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());
                        Console.WriteLine("El primer dado es....");
                        Thread.Sleep(2000);
                        Console.WriteLine(dadoAtaque2);
                        Thread.Sleep(1000);
                        Console.WriteLine("El segundo dado es....");
                        Thread.Sleep(2000);
                        Console.WriteLine("....");
                        Thread.Sleep(1000);
                        Console.WriteLine("....");
                        Thread.Sleep(1000);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+P.Nombre.ToUpper());
                        Console.WriteLine("Y el dado defensor es....");
                        Thread.Sleep(1000);
                        Console.WriteLine("....");
                        Thread.Sleep(1000);
                        Console.WriteLine("....");
                        Thread.Sleep(1000);
                        Console.WriteLine(dadoDefensa3);
                        Thread.Sleep(1000);
                        //CAMBIAR POR UN FOR
                        if(dadoAtaque2>dadoDefensa3)
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine("Fuiste derrotado");
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                        }else if (dadoAtaque3>dadoDefensa3)
                        {
                            Thread.Sleep(2000);
                            Console.WriteLine("Fuiste derrotado");
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                        }else
                        {
                            Thread.Sleep(2000);
                            Console.WriteLine("Ganaste, el enemigo no logro derrotar tu ejercito");
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados=1;
                        }
                    }

                }else if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados>1)
                {
                    Console.WriteLine("El ataque sera con un dado");
                    //ATAQUE CON 1 SOLDADOS
                    if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>2)
                    {
                        Console.WriteLine("El defensa utiliza 3 dados");
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa1 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        //ORDENO DE MAYOR A MENOR ATAQUE
                        int[] valoresDefensa = { dadoDefensa1, dadoDefensa2, dadoDefensa3 };
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());
                        Console.WriteLine("El valor del dado atacante es....");
                        Thread.Sleep(2000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Sigue girando....");
                        Thread.Sleep(1000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Sigue girando....");
                        Console.WriteLine("Final....");
                        Thread.Sleep(1000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("COMENZAMOS CON LA DEFENSA DE "+P.Nombre.ToUpper());
                        for (int i = 0; i < 3; i++)
                        {
                            Console.WriteLine("RONDA "+(i+1));
                            Console.WriteLine("El valor del dado defensor es....");
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Console.WriteLine(valoresDefensa[i]);

                            if (dadoAtaque3<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                                Console.WriteLine("Ganaste, el rival no pudo matar tu ejercito");
                                break;
                            }else
                            {
                                Console.WriteLine("Perdiste, el rival derroto a tu ejercito");
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados>1)
                    {
                        Console.WriteLine("Defensa con 2 soldados");
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa2 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        int[] valoresDefensa = { dadoDefensa2,dadoDefensa3};
                        Array.Sort(valoresDefensa);
                        Array.Reverse(valoresDefensa);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());
                        Console.WriteLine("El valor del dado atacante es....");
                        Thread.Sleep(2000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Sigue girando....");
                        Thread.Sleep(1000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Sigue girando....");
                        Console.WriteLine("Final....");
                        Thread.Sleep(1000);
                        dadoAtaque3 = dado.Next(1,7);
                        Console.WriteLine(dadoAtaque3);
                        for (int i = 0; i < 2; i++)
                        {
                            Console.WriteLine("El valor del dado defensor es....");
                            Thread.Sleep(2000);
                            Console.WriteLine("....");
                            Thread.Sleep(1000);
                            Console.WriteLine("....");
                            Console.WriteLine(valoresDefensa[i]);
                            if (dadoAtaque3<=valoresDefensa[i])
                            {
                                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                                break;
                            }else
                            {
                                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                            }
                        }

                    }else
                    {
                        Console.WriteLine("Defensa con 1 soldados");
                        dadoAtaque3 = dado.Next(1,7);
                        dadoDefensa3 = dado.Next(1,7);
                        Console.WriteLine("COMENZAMOS CON EL ATAQUE DE "+V.Nombre.ToUpper());
                        Console.WriteLine("El valor del dado es....");
                        Thread.Sleep(2000);
                        Console.WriteLine(dadoAtaque3);
                        Console.WriteLine("Y el dado defensor es....");
                        Console.WriteLine("....");
                        Console.WriteLine("....");
                        Thread.Sleep(1000);
                        Console.WriteLine(dadoDefensa3);
                        if (dadoAtaque3>dadoDefensa3)
                        {
                            Console.WriteLine("Fuiste derrotado");
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados-=1;
                        }else
                        {
                            Console.WriteLine("Ganaste, el enemigo no pudo conquistar tu pais");
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                        }
                    }
                }
            if (Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados==0)
                        {
                            Console.WriteLine("El pais defensor perdio la guerra.");
                            Console.WriteLine(V.Nombre.ToUpper()+"HA GANADO LA GUERRA Y CONQUISTO "+Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
                            Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                            Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados+=1;
                            Paises.Find(pais=>pais.Nombre==paisDefensa).duenio = V.Nombre;
                            V.CondicionParaGanar++;
                            P.CondicionParaGanar--;
                            V.AgregarPais(Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
                            P.BorrarPais(Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
                        }else if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados==1)
                        {
                            
                            Console.WriteLine("El pais atacante perdio la guerra.");
                            Console.WriteLine(V.Nombre.ToUpper()+" HA PERDIDO LA GUERRA");
                        }
    }else //EN EL CASO QUE EL PAIS NO TENGA DUEÑO
    {
            if (Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados>1)
            {
                Paises.Find(pais=>pais.Nombre==paisDefensa).duenio=V.Nombre;
                V.AgregarPais(Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
                Paises.Find(pais=>pais.Nombre==paisDefensa).Soldados+=1;
                Paises.Find(pais=>pais.Nombre==paisAtaque).Soldados-=1;
                paisesLibres.Remove(Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
                paisesOcupados.Add(Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
            }else
            {
                Console.WriteLine("El enemigo intento pero no puedo conquistar "+Paises.Find(pais=>pais.Nombre==paisDefensa).Nombre);
            }   
        
    }
}

    public void FindelTurno(Personaje P, Personaje V, List<Pais> Paises)
    {
        int villano=0,player=0;
        foreach (var pais in P.Paises)
        {
            player++;
        }
        foreach (var pais in V.Paises)
        {
            villano++;
        }
        P.CantidadSoldados=player;
        V.CantidadSoldados=villano;
        Console.WriteLine("A "+P.Nombre+" se le agrego "+player+" soldados.");
        Console.WriteLine("A "+V.Nombre+" se le agrego "+villano+" soldados.");
        CargarSoldados(P, Paises);
        CargarSoldadosRival(V, Paises);
        //SUBIR LA CANTIDAD DE SOLADOS PARA EL PROXIMO TURNO DE ACUERDO A LA CANTIDAD DE PAISES QUE TIENE
        /*Console.WriteLine("   O");
          Console.WriteLine(" ¬/|\");
          Console.WriteLine("  / \"); */
    
    }

    }
}