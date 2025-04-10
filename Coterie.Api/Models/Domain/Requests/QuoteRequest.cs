using System.Collections.Generic;
using Coterie.Api.Models.Enums;

namespace Coterie.Api.Models.Requests
{
    public class QuoteRequest
    {
    
        public BusinessType Business { get; set; }
        public double Revenue { get; set; }
        public List<StateCode> States { get; set; }
    }
}

