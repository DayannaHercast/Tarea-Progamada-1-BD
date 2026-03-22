using Microsoft.AspNetCore.Mvc;
using EmpleadosWeb.Models;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

public class EmpleadoController : Controller
{
    private readonly IConfiguration _configuration;

    public EmpleadoController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // LISTAR EMPLEADOS
    public IActionResult Index()
    {
        List<Empleado> empleados = new List<Empleado>();
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("listarEmpleados", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                empleados.Add(new Empleado
                {
                    Id = (int)reader["Id"],
                    Nombre = reader["Nombre"].ToString(),
                    Salario = (decimal)reader["Salario"]
                });
            }
        }

        return View(empleados);
    }

    // INSERTAR EMPLEADO
    [HttpPost]
    public IActionResult Insertar(string Nombre, decimal Salario)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("insertarEmpleado", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // Parámetros de entrada
            cmd.Parameters.AddWithValue("@inNombre", Nombre);
            cmd.Parameters.AddWithValue("@inSalario", Salario);

            // Parámetro de salida
            SqlParameter resultado = new SqlParameter("@outResultado", SqlDbType.Int);
            resultado.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(resultado);

            conn.Open();
            cmd.ExecuteNonQuery();

            int valorResultado = (int)resultado.Value;

            // Mensaje según el resultado de la inserción
            if (valorResultado == 0)
            {
                ViewBag.Mensaje = "El empleado ya existe.";
            }
            else
            {
                ViewBag.Mensaje = "Empleado insertado correctamente.";
            }
        }

        return RedirectToAction("Index");
    }
}