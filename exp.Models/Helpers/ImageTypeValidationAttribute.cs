using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class ImageTypeValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        string[] allowedTypes = new string[] { "jpeg", "jpg", "png", "webp" };

        if (value == null)
        {
            return new ValidationResult(ErrorMessage ?? "Image is required");
        }

        var base64String = value as string;
        if (string.IsNullOrEmpty(base64String))
        {
            return new ValidationResult(ErrorMessage ?? "Invalid image format");
        }

        var regex = new Regex(@"^data:image\/(?<type>.+?);base64,", RegexOptions.IgnoreCase);
        var match = regex.Match(base64String);
        if (!match.Success)
        {
            return new ValidationResult(ErrorMessage ?? "Invalid image format");
        }

        var type = match.Groups["type"].Value.ToLowerInvariant();
        if (allowedTypes.Contains(type))
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage ?? "Image type not allowed");
    }

}
