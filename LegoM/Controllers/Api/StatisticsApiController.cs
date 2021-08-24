namespace LegoM.Controllers.Api
{
    using LegoM.Services.Statistics;
    using LegoM.Services.Statistics.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using System;

    using static WebConstants.Cache;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController:ControllerBase
    {
        private readonly IStatisticsService statistics;
        private readonly IMemoryCache cache;

        public StatisticsApiController(IStatisticsService statistics, IMemoryCache cache)
        {
            this.statistics = statistics;
            this.cache = cache;
        }

        [HttpGet]
        public StatisticsServiceModel GetStatistics()
        {
            var totalStatistics = this.cache.Get<StatisticsServiceModel>(TotalStatisticsCacheKey);

            if (totalStatistics == null)
            {
                var statistics = this.statistics.Total();

                var cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                totalStatistics = this.cache.Set(TotalStatisticsCacheKey, statistics, cacheOptions);
            }

            return totalStatistics;
        }
    }
}
