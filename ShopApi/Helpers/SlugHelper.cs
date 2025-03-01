using System;
using System.Text.RegularExpressions;

namespace SaplingStore.Helpers;



public static class SlugHelper
{
    public static string GenerateSlug(string title)
    {
        if (string.IsNullOrEmpty(title)) return string.Empty;

        // Başlık küçük harfe dönüştürülür
        string slug = title.ToLower();

        // Türkçe karakterleri dönüştürme (isteğe bağlı)
        slug = slug.Replace("ı", "i").Replace("ş", "s").Replace("ğ", "g").Replace("ü", "u")
            .Replace("ö", "o").Replace("ç", "c").Replace("ı", "i").Replace("ğ", "g")
            .Replace("ç", "c").Replace("ü", "u").Replace("ş", "s");

        // Özel karakterleri temizleme ve boşlukları tire ile değiştirme
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", " ").Trim(); // Fazla boşlukları tek bir boşluğa çevir

        // Boşlukları tire ile değiştirme
        slug = slug.Replace(' ', '-');

        return slug;
    }
}