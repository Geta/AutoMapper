namespace Geta.AutoMapper.Tests.AttributeMapping
{
    public class AttributeMappingTestModel
    {
        public int SomeProperty { get; set; }

        public AttributeMappingTestChildModel Child { get; set; }
    }

    public class AttributeMappingTestChildModel
    {
        public string Property { get; set; }
    }
}