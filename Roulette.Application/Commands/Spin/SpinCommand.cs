using System;
using MediatR;
using Roulette.Domain;

namespace Roulette.Application.Commands.Spin
{
    public class SpinCommand : IRequest<BetType>
    {
    }
}

