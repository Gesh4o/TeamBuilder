﻿namespace TeamBuilder.Web
{
    using AutoMapper;

    using TeamBuilder.Common.ViewModels.Account;
    using TeamBuilder.Models;

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
                    });
        }
    }
}