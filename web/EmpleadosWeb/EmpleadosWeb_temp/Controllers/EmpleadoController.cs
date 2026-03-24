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
    // MOSTRAR FORMULARIO
    [HttpGet]
    public IActionResult Insertar()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Insertar(string nombre, decimal salario)
    {
        //Validación básica
        if (string.IsNullOrWhiteSpace(nombre))
        {
            TempData["Mensaje"] = "El nombre no puede estar vacio.";
            return RedirectToAction("Insertar");
        }

        //Solo letras, espacios y guion
        foreach (char c in nombre)
        {
            if (!char.IsLetter(c) && c != ' ' && c != '-')
            {
                TempData["Mensaje"] = "El nombre solo puede contener letras y guion.";
                return RedirectToAction("Insertar");
            }
        }

        if (salario <= 0)
        {
            TempData["Mensaje"] = "El salario debe ser mayor a 0.";
            return RedirectToAction("Insertar");
        }

        //Llamada al SP
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("insertarEmpleado", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@innombre", nombre);
            cmd.Parameters.AddWithValue("@insalario", salario);

            SqlParameter resultado = new SqlParameter("@outResultado", SqlDbType.Int);
            resultado.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(resultado);

            conn.Open();
            cmd.ExecuteNonQuery();

            int valorResultado = (int)resultado.Value;

            if (valorResultado == -1)
            {
                TempData["Mensaje"] = "El empleado ya existe.";
                return RedirectToAction("Insertar");
            }
        }

        TempData["Mensaje"] = "Empleado insertado correctamente.";
        return RedirectToAction("Index");
    }
}