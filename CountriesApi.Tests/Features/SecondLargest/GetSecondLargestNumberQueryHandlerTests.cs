using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountriesApi.Application.Features.SecondLargest;
using CountriesApi.Application.Validators;
using FluentValidation.TestHelper;

namespace CountriesApi.Tests.Features.SecondLargest
{
    public class GetSecondLargestNumberQueryHandlerTests
    {
        private readonly GetSecondLargestNumberQueryValidator _validator = new();

        [Fact]
        public void ValidQuery_ShouldNotHaveErrors()
        {
            var query = new GetSecondLargestNumberQuery { Numbers = [5, 3] };
            var result = _validator.TestValidate(query);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void ValidQuery_ShouldHaveErrors()
        {
            var query = new GetSecondLargestNumberQuery { Numbers = [5] };
            var result = _validator.TestValidate(query);
            result.ShouldHaveValidationErrorFor(x => x.Numbers);
        }

        [Fact]
        public async Task Handle_InvalidArray_ThrowsException()
        {
            var handler = new GetSecondLargestNumberQueryHandler();
            var query = new GetSecondLargestNumberQuery { Numbers = new[] { 4 } };

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidArray_ReturnsSecondLargest()
        {
            var handler = new GetSecondLargestNumberQueryHandler();
            var query = new GetSecondLargestNumberQuery { Numbers = new[] { 4, 2, 6, 8, 1, 6 } };
            var result = await handler.Handle(query, CancellationToken.None);
            Assert.Equal(6, result);
        }
    }
}
