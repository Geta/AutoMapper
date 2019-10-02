using AutoMapper;
using AutoMapper.Configuration.Annotations;

namespace Geta.AutoMapper.Tests.AttributeMapping
{
    [AutoMap(typeof(AttributeMappingTestModel))]
    public class AttributeMappingTestViewModel
    {
        [SourceMember(nameof(AttributeMappingTestModel.SomeProperty))]
        public int PropertyA { get; set; }

        public string ChildProperty { get; set; }
    }
}