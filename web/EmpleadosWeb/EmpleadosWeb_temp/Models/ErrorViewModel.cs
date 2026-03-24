namespace EmpleadosWeb.Models
{
    // Modelo utilizado para manejar información de errores
    public class ErrorViewModel
    {
        // Identificador de la solicitud (request)
        public string? RequestId { get; set; }

        // Indica si se debe mostrar el identificador en la vista
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
