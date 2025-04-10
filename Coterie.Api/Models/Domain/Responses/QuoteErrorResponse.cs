using System.Collections.Generic;
using Coterie.Api.Models.Responses;

namespace Coterie.Api.Models.Domain.Responses
{
    public class QuoteErrorResponse : BaseExceptionResponse
    {
        public List<string> Errors { get; set; } = new List<string>();
    }
}

