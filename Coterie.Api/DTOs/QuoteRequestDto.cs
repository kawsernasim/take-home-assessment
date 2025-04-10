using System.Collections.Generic;


namespace Coterie.Api.DTOs
{
    public class QuoteRequestDto
    {
        public string Business { get; set; } = string.Empty;

        public long Revenue { get; set; }

        public List<string> States { get; set; } = new();
    }
}

