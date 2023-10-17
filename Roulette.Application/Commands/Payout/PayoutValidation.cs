using FluentValidation;

namespace Roulette.Application.Commands.Payout
{
    public class PayoutValidation : AbstractValidator<PayoutCommand>
    {
        public PayoutValidation()
        {
            RuleFor(x => x.BetId).NotEmpty().WithMessage("BetId is required.");
            RuleFor(x => x.Amount).GreaterThan(0M).WithMessage("Amount must be greater than 0.");
        }
    }
}