namespace SaplingStore.Helpers;

public enum FormTypes {
    Image,
    
}
public class FormManager {
    
    public static Dictionary<FormTypes, List<string>> AllowedExtensions = new Dictionary<FormTypes, List<string>>()
    {

        {
            FormTypes.Image,new List<string>(){".jpg",".png",".bmp",".gif",".ico"}
        }
    };
    public static string Save(IFormFile file, string folder, FormTypes type) {
        string rootPath = Directory.GetCurrentDirectory() + "/wwwroot";
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Invalid file");
        }

        // Dosya uzantısını kontrol et
        string extension = Path.GetExtension(file.FileName).ToLower();
        if (!AllowedExtensions[type].Contains(extension))
        {
            throw new ArgumentException("Unsupported file type");
        }

        // Klasör yolunu oluştur (eğer yoksa)
        string directoryPath = Path.Combine(rootPath, folder);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        // Dosya adını düzenle (çok uzun isimleri kısalt)
        string newFileName = Guid.NewGuid().ToString() + 
                             (file.FileName.Length > 64 ? file.FileName.Substring(file.FileName.Length - 64) : file.FileName);

        string filePath = Path.Combine(directoryPath, newFileName);

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return filePath;
    }
    public static List<string> Save(IList<IFormFile> files, string folder, FormTypes type) {
        List<string> Urls = new List<string>();
        foreach (var file in files)
        {
            Urls.Add(Save(file, folder, type));
        }
        return Urls;
    }


    public static void Delete(string rootPath, string folder, string fileName)
    {
        string path = Path.Combine(rootPath, folder, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}