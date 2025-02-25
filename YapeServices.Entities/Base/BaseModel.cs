using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Entities.Base
{
    public class BaseModel : IValidatableObject
    {
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult>();
        }

        public bool IsValid(out List<ValidationResult> errors)
        {
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(this, new ValidationContext(this, null, null), results, true);
            errors = results;
            return isValid;
        }
    }
}