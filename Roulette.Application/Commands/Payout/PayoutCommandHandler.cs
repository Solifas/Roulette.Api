using System;
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
        private readonly IRepository<User> _userAccountRepository;

        public PayoutCommandHandler(IRepository<User> userAccountRepository, IRepository<PayOut> payoutRepository)
        {
            _userAccountRepository = userAccountRepository;
            _payoutRepository = payoutRepository;
        }

        public async Task<PayoutResponse> Handle(PayoutCommand request, CancellationToken cancellationToken)
        {
            var validator = await new PayoutValidation().ValidateAsync(request, cancellationToken);
            if (validator.Errors.Count > 0) throw new ValidationException(validator);

            var getUserByIdQuery = $"SELECT * FROM Users WHERE UserName = {request.UserName}";
            var user = await _userAccountRepository.Get(getUserByIdQuery);
            if (user == null) throw new NotFoundException("User not found");
            if (user.Balance < request.Amount) throw new BadRequestException("Insufficient funds");

            var payout = new PayOut
            {
                Id = Guid.NewGuid(),
                UserId = user.UserId,
                Amount = request.Amount,
                IsRetreived = true
            };
            var insertPayoutQuery = "INSERT INTO Payouts (BetId, UserId, Amount) VALUES (@BetId, @UserId, @Amount)";
            _payoutRepository.AddAsync(insertPayoutQuery, payout);
            return new PayoutResponse
            {
                UserId = user.UserId,
                Amount = request.Amount,
                BetId = request.BetId
            };
        }
    }
}