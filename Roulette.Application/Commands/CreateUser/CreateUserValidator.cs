using FluentValidation;

namespace Roulette.Application.Commands.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required.");
            RuleFor(x => x.Balance).GreaterThanOrEqualTo(0M).WithMessage("Balance must be greater than or equal to 0.");
        }
    }
}