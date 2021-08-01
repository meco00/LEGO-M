

namespace LegoM.Controllers.Api
{
    using LegoM.Data;
    using LegoM.Services.Statistics;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController:ControllerBase
    {

        private readonly IStatisticsService statistics;

        public StatisticsApiController(IStatisticsService statistics)
        => this.statistics = statistics;



        [HttpGet]
        public StatisticsServiceModel GetStatistics()
        => this.statistics.Total();





    }
}
