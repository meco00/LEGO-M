namespace LegoM.Areas.Admin.Models.Reports
{
    using LegoM.Services.Reports.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ReportsQueryModel
    {
        public const int ReportsPerPage = 9;

        public int CurrentPage { get; set; } = 1;

        [Display(Name = "Search")]
        public string SearchTerm { get; init; }

        public int TotalReports { get; set; }

        public IEnumerable<ReportServiceModel> Reports { get; set; }
    }
}
