using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Roulette.Application.Interfaces;
using Roulette.Domain;
using Roulette.Domain.Interfaces;

namespace Roulette.Application.Queries.ShowPreviousSpins
{
    public class ShowPreviousSpinsQueryHandler : IRequestHandler<GetPreviousSpinsQuery, IEnumerable<SpinHistory>>
    {
        private readonly IBetEngine _betEngine;

        public ShowPreviousSpinsQueryHandler(IBetEngine betEngine)
        {
            _betEngine = betEngine;
        }

        public async Task<IEnumerable<SpinHistory>> Handle(GetPreviousSpinsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _betEngine.ShowPreviousSpins();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("There was an error getting the previous spins", ex);
            }
        }
    }
}