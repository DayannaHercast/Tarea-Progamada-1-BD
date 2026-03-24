namespace EmpleadosWeb.Models
{
    // Modelo que representa la entidad Empleado
    public class Empleado
    {
        // identificador único del empleado
        public int id { get; set; }
        // Nombre del empleado
        public string nombre { get; set; }

        // Salario del empleado
        public decimal salario { get; set; }
    }
}
