namespace LegoM.Test.Mocks
{
    using AutoMapper;
    using LegoM.Infrastructure;
    using Moq;

    public static class MapperMock
    {
        public static IMapper Instance
        {
            get
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                  cfg.AddProfile<MappingProfile>()
                  );

                return new Mapper(mapperConfiguration);
            }
        }
    }
}
