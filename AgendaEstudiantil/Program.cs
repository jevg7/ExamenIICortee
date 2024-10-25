using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public struct Actividades
{
    public string nombre;
    public string fecha;
    public string hora;
    public string descripcion;

    public Actividades(string Nombre, string Fecha, string Hora, string Descripcion)
    {
        nombre = Nombre;
        fecha = Fecha;
        hora = Hora;
        descripcion = Descripcion;
    }
}

public class Program
{
    private const string Agenda = "agenda.txt";
    private static List<Actividades> actividadesList = new List<Actividades>();

    public static void Main()
    {
        
        CargarActividades();

        int opt = 0;
        do
        {
            Console.WriteLine("Bienvenido a la agenda de actividades");
            Console.WriteLine("1. Registrar actividad");
            Console.WriteLine("2. Guardar actividades");
            Console.WriteLine("3. Mostrar actividades");
            Console.WriteLine("4. Buscar actividad por fecha");
            Console.WriteLine("5. Salir");
            Console.WriteLine("Seleccione una opción");
            opt = int.Parse(Console.ReadLine());

            switch (opt)
            {
                case 1:
                    RegistrarActividad();
                    Console.Clear();
                    break;
                case 2:
                    GuardarActividades();
                    Console.Clear();
                    break;
                case 3:
                    MostrarActividades();
                    Console.ReadKey();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.Clear();

                    break;
                case 4:
                    BuscarActividadPorFecha();
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("Opción no válida");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }

        } while (opt != 5);
    }

    public static void CargarActividades()
    {
        if (File.Exists(Agenda))
        {
            try
            {
                using (BinaryReader reader = new BinaryReader(File.Open(Agenda, FileMode.Open)))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        string nombre = reader.ReadString();
                        string fecha = reader.ReadString();
                        string hora = reader.ReadString();
                        string descripcion = reader.ReadString();

                        Actividades actividad = new Actividades(nombre, fecha, hora, descripcion);
                        actividadesList.Add(actividad);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar actividades: " + ex.Message);
            }
        }
    }

    public static void RegistrarActividad()
    {
        Console.WriteLine("Cuantas actividades desea registrar?");
        int n = int.Parse(Console.ReadLine());

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine("Actividad {0}", i + 1);
            Console.WriteLine("Nombre: ");
            string nombre = Console.ReadLine();

            Console.WriteLine("Fecha (DD/MM/YY): ");
            string patronFecha = @"^\d{2}/\d{2}/\d{4}$";
            Regex regex = new Regex(patronFecha);
            string fecha = Console.ReadLine();
            while (!regex.IsMatch(fecha))
            {
                Console.WriteLine("Fecha no válida, ingrese nuevamente");
                fecha = Console.ReadLine();
            }

            Console.WriteLine("Hora (HH:MM): ");
            string patronHora = @"^\d{2}:\d{2}$";
            Regex regexHora = new Regex(patronHora);
            string hora = Console.ReadLine();
            while (!regexHora.IsMatch(hora))
            {
                Console.WriteLine("Hora no válida, ingrese nuevamente");
                hora = Console.ReadLine();
            }

            Console.WriteLine("Descripción: ");
            string descripcion = Console.ReadLine();

            Actividades actividad = new Actividades(nombre, fecha, hora, descripcion);
            actividadesList.Add(actividad);
        }
    }

    public static void GuardarActividades()
    {
        Console.WriteLine("Guardando actividades...");

        
        using (BinaryWriter writer = new BinaryWriter(File.Open(Agenda, FileMode.Create)))
        {
            foreach (Actividades actividad in actividadesList)
            {
                writer.Write(actividad.nombre);
                writer.Write(actividad.fecha);
                writer.Write(actividad.hora);
                writer.Write(actividad.descripcion);
            }
        }

        Console.WriteLine("Actividades guardadas exitosamente.");
        Console.ReadKey();
    }

    public static void MostrarActividades()
    {
        if (actividadesList.Count == 0)
        {
            Console.WriteLine("No hay actividades registradas.");
            Console.ReadKey();
            return;
        }

        foreach (var actividad in actividadesList)
        {
            Console.WriteLine("Nombre: {0}", actividad.nombre);
            Console.WriteLine("Fecha: {0}", actividad.fecha);
            Console.WriteLine("Hora: {0}", actividad.hora);
            Console.WriteLine("Descripción: {0}", actividad.descripcion);
            Console.WriteLine();
        }
    }

    public static void BuscarActividadPorFecha()
    {
        Console.WriteLine("Ingrese la fecha de la actividad a buscar (DD/MM/YY)");
        string patronFecha = @"^\d{2}/\d{2}/\d{4}$";
        Regex regex = new Regex(patronFecha);
        string fecha = Console.ReadLine();
        while (!regex.IsMatch(fecha))
        {
            Console.WriteLine("Fecha no válida, ingrese nuevamente");
            fecha = Console.ReadLine();
        }

        bool found = false;
        foreach (var actividad in actividadesList)
        {
            if (actividad.fecha == fecha)
            {
                found = true;
                Console.WriteLine("Nombre: {0}", actividad.nombre);
                Console.WriteLine("Fecha: {0}", actividad.fecha);
                Console.WriteLine("Hora: {0}", actividad.hora);
                Console.WriteLine("Descripción: {0}", actividad.descripcion);
                Console.WriteLine();
            }
        }

        if (!found)
        {
            Console.WriteLine("No se encontraron actividades para la fecha ingresada.");
        }
        Console.ReadKey();
    }
}
