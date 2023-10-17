using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Roulette.Application.Exceptions;
using Roulette.Application.Interfaces;
using Roulette.Domain;

namespace Roulette.Application.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        public readonly IRepository<User> _userRepository;

        public CreateUserCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validator = await new CreateUserValidator().ValidateAsync(request, cancellationToken);

            if (validator.Errors.Count > 0) throw new ValidationException(validator);  

            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = request.UserName,
                Balance = request.Balance
            };

            var insertUserQuery = "INSERT INTO Users (UserId, UserName, Balance) VALUES (@UserId, @UserName, @Balance)";
            try
            {
                await _userRepository.AddAsync(insertUserQuery, user);
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to create user", ex);
            }
            return;
        }
    }
}