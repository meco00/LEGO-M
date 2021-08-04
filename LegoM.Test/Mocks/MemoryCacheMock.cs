namespace LegoM.Test.Mocks
{
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class MemoryCacheMock
    {
        public static IMemoryCache Instance
        {
            get
            {
                
                var memoryCacheOptions = new MemoryCacheOptions();

                return new MemoryCache(memoryCacheOptions);
            }
        }
    }
}
