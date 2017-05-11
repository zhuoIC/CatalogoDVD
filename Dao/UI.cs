using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Data;


namespace Dao
{
    public class UI
    {
        static DaoDVDMySQL dao;
        static string host = "localhost";
        static string bd = "catalogo";
        static string usr = "usr_catalogo";
        static string pwd = "12345678";
        static ObservableCollection<Dvd> listado;
        static DataTable tabla;

        public UI()
        {
            dao = new DaoDVDMySQL();
            PedirOpcion();
        }


        /// <summary>
        /// Opciones de menú principal
        /// </summary>
        static void Menu()
        {
            Console.WriteLine("\nCATALOGO DE DVDs - Menú de opciones");
            Console.WriteLine("==================================="); ;
            Console.WriteLine("(0) Conectar con la BADA");
            Console.WriteLine("(1) Desconectar con la BADA");
            Console.WriteLine("(2) Listar DVD por código[PA]");
            Console.WriteLine("(3) Listar DVD por codigo [SQL Directo]");
            Console.WriteLine("(4) Listar DVD tabla");
            Console.WriteLine("(5) Eliminar DVD por codigo [SQL directo]");
            Console.WriteLine("(Q) Fin del programa");

            Console.WriteLine("Opción? ");
        }

        static void PedirOpcion()
        {
            ConsoleKeyInfo tecla;
            do
            {
                Menu();
                tecla = Console.ReadKey();
                try
                {
                    Console.WriteLine();
                    switch ((char)tecla.Key)
                    {
                        case '0':
                            if (!dao.Conectado())
                            {
                                if (dao.Conectar(host, bd, usr, pwd))
                                    Console.WriteLine("Conexión exitosa a la base de datos: " + bd); 
                            }
                            else
                            {
                                Console.WriteLine("Ya hay una conexión establecida");
                            }
                            break;
                        case '1':
                            if (dao.Conectado())
                            {
                                dao.Desconectar();
                                Console.WriteLine("Desconexión exitosa a la base de datos: " + bd);   
                            }
                            else
                            {
                                Console.WriteLine("No hay una conexión establecida");
                            }
                            break;
                        case '2':
                            listado = dao.SeleccionarPA(null);
                            MostrarListado();
                            break;

                        case '3':
                            listado = dao.Seleccionar(null);
                            MostrarListado();
                            break;

                        case '4':
                            tabla = dao.SeleccionarTB(null);
                            MostrarListado();
                            break;

                        case '5':
                            Console.Write("Codigo de DVD a eliminar: ");
                            string codigo = Console.ReadLine();
                            Console.WriteLine(dao.Borrar(codigo) + " filas borradas");
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ha ocurrido un error: " + e.Message);
                }
            } while (tecla.Key != ConsoleKey.Q);
        }

        /// MostrarListado
        /// <summary>
        /// Recorrido de la llamada a un procedimiento
        /// </summary>
        static void MostrarListado()
        {
            if (dao.Conectado())
            {
                foreach (Dvd unDvd in listado)
                {
                    Console.WriteLine();
                    Console.WriteLine(unDvd.ToString());
                } 
            }
            else
            {
                Console.WriteLine("No hay una conexión válida");
            }
        }
    }
}
