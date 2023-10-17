using System.Collections.Generic;
using System.Linq;
using MediatR;

namespace Roulette.Application.Commands.CreateUser
{
    public class CreateUserCommand : IRequest
    {
        public string UserName { get; set; } = string.Empty;
        public decimal Balance { get; set; } = 0M;
    }
}