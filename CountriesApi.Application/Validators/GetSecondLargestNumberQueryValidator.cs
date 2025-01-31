using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CountriesApi.Application.Features.SecondLargest;
using FluentValidation;

namespace CountriesApi.Application.Validators
{
    public class GetSecondLargestNumberQueryValidator : AbstractValidator<GetSecondLargestNumberQuery>
    {
        public GetSecondLargestNumberQueryValidator()
        {
            RuleFor(x => x.Numbers)
                .NotNull().WithMessage("Input Cannot be null")
                .Must(numbers => numbers.Count() >= 2).WithMessage("Input must have at least two elements.");
        }
    }
}
