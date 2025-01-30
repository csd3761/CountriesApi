using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountriesApi.Application.Features.SecondLargest;

namespace CountriesApi.Tests.Features.SecondLargest
{
    public class GetSecondLargestNumberQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ValidArray_ReturnsSecondLargest()
        {
            var handler = new GetSecondLargestNumberQueryHandler();
            var query = new GetSecondLargestNumberQuery { Numbers = new[] { 4, 2, 6, 8, 1, 6 } };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Equal(6, result);
        }

        [Fact]
        public async Task Handle_InvalidArray_ThrowsException()
        {
            var handler = new GetSecondLargestNumberQueryHandler();
            var query = new GetSecondLargestNumberQuery { Numbers = new[] { 4 } };

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }
    }
}
