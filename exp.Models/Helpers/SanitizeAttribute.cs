using Ganss.Xss;
using System.ComponentModel.DataAnnotations;

public class SanitizeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        HtmlSanitizer sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Clear(); // Reset allowed tags to default 

        if (value is string stringValue)
        {
            var sanitizedValue = sanitizer.Sanitize(stringValue);

            var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(validationContext.ObjectInstance, sanitizedValue);
            }
        }
        else if (value is List<string> stringList)
        {
            var sanitizedList = stringList.Select(s => sanitizer.Sanitize(s)).ToList();

            var property = validationContext.ObjectType.GetProperty(validationContext.MemberName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(validationContext.ObjectInstance, sanitizedList);
            }
        }
        return ValidationResult.Success;

    }
}
