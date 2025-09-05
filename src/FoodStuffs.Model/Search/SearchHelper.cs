using Lucene.Net.Documents;
using Lucene.Net.Facet;
using System.Globalization;

namespace FoodStuffs.Model.Search;

public static class SearchHelper
{
    /// <summary>
    /// Convert a string field to a DateTime, or return null if the field is empty.
    /// DateTimes should be stored using .ToString("o") to ensure they are stored in a sortable and parsable format.
    /// </summary>
    public static DateTime? GetStringFieldAsDateTimeOrNull(this Document doc, string fieldName)
    {
        var value = doc.Get(fieldName);

        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        // This should yield the same DateTime (date, time and kind) that was originally stored and won't be affected by parser's timezone.
        return DateTime.ParseExact(value, "o", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    /// <summary>
    /// Get facets from a DrillSidewaysResult and FacetsConfig
    /// </summary>
    public static List<SearchFacet> GetFacets(DrillSidewaysResult drillResult, FacetsConfig facetsConfig)
    {
        return [.. facetsConfig.DimConfigs.Keys
            .Select(fieldName => new SearchFacet
            {
                FieldName = fieldName,
                Values = [.. (drillResult.Facets
                    .GetTopChildren(int.MaxValue, fieldName)?
                    .LabelValues ?? [])
                    .Select(x => new SearchFacetValue
                    {
                        FieldValue = x.Label,
                        Count = (int)x.Value
                    })]
            })];
    }
}
