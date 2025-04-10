using Coterie.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Coterie.Api.Handlers
{
    public interface IQuoteHandler
    {
        IActionResult Handle(QuoteRequestDto requestDto);
    }
}

