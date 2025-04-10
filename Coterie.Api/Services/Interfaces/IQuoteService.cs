using Coterie.Api.Models.Domain.Results;
using Coterie.Api.Models.Requests;
using Coterie.Api.Models.Responses;

namespace Coterie.Api.Services.Interfaces
{
    public interface IQuoteService
    {
        QuoteResult CalculateQuote(QuoteRequest request);
    }
}

