namespace LegoM.Services.Reports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReportQueryModel
    {
        public int CurrentPage { get; init; }

        public int ReportsPerPage { get; init; }

        public int TotalReports { get; init; }

        public IEnumerable<ReportServiceModel> Reports { get; init; }
    }
}
