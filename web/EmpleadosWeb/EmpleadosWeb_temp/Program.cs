var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios (MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    // Manejo de errores en producción
    app.UseExceptionHandler("/Home/Error");

    // Seguridad HTTP Strict Transport Security
    app.UseHsts();
}

// Redirección a HTTPS
app.UseHttpsRedirection();

// Archivos estáticos (css, js, imágenes)
app.UseStaticFiles();

// Habilitar enrutamiento
app.UseRouting();

// Autorización (no se usa autenticación en este proyecto)
app.UseAuthorization();

// Ruta principal del sistema
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Empleado}/{action=Index}/{id?}"
);

// Ejecutar la aplicación
app.Run();
