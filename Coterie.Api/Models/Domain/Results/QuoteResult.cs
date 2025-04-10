using System.Collections.Generic;
using Coterie.Api.Models.Domain.Responses;

namespace Coterie.Api.Models.Domain.Results
{
    public class QuoteResult
    {
        public bool IsSuccessful { get; set; }

        public QuoteResponse? Data { get; set; }

        public List<string> Errors { get; set; } = new();
    }
}

