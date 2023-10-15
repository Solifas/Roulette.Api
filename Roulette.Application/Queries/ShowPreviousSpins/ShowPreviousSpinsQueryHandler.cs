using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Roulette.Application.Interfaces;
using Roulette.Domain;

namespace Roulette.Application.Queries.ShowPreviousSpins
{
    public class ShowPreviousSpinsQueryHandler : IRequestHandler<GetPreviousSpinsQuery, List<BetType>>
    {
        private readonly IRepository<BetHistory> _repository;
        public Task<List<BetType>> Handle(GetPreviousSpinsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}