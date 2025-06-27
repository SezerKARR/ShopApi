namespace Shop.Application.Validations
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    public class ImageUploadValidationAttribute : ValidationAttribute
{
    private readonly int _maxFiles;
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp"];

    public ImageUploadValidationAttribute(int maxFiles)
    {
        _maxFiles = maxFiles;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not List<IFormFile> files)
        {
            return ValidationResult.Success;
        }

        if (files.Count > _maxFiles)
        {
            return new ValidationResult($"You can max {_maxFiles} file upload.");
        }

        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!_allowedExtensions.Contains(extension))
            {
                return new ValidationResult($"only these formats are supported: {string.Join(", ", _allowedExtensions)}");
            }
        }

        return ValidationResult.Success;
    }
}
}
