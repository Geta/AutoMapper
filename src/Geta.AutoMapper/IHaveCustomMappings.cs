using AutoMapper;

namespace Geta.AutoMapper
{
    public interface IHaveCustomMappings
    {
        void CreateMappings(IConfiguration configuration);
    }
}
