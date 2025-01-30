using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CountriesApi.Application.Features.SecondLargest
{
    public class GetSecondLargestNumberQueryHandler : IRequestHandler<GetSecondLargestNumberQuery, int>
    {
        public Task<int> Handle(GetSecondLargestNumberQuery request, CancellationToken cancellationToken)
        {
            if (request.Numbers == null || request.Numbers.Count() < 2)
                throw new ArgumentException("Array must have at least two elements.");

            var sorted = request.Numbers.OrderByDescending(x => x).ToList();
            return Task.FromResult(sorted[1]);
        }
    }
}
