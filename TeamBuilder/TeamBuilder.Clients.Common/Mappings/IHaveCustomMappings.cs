namespace TeamBuilder.Clients.Common.Mappings
{
    using AutoMapper;

    public interface IHaveCustomMappings
    {
        void ConfigureMappings(IMapperConfigurationExpression configuration);
    }
}
