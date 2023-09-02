namespace WebUI.Client.Test.Extensions;

using AngleSharp.Dom;

public static class AngleSharpExtensions
{
    public static string TrimmedText(this IElement self)
    {
        return self.TextContent.Trim();
    }
}
