using System;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Roulette.Application.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public IList<string> ValidationErrors { get; set; }
        public ValidationException(ValidationResult validationResult)
        {
            ValidationErrors = new List<string>();
            foreach (var validationError in validationResult.Errors)
            {
                ValidationErrors.Add(validationError.ErrorMessage);
            }
        }
    }
}
