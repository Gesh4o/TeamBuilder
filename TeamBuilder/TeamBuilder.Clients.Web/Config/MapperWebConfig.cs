namespace TeamBuilder.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using AutoMapper;

    using TeamBuilder.Clients.Common;
    using TeamBuilder.Clients.Common.Mappings;
    using TeamBuilder.Clients.Models.Account;
    using TeamBuilder.Data.Models;

    public static class MapperWebConfig
    {
        public static void RegisterAllMappings()
        {
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<RegisterViewModel, ApplicationUser>()
                        .ForMember(
                            dest => dest.UserName,
                            mo => mo.MapFrom(src => src.Username));

                    ConfigureCustomMappings(cfg);
                    ConfigureDefaultMappings(cfg);
                });
        }

        private static void ConfigureDefaultMappings(IMapperConfigurationExpression cfg)
        {
            var maps = Assembly.Load(ServerConstants.ModelsClientsAssembly)
            .GetTypes()
            .SelectMany(t => t.GetInterfaces(), (t, i) => new { t, i })
            .Where(
                type =>
                type.i.IsGenericType && type.i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                !type.t.IsAbstract && !type.t.IsInterface)
                .Select(type => new { Source = type.i.GetGenericArguments()[0], Destination = type.t });

            foreach (var map in maps)
            {
                cfg.CreateMap(map.Source, map.Destination);
                cfg.CreateMap(map.Destination, map.Source);
            }
        }

        private static void ConfigureCustomMappings(IMapperConfigurationExpression cfg)
        {
            IEnumerable<IHaveCustomMappings> maps = Assembly.Load(ServerConstants.ModelsClientsAssembly).GetTypes().SelectMany(t => t.GetInterfaces(), (t, i) => new { t, i })
                                    .Where(type => typeof(IHaveCustomMappings).IsAssignableFrom(type.t) &&
                                    !type.t.IsAbstract && !type.t.IsInterface)
                                    .Select(type => (IHaveCustomMappings)Activator.CreateInstance(type.t));

            foreach (IHaveCustomMappings map in maps)
            {
                map.ConfigureMappings(cfg);
            }
        }
    }
}