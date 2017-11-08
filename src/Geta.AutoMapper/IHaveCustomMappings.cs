using AutoMapper;

namespace Geta.AutoMapper
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IMapperConfiguration configuration);
    }
}
