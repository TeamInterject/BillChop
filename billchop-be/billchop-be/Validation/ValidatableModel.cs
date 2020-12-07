using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace BillChopBE.Validation
{
    public interface IValidatableModel : IValidatable
    {
    }

    public class ValidatableModel : IValidatableModel
    {
        public void Validate() => Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
