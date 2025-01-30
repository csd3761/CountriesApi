using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CountriesApi.Application.Features.SecondLargest
{
    public class GetSecondLargestNumberQuery : IRequest<int>
    {
        public IEnumerable<int> Numbers { get; set; }
    }
}
