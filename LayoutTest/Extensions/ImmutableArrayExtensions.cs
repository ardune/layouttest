using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace LayoutTest.Extensions
{
    public static class ImmutableArrayExtensions
    {
        private static readonly object MapperSync = new object();
        private static readonly Dictionary<Type,IMapper> Mappers = new Dictionary<Type, IMapper>();

        public static T[] Update<T, TOther>(this T[] target, T itemFromCollection, TOther propertiesToUpdate)
            where T : new()
        {
            var mapper = GetMapper<T, TOther>();

            var result = new T();
            result = mapper.Map(itemFromCollection, result);
            result = mapper.Map(propertiesToUpdate, result);

            var results = target.Select(x => ReferenceEquals(x,itemFromCollection) ? result : x);

            return results.ToArray();
        }

        private static IMapper GetMapper<T, TOther>() where T : new()
        {
            lock (MapperSync)
            {
                var key = typeof(TOther);
                if (Mappers.ContainsKey(key))
                {
                    return Mappers[key];
                }

                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<TOther, T>();
                    cfg.CreateMap<T, T>();
                });
                var mapper = new Mapper(config);
                Mappers[key] = mapper;
                return mapper;
            }
        }
    }
}
