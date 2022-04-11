# Geta Mapping

![](http://tc.geta.no/app/rest/builds/buildType:(id:GetaPackages_GetaMapping_00ci),branch:master/statusIcon)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Geta_geta-mapping&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Geta_geta-mapping)
[![Platform](https://img.shields.io/badge/Platform-.NET%20Standard%202.0-blue.svg?style=flat)](https://docs.microsoft.com/en-us/dotnet/core/)

Geta Mapping is a project for common mapping logic. It consists of two libraries: [Geta.Mapping](#getamapping) and [Geta.AutoMapper](#getaautomapper).

## Geta.Mapping

### Description

Geta.Mapping is a library with abstractions for common mapping logic.

### Installation

```
Install-Package Geta.Mapping
```

### DI Configuration

For `StructureMap` or `Lamar`, configure interfaces to connect to implementations automatically:

```csharp
Scan(x =>
{
    x.TheCallingAssembly();
    x.WithDefaultConventions();
    x.ConnectImplementationsToTypesClosing(typeof(IMapper<,>));
    x.ConnectImplementationsToTypesClosing(typeof(ICreateFrom<,>));
});
```

### Usage

Then create a mapping class you want to map one object to another. Inherit from `IMapper<TFrom, TTo>`.

```csharp
public class MyPocoToMyDtoMapper : IMapper<MyPoco, MyDto>
{
    public virtual void Map(MyPoco from, MyDto to)
    {
        to.Name = from.Name;
    }
}
```

This mapping implementation will work for any classes, even for those that have a constructor with parameters.

If your destination class has a parameter-less constructor, then you can implement `Mapper<TFrom, TTo>`.

```csharp
public class MyPocoToMyDtoMapper : Mapper<MyPoco, MyDto>
{
    public override void Map(MyPoco from, MyDto to)
    {
        to.Name = from.Name;
    }
}
```

Now you can use this mapper by injecting it.

When you want to map one object to another, then use `IMapper<TFrom, TTo>` interface.

```csharp
public class MyController
{
    private readonly IMapper<MyPoco, MyDto> _myMapper;

    public MyController(IMapper<MyPoco, MyDto> myMapper)
    {
        _myMapper = myMapper;
    }
    
    public IActionResult Index()
    {
        var myPoco = // Get a source object from DB or somewhere else
        var myDto = new MyDto(true); // Instantiating _myDto_ manually as there is no parameter-less contructor
        _myMapper.Map(myPoco, myDto);

        // ...
    }
}
```

When you want to create one object from another, and a destination object's class has a parameter-less constructor, your mapper should implement `Mapper<TFrom, TTo>` and you should inject `ICreateFrom<TFrom, TTo>`.

```csharp
public class MyController
{
    private readonly ICreateFrom<MyPoco, MyDto> _myDtoCreator;

    public MyController(ICreateFrom<MyPoco, MyDto> myDtoCreator)
    {
        _myDtoCreator = myDtoCreator;
    }
    
    public IActionResult Index()
    {
        var myPoco = // Get a source object from DB or somewhere else
        var myDto = _myDtoCreator.Create(myPoco);

        // ...
    }
}
```

## Geta.AutoMapper

![](http://tc.geta.no/app/rest/builds/buildType:(id:TeamFrederik_AutoMapper_Debug)/statusIcon)

### Description
Geta.AutoMapper is a small addition for Automapper library to simplify mapping configuration. It scans for existing mappings and provides a standardized way to do a custom mappings with automapper using `ICustomMapping` interface.

### Features
* Scans for existing mappings in the solution
* Use `AutoMap` attribute to cover simple mapping cases (default functionality in AutoMapper - https://docs.automapper.org/en/stable/Attribute-mapping.html)
* Use `ICustomMapping` interface for a custom mapping scenarios

### How to get started?
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
### Usage 
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

