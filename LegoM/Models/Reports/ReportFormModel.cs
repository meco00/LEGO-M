namespace LegoM.Models.Reports
{
    using LegoM.Data.Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static Data.DataConstants.Report;

    public class ReportFormModel
    {
        [Required(ErrorMessage = "Please select an rating from the list.")]
        [EnumDataType(typeof(ReportType))]
        public ReportType? ReportType { get; set; }

        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength, ErrorMessage = "Field {0} must be between {2} and {1} characters long")]
        [Display(Name = "Report:")]
        public string Content { get; set; }

       
    }
}
