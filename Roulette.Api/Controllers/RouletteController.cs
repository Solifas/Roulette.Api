using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Roulette.Application.Commands.Payout;
using Roulette.Application.Commands.PlaceBet;
using Roulette.Application.Commands.Spin;
using Roulette.Application.Queries.ShowPreviousSpins;
using Roulette.Domain;

namespace Roulette.Api.Controllers
{
    [Route("api/[controller]")]
    public class RouletteController : Controller
    {
        private readonly IMediator _mediator;

        public RouletteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("placebet")]
        public Task PlaceBet([FromBody] PlaceBetCommand request)
        {
            return _mediator.Send(request);
        }

        [HttpGet("spin")]
        public async Task<BetType> Spin(SpinCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet("show-previous-spins")]
        public Task<IEnumerable<SpinHistory>> ShowPreviousSpins(GetPreviousSpinsQuery request)
        {
            return _mediator.Send(request);
        }

        [HttpPost("payout")]
        public Task<PayoutResponse> Payout([FromBody] PayoutCommand request)
        {
            return _mediator.Send(request);
        }
    }
}