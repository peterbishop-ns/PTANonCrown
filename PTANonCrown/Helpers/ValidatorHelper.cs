using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PTANonCrown.Data.Models;
namespace PTANonCrown.Helpers
{

    public static class ValidationHelpers
    {
        public static string GetStandErrors(IEnumerable<Stand> stands)
        {
            if (stands == null)
                return string.Empty;

            var list = new List<string>();

            foreach (var stand in stands)
            {
                stand.ValidateAll();

                foreach (var error in stand.GetAllErrors() ?? Enumerable.Empty<string>())
                {
                    list.Add($"Stand {stand.StandNumber}: {error}");
                }
            }

            return string.Join("\n", list);
        }

        public static string GetPlotErrors(IEnumerable<Stand> stands)
        {
            if (stands == null)
                return string.Empty;

            var list = new List<string>();

            foreach (var stand in stands)
            {
                foreach (var plot in stand.Plots ?? Enumerable.Empty<Plot>())
                {
                    plot.ValidateAll();

                    foreach (var error in plot.GetAllErrors() ?? Enumerable.Empty<string>())
                    {
                        list.Add($"Stand {stand.StandNumber} - Plot {plot.PlotNumber}: {error}");
                    }
                }
            }

            return string.Join("\n", list);
        }

        public static string GetTreeErrors(IEnumerable<Stand> stands)
        {
            if (stands == null)
                return string.Empty;
                
            var list = new List<string>();

            foreach (var stand in stands)
            {
                foreach (var plot in stand.Plots ?? Enumerable.Empty<Plot>())
                {

                    if (plot.PlotTreeLive.Count == 1) { continue;} // we remove single trees; these are placeholders that get created 
                    // when navigating to the tree tab

                    foreach (var tree in plot.PlotTreeLive ?? Enumerable.Empty<TreeLive>())
                    {
                        tree.ValidateAll();
                        var errors = tree.GetAllErrors() ?? Enumerable.Empty<string>();

                        foreach (var error in errors)
                        {
                            list.Add($"Stand {stand.StandNumber} - Plot {plot.PlotNumber} - Tree {tree.TreeNumber}: {error}");
                        }
                    }
                }
            }

            return string.Join("\n", list);
        }

        


                    public static string GetSummaryErrors(IEnumerable<Stand> stands)
        {
            if (stands == null)
                return string.Empty;

            var list = new List<string>();

            foreach (var stand in stands)
            {
                foreach (var plot in stand.Plots ?? Enumerable.Empty<Plot>())
                {
                    var missingTrees = new List<int>();

                    foreach (var tree in plot.PlotTreeLive ?? Enumerable.Empty<TreeLive>())
                    {
                        tree.ValidateAll();

                        // If tree has errors, add its number to the list
                        if ((tree.GetAllErrors() ?? Enumerable.Empty<string>()).Any())
                        {
                            missingTrees.Add(tree.TreeNumber);
                        }
                    }

                    if (missingTrees.Count > 0)
                    {
                        var treeList = string.Join(", ", missingTrees);
                        list.Add($"Cannot generate cruise summary for Plot {plot.PlotNumber}. " +
                                 $"Following tree(s) are missing required info: {treeList}");
                    }
                }
            }

            return string.Join("\n", list);
        }

        public static string GetAllErrors(IEnumerable<Stand> stands)
        {
            var allErrors = new[]
            {
                GetStandErrors(stands),
                GetPlotErrors(stands),
                GetTreeErrors(stands)
            }
            .Where(s => !string.IsNullOrWhiteSpace(s));

            return string.Join("\n", allErrors);
        }
    }
}
