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
                    nombre = reader["nombre"].ToString(),
                    salario = (decimal)reader["salario"]
                });
            }
        }

        return View(empleados);
    }

    // INSERTAR EMPLEADO
    [HttpPost]
    public IActionResult Insertar(string nombre, decimal salario)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
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
            cmd.ExecuteNonQuery();

            int valorResultado = (int)resultado.Value;

            // Mensaje según el resultado de la inserción
            if (valorResultado == -1)
            {
                TempData["Mensaje"] = "El empleado ya existe.";
            }
            else
            {
                TempData["Mensaje"] = "Empleado insertado correctamente.";
            }
        }

        return RedirectToAction("Index");
    }
}