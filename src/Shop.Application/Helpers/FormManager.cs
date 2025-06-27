namespace Shop.Application.Helpers;

using Microsoft.AspNetCore.Http;

public enum FormTypes {
    Image,
    
}
public static class FormManager {
    
    public static Dictionary<FormTypes, List<string>> AllowedExtensions = new Dictionary<FormTypes, List<string>>()
    {

        {
            FormTypes.Image,new List<string>(){".jpg",".png",".bmp",".gif",".ico"}
        }
    };
    public static string Save(IFormFile? file, string folder, FormTypes type) {
        string rootPath = Directory.GetCurrentDirectory() + "/wwwroot";
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Invalid file");
        }

        // string extension = Path.GetExtension(file.FileName).ToLower();
        // if (!AllowedExtensions[type].Contains(extension))
        // {
        //     throw new ArgumentException("Unsupported file type");
        // }

        string directoryPath = Path.Combine(rootPath, folder);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string newFileName = Guid.NewGuid().ToString() + 
                             (file.FileName.Length > 64 ? file.FileName.Substring(file.FileName.Length - 64) : file.FileName);

        string filePath = Path.Combine(directoryPath, newFileName);

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }
        string asd = Path.Combine(folder, newFileName);
        return asd;
    }
    public static List<string> Save(IList<IFormFile> files, string folder, FormTypes type) {
        List<string> urls = new List<string>();
        foreach (var file in files)
        {
            urls.Add(Save(file, folder, type));
        }
        return urls;
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