using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillChopBE.DataAccessLayer.Models.Validation
{
    public interface IValidatableModel
    {
        public void Validate();
        public ValidationContainer SilentValidate();
    }

    public class ValidationContainer
    {
        public bool IsSuccess { get; }
        public ICollection<ValidationResult> ValidationResults { get; }

        public ValidationContainer(bool isSuccess, ICollection<ValidationResult> validationResults) 
        {
            IsSuccess = isSuccess;
            ValidationResults = validationResults;
        }
    }

    public class ValidatableModel : IValidatableModel
    {
        public ValidationContainer SilentValidate()
        {
            var validationResults = new List<ValidationResult>();
            var isSuccess = Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);
            return new ValidationContainer(isSuccess, validationResults);
        }

        public void Validate() => Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
