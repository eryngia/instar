using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

// Generator for insect parameter lists.
public static class InsectParameters {

    private static List<InsectParameter> highlightParameters = new List<InsectParameter> {
        InsectParameter.HornHighlight,
        InsectParameter.AntennaHighlight,
        InsectParameter.EyesHighlight,
        InsectParameter.LegHighlight,
        InsectParameter.BodySpotsHighlight,
        InsectParameter.WingSpotsHighlight,
        InsectParameter.BodyOutlineHighlight,
        InsectParameter.WingOutlineHighlight,
        InsectParameter.BodyStripesHighlight
    };

    // Generate a list of compatible insect parameters based on their weights.
    public static List<InsectParameter> Generate(int highlightCount) {
        
        var result = new List<InsectParameter>();
        var candidates = new List<(InsectParameter value, float weight, HashSet<InsectParameter> excludes)>();

        // extract metadata into candidate list
        foreach (InsectParameter value in Enum.GetValues(typeof(InsectParameter))) {
            var field = typeof(InsectParameter).GetField(value.ToString());
            var attribute = (InsectParameterAttribute)Attribute.GetCustomAttribute(field, typeof(InsectParameterAttribute));

            if (attribute != null) {
                candidates.Add((
                    value,
                    attribute.Weight,
                    new HashSet<InsectParameter>(attribute.Excludes)
                ));
            }
        }

        // cumulative exclusion set
        var excluded = new HashSet<InsectParameter>();

        // iteratively select candidates
        while (candidates.Count > 0) {

            // filter valid candidates by removing any exclusions
            var validCandidates = candidates
                .Where(c => !excluded.Contains(c.value))
                .ToList();

            if (validCandidates.Count == 0) break;

            // select a weighted random item
            float totalWeight = validCandidates.Sum(c => c.weight);
            float roll = Random.Range(0f, totalWeight);

            InsectParameter selected = default;
            float cumulative = 0f;

            foreach (var candidate in validCandidates) {
                cumulative += candidate.weight;
                if (roll <= cumulative) {
                    selected = candidate.value;
                    break;
                }
            }

            // add selected item to the result
            result.Add(selected);

            // add exclusions in both directions
            var selectedExclusions = candidates
                .First(c => c.value.Equals(selected))
                .excludes;

            excluded.UnionWith(selectedExclusions);

            var exclusionsToAdd = candidates
                .Where(c => c.excludes.Overlaps(result))
                .Select(c => c.value)
                .ToList();

            excluded.UnionWith(exclusionsToAdd);

            // remove selected item from candidates
            candidates.RemoveAll(c => c.value.Equals(selected));
        }

        RetainHighlights(result, highlightCount);

        return result;

    }

    // Remove highlights from the list of parameters until only a given number remain.
    private static void RetainHighlights(List<InsectParameter> parameters, int highlightCount) {
        var highlights = parameters.Where(p => highlightParameters.Contains(p)).ToList();
        var nonHighlights = parameters.Where(p => !highlightParameters.Contains(p)).ToList();

        if (highlights.Count > highlightCount) {
            highlights = highlights.Take(highlightCount).ToList();
        }

        parameters.Clear();
        parameters.AddRange(highlights);
        parameters.AddRange(nonHighlights);
    }

}