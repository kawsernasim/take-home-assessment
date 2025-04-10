using System.Collections.Generic;
using Coterie.Api.Models.Enums;

namespace Coterie.Api.Models.Configs
{
    public class QuoteFactorConfigs
    {
        public decimal BasePremium { get; set; }
        public decimal HazardFactor { get; set; }

        public Dictionary<BusinessType, decimal> BusinessFactors { get; set; } = new();
        public Dictionary<StateCode, decimal> StateFactors { get; set; } = new();
    }
}

