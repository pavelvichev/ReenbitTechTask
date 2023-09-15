using System.ComponentModel.DataAnnotations;

namespace BlobTask
{

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ValidateDocx : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public ValidateDocx(params string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is not IFormFile file)
            {
                return new ValidationResult("The provided file is not valid.");
            }

            if (file.Length == 0)
            {
                return new ValidationResult("The provided file is empty.");
            }


            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(fileExtension))
            {
                return new ValidationResult($"Allowed file extensions are: {string.Join(", ", _allowedExtensions)}");
            }

            return ValidationResult.Success;
        }
    }

}
