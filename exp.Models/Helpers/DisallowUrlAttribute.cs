using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace exp.Models.Helpers
{
    public sealed class DisallowUrlAttribute : ValidationAttribute
    {
        private static readonly Regex LooksLikeUrl = new(
            @"((?i)\bhttps?://|\bwww\.|@|[A-Za-z0-9-]+\.[A-Za-z]{2,})(/|\b)",
            RegexOptions.Compiled);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var s = value as string;
            if (string.IsNullOrWhiteSpace(s)) return ValidationResult.Success;

            // Normalizează spații
            s = s.Trim();

            // Dacă arată ca URL/email/domeniu, respinge
            if (LooksLikeUrl.IsMatch(s))
                return new ValidationResult("Câmpul Name nu poate conține URL-uri, domenii sau adrese de email.");

            return ValidationResult.Success;
        }
    }
}
