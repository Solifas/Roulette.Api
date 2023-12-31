using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Roulette.Application.Commands.PlaceBet;

namespace Roulette.Application.Commands.PlaceBet
{
    public class PlaceBetValidator : AbstractValidator<PlaceBetCommand>
    {
        public PlaceBetValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.BetType).IsInEnum();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}