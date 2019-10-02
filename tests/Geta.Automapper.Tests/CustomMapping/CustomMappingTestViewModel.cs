using System;
using AutoMapper;

namespace Geta.AutoMapper.Tests.CustomMapping
{
    public class CustomMappingTestViewModel : ICustomMapping
    {
        public int Property { get; set; }
        public DateTime OtherProperty { get; set; }
        public string ChildProperty { get; set; }
        public void CreateMapping(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CustomMappingTestModel, CustomMappingTestViewModel>()
                .ForMember(d => d.ChildProperty, 
                    o => o.MapFrom(s => s.Child.SomeProperty))
                .ForMember(d => d.Property,
                    o => o.MapFrom(s => s.SomeProperty));
        }
    }
}