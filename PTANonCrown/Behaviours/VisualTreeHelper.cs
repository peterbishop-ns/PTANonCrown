using Microsoft.Maui.Controls;
using System.Collections.Generic;

public static class VisualTreeHelper
{
    /// <summary>
    /// Finds the first parent of the specified type in the visual tree.
    /// </summary>
    public static T? FindParent<T>(this Element element) where T : Element
    {
        Element? parent = element.Parent;
        while (parent != null)
        {
            if (parent is T found)
                return found;
            parent = parent.Parent;
        }
        return null;
    }

    /// <summary>
    /// Finds all descendant elements of the specified type in the visual tree.
    /// </summary>
    public static IEnumerable<T> FindDescendants<T>(this Element element) where T : Element
    {
        if (element is IVisualTreeElement visualTreeElement)
        {
            foreach (var child in visualTreeElement.GetVisualChildren())
            {
                if (child is T match)
                    yield return match;

                if (child is Element childElement)
                {
                    foreach (var descendant in childElement.FindDescendants<T>())
                    {
                        yield return descendant;
                    }
                }
            }
        }
    }
}
