using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Roulette.Application.Commands.CreateUser;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Roulette.Api.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-user")]
        public Task<IActionResult> CreateUserCommand([FromBody] CreateUserCommand request)
        {
            return Task.FromResult<IActionResult>(Ok(_mediator.Send(request)));
        }
    }
}