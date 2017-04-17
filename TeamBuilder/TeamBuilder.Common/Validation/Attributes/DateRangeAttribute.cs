namespace TeamBuilder.Common.Validation.Attributes
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DateRangeAttribute : ValidationAttribute
    {
        public string MinValue { get; set; }

        public string MaxValue { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Date cannot be null or empty!");
            }

            DateTime? castedValue = value as DateTime?;

            if (castedValue == null)
            {
                return new ValidationResult("Date not valid!");
            }

            if (DateTime.Parse(this.MinValue) > castedValue || DateTime.Parse(this.MaxValue) < castedValue)
            {
                return new ValidationResult($"Date must be after {this.MinValue}.");
            }

            return ValidationResult.Success;
        }
    }
}
