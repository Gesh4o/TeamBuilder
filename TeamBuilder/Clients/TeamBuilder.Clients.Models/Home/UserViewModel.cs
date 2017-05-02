namespace TeamBuilder.Clients.Models.Home
{
    using AutoMapper;

    using TeamBuilder.Clients.Common.Mappings;
    using TeamBuilder.Data.Models;

    public class UserViewModel : IHaveCustomMappings
    {
        public string Username { get; set; }

        public void ConfigureMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserViewModel>().ForMember(
                dest => dest.Username,
                mo => { mo.MapFrom(src => src.UserName); });
        }
    }
}
