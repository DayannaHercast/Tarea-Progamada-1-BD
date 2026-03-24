using Microsoft.AspNetCore.Mvc;
using EmpleadosWeb.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

public class EmpleadoController : Controller
{
    private readonly IConfiguration _configuration;

    // Constructor para inyección de configuración
    public EmpleadoController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // LISTAR EMPLEADOS
    public IActionResult Index()
    {
        // Lista donde se almacenan los empleados
        List<Empleado> empleados = new List<Empleado>();

        // Obtener cadena de conexión
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            // Configuración del procedimiento almacenado
            SqlCommand cmd = new SqlCommand("listarEmpleados", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();

            // Ejecución del lector de datos
            SqlDataReader reader = cmd.ExecuteReader();

            // Recorrer resultados
            while (reader.Read())
            {
                empleados.Add(new Empleado
                {
                    id = (int)reader["id"],
                    nombre = reader["nombre"].ToString(),
                    salario = (decimal)reader["salario"]
                });
            }
        }

        // Retornar vista con la lista
        return View(empleados);
    }

    // INSERTAR EMPLEADO
    [HttpGet]
    public IActionResult Insertar()
    {
        return View();
    }

    // Procesar inserción
    [HttpPost]
    public IActionResult Insertar(string nombre, decimal salario)
    {
        // Validación: nombre vacío
        if (string.IsNullOrWhiteSpace(nombre))
        {
            TempData["Mensaje"] = "El nombre no puede estar vacio.";
            return RedirectToAction("Insertar");
        }

        // Validación: solo letras, espacios y guion
        foreach (char c in nombre)
        {
            if (!char.IsLetter(c) && c != ' ' && c != '-')
            {
                TempData["Mensaje"] = "El nombre solo puede contener letras y guion.";
                return RedirectToAction("Insertar");
            }
        }

        // Validación: salario positivo
        if (salario <= 0)
        {
            TempData["Mensaje"] = "El salario debe ser mayor a 0.";
            return RedirectToAction("Insertar");
        }

        // Cadena de conexión
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            // Configuración del procedimiento almacenado
            SqlCommand cmd = new SqlCommand("insertarEmpleado", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Parámetros de entrada
            cmd.Parameters.AddWithValue("@innombre", nombre);
            cmd.Parameters.AddWithValue("@insalario", salario);

            // Parámetro de salida
            SqlParameter resultado = new SqlParameter("@outResultado", SqlDbType.Int);
            resultado.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(resultado);

            conn.Open();

            // Ejecutar procedimiento
            cmd.ExecuteNonQuery();

            int valorResultado = (int)resultado.Value;

            // Validar resultado del SP
            if (valorResultado == -1)
            {
                TempData["Mensaje"] = "El empleado ya existe.";
                return RedirectToAction("Insertar");
            }
        }

        // Mensaje de éxito
        TempData["Mensaje"] = "Empleado insertado correctamente.";
        return RedirectToAction("Index");
    }
}