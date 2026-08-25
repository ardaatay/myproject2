using System.ComponentModel.DataAnnotations;

namespace Dto.Validation
{
    public class RequiredIfKategoriAttribute(int id) : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var kategoriIdProperty = instance.GetType().GetProperty("KategoriId");
            
            if (kategoriIdProperty == null)
                return ValidationResult.Success;
                
            var kategoriId = (int)kategoriIdProperty.GetValue(instance, null)!;
            
            if (kategoriId == id && value == null)
                return new ValidationResult(ErrorMessage);
                
            return ValidationResult.Success;
        }
    }
}