using System;

namespace Geta.AutoMapper.Tests.CustomMapping
{
    public class CustomMappingTestModel
    {
        public int SomeProperty { get; set; }
        public DateTime OtherProperty { get; set; }

        public CustomMappingChildModel Child { get; set; }

    }

    public class CustomMappingChildModel
    {
        public string SomeProperty { get; set; }
    }
}