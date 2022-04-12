using System.Reflection;
using AutoFixture.Xunit2;
using AutoMapper;
using AutoMapper.Configuration;
using Xunit;

namespace Geta.AutoMapper.Tests.AttributeMapping
{
    public class AttributeMappingTests
    {
        private IMapper _mapper;

        public AttributeMappingTests()
        {
            var configExpression = new MapperConfigurationExpression();
            AutoMapperConfig.LoadMappings(configExpression, Assembly.GetExecutingAssembly());
            _mapper = new MapperConfiguration(configExpression).CreateMapper();
        }

        [Theory, AutoData]
        public void Type_can_be_successfully_mapped_to_destination_type_using_AutoMap_attribute(
            AttributeMappingTestModel modelToMap)
        {
            var mappedModel = _mapper.Map<AttributeMappingTestViewModel>(modelToMap);

            Assert.Equal(modelToMap.SomeProperty, mappedModel.PropertyA);
            Assert.Equal(modelToMap.Child.Property, mappedModel.ChildProperty);
        }
    }
}