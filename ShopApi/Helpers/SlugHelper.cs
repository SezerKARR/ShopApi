namespace ShopApi.Helpers;

using System.Text.RegularExpressions;

public static class SlugHelper
{
    public static string GenerateSlug(string? title)
    {
        if (string.IsNullOrEmpty(title)) return string.Empty;

        string slug = title.ToLower();

        slug = slug.Replace("ı", "i").Replace("ş", "s").Replace("ğ", "g").Replace("ü", "u")
            .Replace("ö", "o").Replace("ç", "c").Replace("ı", "i").Replace("ğ", "g")
            .Replace("ç", "c").Replace("ü", "u").Replace("ş", "s");

        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", " ").Trim(); 

        slug = slug.Replace(' ', '-');

        return slug;
    }
}