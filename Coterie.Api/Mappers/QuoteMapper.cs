using System;
using System.Collections.Generic;
using System.Linq;
using Coterie.Api.DTOs;
using Coterie.Api.Models.Enums;
using Coterie.Api.Models.Requests;
using Coterie.Api.Utilities;

namespace Coterie.Api.Mappers
{
    public static class QuoteMapper
    {
        public static bool TryMapToDomain(QuoteRequestDto dto, out QuoteRequest domainModel, out List<string> errors)
        {
            errors = new();
            domainModel = new();
        
            if (!Enum.TryParse(dto.Business, true, out BusinessType business))
            {
                errors.Add($"Unsupported business type: '{dto.Business}'");
                return false;
            }
        
            var supportedStates = new List<StateCode>();
            foreach (var input in dto.States)
            {
                if (!StateMapper.TryMap(input, out var stateCode))
                {
                    errors.Add($"Unsupported state: '{input}'. Covered states: TX, FL, OH (or full names).");
                }
                else
                {
                    supportedStates.Add(stateCode);
                }
            }

            if (errors.Any()) return false;

            domainModel = new QuoteRequest
            {
                Business = business,
                Revenue = dto.Revenue,
                States = supportedStates
            };

            return true;
        }
    }
}

