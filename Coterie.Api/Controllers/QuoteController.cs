using Coterie.Api.DTOs;
using Coterie.Api.Handlers;
using Coterie.Api.Models.Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coterie.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuoteController : ControllerBase
    {
        private readonly IQuoteHandler _quoteHandler;

        public QuoteController(IQuoteHandler quoteHandler)
        {
            _quoteHandler = quoteHandler;
        }

        [HttpPost]
        [ProducesResponseType(typeof(QuoteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(QuoteErrorResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetQuote([FromBody] QuoteRequestDto requestDto)
        {
            return _quoteHandler.Handle(requestDto);
        }
    }
}

