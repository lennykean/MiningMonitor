using System;
using System.ComponentModel.DataAnnotations;

namespace MiningMonitor.Model.Validation
{
    public class RequiredGuidAttribute : RequiredAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Guid guid && guid != Guid.Empty)
                return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required");
        }
    }
}
