using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZSharp.Framework.ValueObjects
{
    public class DateRange : BaseValueObject
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 用於EF的數據構造
        /// </summary>
        public DateRange()
        {
        }

        public DateRange(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;

            base.ThrowExceptionIfInvalid();
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            if (StartDate > EndDate)
            {
                //TODO: Message可放入資源文件中維護
                validationResults.Add(new ValidationResult("StartDateTime must be before EndDateTime.",
                                                         new string[] { "StartDateTime" }));
            }

            return validationResults;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.StartDate;
            yield return this.EndDate;
        }
    }
}
