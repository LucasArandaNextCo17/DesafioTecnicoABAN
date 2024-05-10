using Microsoft.AspNetCore.Mvc; // Importa el espacio de nombres para los atributos y clases relacionadas con ASP.NET Core MVC.
using MVCCRUD.Models; // Importa el espacio de nombres donde se encuentra la clase ErrorViewModel.
using System.Diagnostics; // Importa el espacio de nombres para la clase Activity y Debug.

namespace MVCCRUD.Controllers
{
    public class HomeController : Controller // Define la clase HomeController que hereda de Controller.
    {
        private readonly ILogger<HomeController> _logger; // Declara una instancia privada de ILogger para registrar información de diagnóstico.

        public HomeController(ILogger<HomeController> logger) // Constructor que recibe un ILogger como parámetro.
        {
            _logger = logger; // Asigna el ILogger recibido al campo privado _logger.
        }

        public IActionResult Index() // Acción para la página de inicio.
        {
            return View(); // Devuelve una vista llamada "Index".
        }

        public IActionResult Privacy() // Acción para la página de privacidad.
        {
            return View(); // Devuelve una vista llamada "Privacy".
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Atributo para configurar la caché de la respuesta HTTP.
        public IActionResult Error() // Acción para manejar errores.
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); // Devuelve una vista de error con un modelo de ErrorViewModel.
        }
    }
}
