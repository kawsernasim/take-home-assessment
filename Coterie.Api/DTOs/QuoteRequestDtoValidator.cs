using FluentValidation;

namespace Coterie.Api.DTOs
{
    public class QuoteRequestDtoValidator : AbstractValidator<QuoteRequestDto>
    {
        public QuoteRequestDtoValidator()
        {
            RuleFor(x => x.Business)
                .NotEmpty().WithMessage("Business is required.");

            RuleFor(x => x.States)
                .NotEmpty().WithMessage("At least one state must be provided.");
        }
    }
}

