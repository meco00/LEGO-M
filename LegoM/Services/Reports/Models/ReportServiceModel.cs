namespace LegoM.Services.Reports.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReportServiceModel
    {
        public int Id { get; set; }

        public string ReportType { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public string ProductId { get; set; }

        public string ProductTitle { get; set; }

        public string PublishedOn { get; set; }
    

    }
}
