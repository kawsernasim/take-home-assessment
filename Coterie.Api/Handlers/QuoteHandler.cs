using System.Collections.Generic;
using Coterie.Api.DTOs;
using Coterie.Api.Mappers;
using Coterie.Api.Models.Domain.Responses;
using Coterie.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Coterie.Api.Handlers
{
    public class QuoteHandler : IQuoteHandler
    {
        private readonly IQuoteService _quoteService;

        public QuoteHandler(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        public IActionResult Handle(QuoteRequestDto requestDto)
        {
            if (!QuoteMapper.TryMapToDomain(requestDto, out var request, out var validationErrors))
            {
                return BuildErrorResponse(validationErrors);
            }

            var result = _quoteService.CalculateQuote(request);
            return result.IsSuccessful
                ? BuildSuccessResponse(result.Data!)
                : BuildErrorResponse(result.Errors);
        }
    
        private static IActionResult BuildSuccessResponse(QuoteResponse data)
        {
            return new OkObjectResult(new QuoteResponse
            {
                Business = data.Business,
                Revenue = data.Revenue,
                Premiums = data.Premiums,
            });
        }

        private static IActionResult BuildErrorResponse(List<string> errors)
        {
            return new BadRequestObjectResult(new QuoteErrorResponse
            {
                Errors = errors,
            });
        }
    }
}

