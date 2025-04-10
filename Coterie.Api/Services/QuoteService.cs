using System.Collections.Generic;
using System.Linq;
using Coterie.Api.Models.Configs;
using Coterie.Api.Models.Domain.Responses;
using Coterie.Api.Models.Domain.Results;
using Coterie.Api.Models.Requests;
using Coterie.Api.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Coterie.Api.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly QuoteFactorConfigs _quoteFactorConfigs;
        private readonly ILogger<QuoteService> _logger;
        public QuoteService(IOptions<QuoteFactorConfigs> quoteFactorConfigs, ILogger<QuoteService> logger)
        {
            _quoteFactorConfigs = quoteFactorConfigs.Value;
            _logger = logger;
        }
    
        public QuoteResult CalculateQuote(QuoteRequest request)
        {
            var result = new QuoteResult();
    
    
            var business = request.Business;
    
            if (!_quoteFactorConfigs.BusinessFactors.TryGetValue(business, out var businessFactor))
            {
                _logger.LogError($"No factor found for business: {business.ToString()}");
                result.Errors.Add($"Unsupported business: {business.ToString()}");
                result.IsSuccessful = false;
                return result;
            }
    
            List<PremiumResult> calculatedPremiumResults = new List<PremiumResult>();
            foreach (var state in request.States)
            {
                if (!_quoteFactorConfigs.StateFactors.TryGetValue(state, out var stateFactor))
                {
                    _logger.LogError($"No factor found for state: {state.ToString()}");
                    result.Errors.Add($"Unsupported state: {state.ToString()}");
                    result.IsSuccessful = false;
                    return result;
                }
    
                var premium = _quoteFactorConfigs.BasePremium * businessFactor * stateFactor * _quoteFactorConfigs.HazardFactor;
                calculatedPremiumResults.Add(new PremiumResult(state, premium));
            }
            
            result.IsSuccessful = calculatedPremiumResults.Any();
            if (!result.IsSuccessful)
            {
                return result;
            }
            result.Data = new QuoteResponse
            {
                Business = request.Business,
                Revenue = request.Revenue,
                Premiums = calculatedPremiumResults
            };
            
            return result;
        }
    }
}

