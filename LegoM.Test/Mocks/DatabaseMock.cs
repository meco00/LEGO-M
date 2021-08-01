namespace LegoM.Test.Mocks
{
    using LegoM.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

   public static class DatabaseMock
    {
        public static LegoMDbContext Instance
        {
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<LegoMDbContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .Options;

                return new LegoMDbContext(dbContextOptions);
            }
        }
    }
}
