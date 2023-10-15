using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Roulette.Application.Exceptions;
using Roulette.Application.Interfaces;
using Roulette.Domain;

namespace Roulette.Application.Commands.Payout
{
    public class PayoutCommandHandler : IRequestHandler<PayoutCommand, PayoutResponse>
    {
        private readonly IRepository<PayOut> _payoutRepository;
        private readonly IRepository<UserAccount> _userAccountRepository;
        public async Task<PayoutResponse> Handle(PayoutCommand request, CancellationToken cancellationToken)
        {
            // validation here
            var user = await _userAccountRepository.GetByIdAsync(request.UserId);
            if (user == null) throw new NotFoundException("User not found");
            if (user.Balance < request.Amount) throw new BadRequestException("Insufficient funds");
            return new PayoutResponse
            {
                UserId = user.UserId,
                Amount = request.Amount,
                BetId = request.BetId
            };
        }
    }
}