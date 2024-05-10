using System; // Importa el espacio de nombres para tipos fundamentales y primitivos.
using System.Collections.Generic; // Importa el espacio de nombres para las interfaces y clases relacionadas con colecciones genéricas.
using System.Linq; // Importa el espacio de nombres para las consultas LINQ.
using System.Threading.Tasks; // Importa el espacio de nombres para tipos y métodos relacionados con la programación asincrónica.
using Microsoft.AspNetCore.Mvc; // Importa el espacio de nombres para los atributos y clases relacionadas con ASP.NET Core MVC.
using Microsoft.AspNetCore.Mvc.Rendering; // Importa el espacio de nombres para las clases relacionadas con la generación de HTML.
using Microsoft.EntityFrameworkCore; // Importa el espacio de nombres para las clases relacionadas con Entity Framework Core.
using MVCCRUD.Models; // Importa el espacio de nombres donde se encuentra la clase Usuario y el contexto de base de datos MVCCRUDContext.

namespace MVCCRUD.Controllers
{
    public class UsuariosController : Controller // Define la clase UsuariosController que hereda de Controller.
    {
        private readonly MVCCRUDContext _context; // Declara una instancia privada de MVCCRUDContext para acceder a la base de datos.

        public UsuariosController(MVCCRUDContext context) // Constructor que recibe MVCCRUDContext como parámetro.
        {
            _context = context; // Asigna el MVCCRUDContext recibido al campo privado _context.
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(string buscar) // Acción para mostrar la lista de usuarios.
        {
            var usuarios = from Usuario in _context.Usuarios select Usuario; // Obtiene todos los usuarios de la base de datos.

            if (!string.IsNullOrEmpty(buscar)) // Verifica si se proporciona un término de búsqueda.
            {
                usuarios = usuarios.Where(s => s.Nombres!.Contains(buscar)); // Filtra los usuarios por nombre que contenga el término de búsqueda.
            }
            return View(await usuarios.ToListAsync()); // Devuelve una vista con la lista de usuarios.
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id) // Acción para mostrar los detalles de un usuario específico.
        {
            if (id == null || _context.Usuarios == null) // Verifica si se proporciona un ID válido y si la tabla de usuarios no está vacía.
            {
                return NotFound(); // Devuelve un resultado NotFound.
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id); // Busca el usuario con el ID proporcionado.
            if (usuario == null) // Verifica si el usuario no se encontró.
            {
                return NotFound(); // Devuelve un resultado NotFound.
            }

            return View(usuario); // Devuelve una vista con los detalles del usuario encontrado.
        }

        // GET: Usuarios/Create
        public IActionResult Create() // Acción para mostrar el formulario de creación de usuarios.
        {
            return View(); // Devuelve una vista para crear un nuevo usuario.
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombres,Apellidos,FechaNacimiento,Cuit,Domicilio,Celular,Email")] Usuario usuario) // Acción para crear un nuevo usuario.
        {
            if (ModelState.IsValid) // Verifica si el modelo es válido.
            {
                _context.Add(usuario); // Agrega el nuevo usuario al contexto.
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
                return RedirectToAction(nameof(Index)); // Redirige al usuario a la página de índice.
            }
            return View(usuario); // Devuelve la vista con el modelo de usuario en caso de error de validación.
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id) // Acción para mostrar el formulario de edición de un usuario.
        {
            if (id == null || _context.Usuarios == null) // Verifica si se proporciona un ID válido y si la tabla de usuarios no está vacía.
            {
                return NotFound(); // Devuelve un resultado NotFound.
            }

            var usuario = await _context.Usuarios.FindAsync(id); // Busca el usuario con el ID proporcionado.
            if (usuario == null) // Verifica si el usuario no se encontró.
            {
                return NotFound(); // Devuelve un resultado NotFound.
            }
            return View(usuario); // Devuelve una vista con el usuario encontrado para su edición.
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombres,Apellidos,FechaNacimiento,Cuit,Domicilio,Celular,Email")] Usuario usuario) // Acción para editar un usuario existente.
        {
            if (id != usuario.Id) // Verifica si el ID proporcionado coincide con el ID del usuario.
            {
                return NotFound(); // Devuelve un resultado NotFound.
            }

            if (ModelState.IsValid) // Verifica si el modelo es válido.
            {
                try
                {
                    _context.Update(usuario); // Actualiza el usuario en el contexto.
                    await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
                }
                catch (DbUpdateConcurrencyException) // Captura excepciones de concurrencia.
                {
                    if (!UsuarioExists(usuario.Id)) // Verifica si el usuario no existe.
                    {
                        return NotFound(); // Devuelve un resultado NotFound.
                    }
                    else
                    {
                        throw; // Relanza la excepción.
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirige al usuario a la página de índice.
            }
            return View(usuario); // Devuelve la vista con el modelo de usuario en caso de error de validación.
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id) // Acción para mostrar la confirmación de eliminación de un usuario.
        {
            if (id == null || _context.Usuarios == null) // Verifica si se proporciona un ID válido y si la tabla de usuarios no está vacía.
            {
                return NotFound(); // Devuelve un resultado NotFound.
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(m => m.Id == id); // Busca el usuario con el ID proporcionado.
            if (usuario == null) // Verifica si el usuario no se encontró.
            {
                return NotFound(); // Devuelve un resultado NotFound.
            }

            return View(usuario); // Devuelve una vista con el usuario encontrado para su eliminación.
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) // Acción para confirmar la eliminación de un usuario.
        {
            if (_context.Usuarios == null) // Verifica si la tabla de usuarios no está vacía.
            {
                return Problem("Entity set 'MVCCRUDContext.Usuarios'  is null."); // Devuelve un problema indicando que la tabla de usuarios es nula.
            }
            var usuario = await _context.Usuarios.FindAsync(id); // Busca el usuario con el ID proporcionado.
            if (usuario != null) // Verifica si el usuario se encontró.
            {
                _context.Usuarios.Remove(usuario); // Elimina el usuario del contexto.
            }

            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos.
            return RedirectToAction(nameof(Index)); // Redirige al usuario a la página de índice.
        }

        private bool UsuarioExists(int id) // Método para verificar si un usuario existe en la base de datos.
        {
            return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault(); // Devuelve true si el usuario existe, de lo contrario, devuelve false.
        }
    }
}
