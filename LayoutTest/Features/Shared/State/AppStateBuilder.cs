using AutoMapper;
using LayoutTest.StateManagement;

namespace LayoutTest.Features.Shared.State
{
    public class AppStateBuilder : IHeldStateBuilder<AppState>
    {
        private static readonly IMapper Mapper;

        public Page[] Pages { get; set; }

        public PrepActivity PrepActivity { get; set; }

        public void InitializeFrom(AppState state)
        {
            Mapper.Map(state, this);
        }

        public AppState Build()
        {
            return Mapper.Map<AppState>(this);
        }

        public AppState DefaultState()
        {
            return new AppState
            {
                Pages = new Page[0],
                PrepActivity = new PrepActivity
                {
                    Foo = true
                }
            };
        }

        static AppStateBuilder()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AppState, AppState>();
                cfg.CreateMap<AppState, AppStateBuilder>()
                    .ReverseMap();
            });
            Mapper = new Mapper(config);
        }
    }
}