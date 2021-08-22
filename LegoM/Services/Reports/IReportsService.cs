namespace LegoM.Services.Reports
{
    using LegoM.Data.Models.Enums;
    using LegoM.Services.Reports.Models;

    public interface IReportsService
    {
        void Add(string content,
            ReportType reportType,
            string productId,
            string userId);

        ReportQueryModel All(
           string searchTerm = null,
           int currentPage = 1,
           int reportsPerPage = int.MaxValue,
           string productId = null);

        bool Delete(int id);


        bool ReportExistsForProduct(string productId, string userId);


    }
}
