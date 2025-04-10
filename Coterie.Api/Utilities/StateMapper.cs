using System;
using System.Collections.Generic;
using Coterie.Api.Models.Enums;

namespace Coterie.Api.Utilities
{
    public class StateMapper
    {
        private static readonly Dictionary<string, StateCode> StateMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "TX", StateCode.TX },
            { "Texas", StateCode.TX },
            { "FL", StateCode.FL },
            { "Florida", StateCode.FL },
            { "OH", StateCode.OH },
            { "Ohio", StateCode.OH }
        };

        public static bool TryMap(string input, out StateCode result)
        {
            return StateMap.TryGetValue(input, out result);
        }
    }
}

