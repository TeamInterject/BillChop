using System.Threading.Tasks;
using BillChopBE.DataAccessLayer;
using BillChopBE.DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace BillChopBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BillChopContext context;

        public UsersController(BillChopContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser()
        {
            var newUser = await context.AddAsync(new User() { Name = "Hello" });
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
