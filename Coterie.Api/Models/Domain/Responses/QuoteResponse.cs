using System.Collections.Generic;
using Coterie.Api.Models.Enums;
using Coterie.Api.Models.Responses;

namespace Coterie.Api.Models.Domain.Responses
{
    public class QuoteResponse : BaseSuccessResponse
    {
        public BusinessType Business { get; set; }
        public double Revenue { get; set; }
        public List<PremiumResult> Premiums { get; set; } = new List<PremiumResult>();
    }

    public record PremiumResult
    {
        public PremiumResult(StateCode state, decimal premium)
        {
            State = state;
            Premium = premium;
        }
        public StateCode State { get; init; }
        public decimal Premium { get; init; }
    }
}