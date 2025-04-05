using System.Collections.Generic;

namespace BlazorAIChatBot.Server.Helpers
{
    public static class EntityCodeMapper
    {
        private static readonly Dictionary<string, string> EntityCodeMap = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Albania", "ALB" },
            { "Andorra", "AND" },
            { "Armenia", "ARM" },
            { "Austria", "AUT" },
            { "Azerbaijan", "AZE" },
            { "Belarus", "BLR" },
            { "Belgium", "BEL" },
            { "Bosnia and Herzegovina", "BIH" },
            { "Bulgaria", "BGR" },
            { "Croatia", "HRV" },
            { "Cyprus", "CYP" },
            { "Czech Republic", "CZE" },
            { "Denmark", "DNK" },
            { "Estonia", "EST" },
            { "Finland", "FIN" },
            { "France", "FRA" },
            { "Georgia", "GEO" },
            { "Germany", "DEU" },
            { "Greece", "GRC" },
            { "Hungary", "HUN" },
            { "Iceland", "ISL" },
            { "Ireland", "IRL" },
            { "Italy", "ITA" },
            { "Kazakhstan", "KAZ" },
            { "Kosovo", "XKX" },
            { "Latvia", "LVA" },
            { "Liechtenstein", "LIE" },
            { "Lithuania", "LTU" },
            { "Luxembourg", "LUX" },
            { "Malta", "MLT" },
            { "Moldova", "MDA" },
            { "Monaco", "MCO" },
            { "Montenegro", "MNE" },
            { "Netherlands", "NLD" },
            { "North Macedonia", "MKD" },
            { "Norway", "NOR" },
            { "Poland", "POL" },
            { "Portugal", "PRT" },
            { "Romania", "ROU" },
            { "Russia", "RUS" },
            { "San Marino", "SMR" },
            { "Serbia", "SRB" },
            { "Slovakia", "SVK" },
            { "Slovenia", "SVN" },
            { "Spain", "ESP" },
            { "Sweden", "SWE" },
            { "Switzerland", "CHE" },
            { "Turkey", "TUR" },
            { "Ukraine", "UKR" },
            { "United Kingdom", "GBR" },
            { "Vatican City", "VAT" },
            { "United States", "USA" },
            { "Canada", "CAN" },
            { "Mexico", "MEX" },
            { "Brazil", "BRA" },
            { "Argentina", "ARG" },
            { "Chile", "CHL" },
            { "Colombia", "COL" },
            { "Peru", "PER" },
            { "Venezuela", "VEN" },
            { "Australia", "AUS" },
            { "New Zealand", "NZL" },
            { "Japan", "JPN" },
            { "South Korea", "KOR" },
            { "India", "IND" },
            { "China", "CHN" },
            { "Indonesia", "IDN" },
            { "Malaysia", "MYS" },
            { "Philippines", "PHL" },
            { "Singapore", "SGP" },
            { "Thailand", "THA" },
            { "Vietnam", "VNM" },
            { "South Africa", "ZAF" },
            { "Egypt", "EGY" },
            { "Saudi Arabia", "SAU" },
            { "United Arab Emirates", "ARE" }
            // Add more countries as needed
        };

        public static string? GetEntityCode(string entity)
        {
            return EntityCodeMap.TryGetValue(entity, out var code) ? code : null;
        }
    }
}