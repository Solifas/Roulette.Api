using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Roulette.Application.Commands.Payout;
using Roulette.Application.Commands.PlaceBet;
using Roulette.Application.Commands.Spin;
using Roulette.Application.Queries.ShowPreviousSpins;

namespace Roulette.Api.Controllers
{
    public class RouletteController : Controller
    {
        private readonly IMediator _mediator;

        public RouletteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("placebet")]
        public Task<IActionResult> PlaceBet(PlaceBetCommand request)
        {
            return Task.FromResult<IActionResult>(Ok(_mediator.Send(request)));
        }

        [HttpGet("spin")]
        public async Task<IActionResult> Spin(SpinCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpGet("show-previous-spins")]
        public Task<IActionResult> ShowPreviousSpins(GetPreviousSpinsQuery request)
        {
            return Task.FromResult<IActionResult>(Ok(_mediator.Send(request)));
        }

        [HttpGet("payout")]
        public Task<IActionResult> Payout(PayoutCommand request)
        {
            return Task.FromResult<IActionResult>(Ok(_mediator.Send(request)));
        }
    }
}