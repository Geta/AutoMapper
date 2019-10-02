# Geta.AutoMapper

![](http://tc.geta.no/app/rest/builds/buildType:(id:TeamFrederik_AutoMapper_Debug)/statusIcon)

## Description
Geta.AutoMapper is a small addition for Automapper library to simplify mapping configuration. It scans for existing mappings and provides a standardized way to do a custom mappings with automapper using `ICustomMapping` interface.

## Features
* Scans for existing mappings in the solution
* Use `AutoMap` attribute to cover simple mapping cases (default functionality in AutoMapper - https://docs.automapper.org/en/stable/Attribute-mapping.html)
* Use `ICustomMapping` interface for a custom mapping scenarios

## How to get started?
```
Install-Package Geta.AutoMapper
```

Make sure to call `AutoMapperConfig.LoadMappings(...)` on application startup.

```csharp
var configExpression = new MapperConfigurationExpression();

/* can add custom configurations if needed */

AutoMapperConfig.LoadMappings(configExpression, Assembly.GetExecutingAssembly());

var mapper = new MapperConfiguration(configExpression).CreateMapper();
```
## Usage 
You can use two ways how to configure the mapping between two types:
1. Decorate destination type with `AutoMap` attribute (functionality already available in AutoMapper);
2. Destination type implementing `ICustomMapping` interface.


```csharp
public class AttributeMappingTestModel
{
	public int SomeProperty { get; set; }

	public AttributeMappingTestChildModel Child { get; set; }

}

public class AttributeMappingTestChildModel
{
	public string Property { get; set; }
}

[AutoMap(typeof(AttributeMappingTestModel))]
public class AttributeMappingTestViewModel
{
	[SourceMember(nameof(AttributeMappingTestModel.SomeProperty))]
	public int PropertyA { get; set; }

	public string ChildProperty { get; set; }
}
```

You can then simply call AutoMapper's `mapper.Map<AttributeMappingTestViewModel>(modelToMap);` to map from a `AttributeTestModel` to an `AttributeTestViewModel`.
```
Note: `SourceMember` attribute isn't working for child object property mapping configurations. If destination object propery isn't following naming pattern {PropertyName}{ChildPropertyName} you will have to use custom mapping approach.
```
Use `ICustomMappings` for more advanced mapping scenarios. 

```csharp
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

public class CustomMappingTestViewModel : ICustomMapping
{
	public int Property { get; set; }
	public DateTime OtherProperty { get; set; }
	public string ChildProperty { get; set; }
	public void CreateMapping(IMapperConfigurationExpression configuration)
	{
		configuration.CreateMap<CustomMappingTestModel, CustomMappingTestViewModel>()
			.ForMember(d => d.ChildProperty, o => o.MapFrom(s => s.Child.SomeProperty))
			.ForMember(d => d.Property, o => o.MapFrom(s => s.SomeProperty));
	}
}
```

