namespace LegoM.Services.Reports.Models
{
    using System.Collections.Generic;

    public class ReportQueryModel
    {
        public int CurrentPage { get; init; }

        public int ReportsPerPage { get; init; }

        public int TotalReports { get; init; }

        public IEnumerable<ReportServiceModel> Reports { get; init; }
    }
}
