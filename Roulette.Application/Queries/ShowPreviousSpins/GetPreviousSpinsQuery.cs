using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Roulette.Domain;

namespace Roulette.Application.Queries.ShowPreviousSpins
{
    public class GetPreviousSpinsQuery : IRequest<IEnumerable<SpinHistory>>
    {
        public int NumberOfSpins { get; set; }
    }
}