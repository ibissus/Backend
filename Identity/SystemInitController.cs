using KompaniaPchor.ORM_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace KompaniaPchor.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemInitController : Controller
    {
        private RoleService _roleService { get; }
        private UserService _userService { get; }

        public SystemInitController(RoleService roleService, UserService userService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Exception), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Init([FromServices] PchorContext _context, [FromServices] IHostingEnvironment env)
        {
            Console.WriteLine("\nInicjalizacja systemu");
            Console.WriteLine("W następnych krokach zostanie usunięta baza danych i utworzona na nowo. Wszystkie dane zostaną UTRACONE");
            Console.Write("Czy kontunuować? [T/N]: ");
            var x = Console.ReadKey().KeyChar.ToString().ToLower();
            Console.WriteLine();
            string msg = "";

            try

            {
                if (x == "t" || x == "y")
                {
                    await _context.Database.EnsureDeletedAsync();
                    Console.WriteLine("Usunięto bazę danych");

                    await _context.Database.MigrateAsync();
                    //await _context.Database.EnsureCreatedAsync();
                    Console.WriteLine("Utworzono nową bazę danych");

                    await _roleService.Init();
                    Console.WriteLine("Utowrzono role systemowe");

                    await _userService.Init();
                    Console.WriteLine("Utowrzono początkowych userów");

                    msg = "System gotowy do pracy";
                }
                else
                {
                    msg = "Anulowano Inicjalizację systemu";
                }
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            Console.WriteLine(msg);
            return Ok(msg);
        }
    }
}
