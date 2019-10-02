using System.Reflection;
using AutoFixture.Xunit2;
using AutoMapper;
using AutoMapper.Configuration;
using Xunit;

namespace Geta.AutoMapper.Tests.CustomMapping
{
    public class CustomMappingTests
    {
        private IMapper _mapper;

        public CustomMappingTests()
        {
            var configExpression = new MapperConfigurationExpression();
            AutoMapperConfig.LoadMappings(configExpression, Assembly.GetExecutingAssembly());
            _mapper = new MapperConfiguration(configExpression).CreateMapper();
        }

        [Theory, AutoData]
        public void Type_can_be_successfully_mapped_to_destination_type_using_custom_mapping(CustomMappingTestModel modelToMap)
        {
            var mappedModel = _mapper.Map<CustomMappingTestViewModel>(modelToMap);

            Assert.Equal(modelToMap.SomeProperty, mappedModel.Property);
            Assert.Equal(modelToMap.OtherProperty, mappedModel.OtherProperty);
            Assert.Equal(modelToMap.Child.SomeProperty, mappedModel.ChildProperty);

        }
    }
}
