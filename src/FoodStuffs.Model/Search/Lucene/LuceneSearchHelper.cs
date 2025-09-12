using Lucene.Net.Documents;
using Lucene.Net.Facet;
using Lucene.Net.Search;
using System.Globalization;

namespace FoodStuffs.Model.Search.Lucene;

public static class LuceneSearchHelper
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
    /// Add a filter to a DrillDownQuery for a single-value facet field if the value is not null.
    /// </summary>
    /// <param name="drillDownQuery">The DrillDownQuery to modify.</param>
    /// <param name="facetName">The name of the facet field to filter on.</param>
    /// <param name="value">The value to filter by, or null to skip adding a filter.</param>
    public static void DrillDownByValue(DrillDownQuery drillDownQuery, string facetName, string? value)
    {
        if (value is not null)
        {
            drillDownQuery.Add(facetName, value);
        }
    }

    /// <summary>
    /// Add a filter to a DrillDownQuery for a multi-value facet field.
    /// </summary>
    /// <param name="drillDownQuery">The DrillDownQuery to modify.</param>
    /// <param name="baseQuery">The base BooleanQuery to modify for AND behavior.</param>
    /// <param name="facetsConfig">The FacetsConfig used to create the DrillDownQuery.</param>
    /// <param name="facetName">The name of the facet field to filter on.</param>
    /// <param name="values">The values to filter by.</param>
    /// <param name="matchAll">If true, only return documents that match all values (AND). If false, return documents that match any value (OR).</param>
    public static void DrillDownByValues(DrillDownQuery drillDownQuery, BooleanQuery baseQuery, FacetsConfig facetsConfig, string facetName, string[] values, bool matchAll)
    {
        if (values.Length == 0)
        {
            return;
        }

        if (matchAll)
        {
            // AND - matches docs with ALL of the selected dimensions
            // For AND behavior with DrillSideways, you need to add each as a separate dimension
            // or handle this differently depending on your facet configuration
            foreach (var value in values)
            {
                var singleCategoryQuery = new DrillDownQuery(facetsConfig)
                    {
                        { facetName, value }
                    };

                baseQuery.Add(singleCategoryQuery, Occur.MUST);
            }

            return;
        }

        // OR - matches docs with ANY of the selected dimensions
        foreach (var value in values)
        {
            drillDownQuery.Add(facetName, value);
        }
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
