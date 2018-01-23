# Geta.AutoMapper

![](http://tc.geta.no/app/rest/builds/buildType:(id:TeamFrederik_AutoMapper_Debug)/statusIcon)

## Description
AutoMapper is a simple little library built to solve a deceptively complex problem - getting rid of code that mapped one object to another. This package contains two interfaces to get started with AutoMapper. 

## Features
* Configurations in appsetting section
* IMapFrom<T> interface is used to automatically configure mapping between two objects
* IHaveCustomMappings interface for more advanced mapping scenarios

```
Install-Package Geta.AutoMapper
```

Add this to your projects appsettings (the assembly with the mapping classes):

```xml
<add key="AutoMapper:AssemblyName" value="assembly-name" />
```

Make sure to call AutoMapperConfig.Execute() on application startup.

```csharp
AutoMapperConfig.Execute();
```
## Usage 
We've included two interface that you can implement to `IMapFrom<T>` and `IHaveCustomMappings`.

`IMapFrom<T>` is used to automatically configure mapping between two objects.

```csharp
    public class Issue
    {
	public int IssueID { get; set; }

	public ApplicationUser Creator { get; set; }

	public ApplicationUser AssignedTo { get; set; }

	public string Subject { get; set; }
	
	public string Body { get; set; }

	public DateTime CreatedAt { get; set; }
	public IssueType IssueType { get; set; }
    }

    public class EditIssueForm : IMapFrom<Domain.Issue>
    {
	[HiddenInput]
	public int IssueID { get; set; }

	[ReadOnly(true)]
	public string CreatorUserName { get; set; }

	[Required]
	public string Subject { get; set; }

	public IssueType IssueType { get; set; }
	
	[Display(Name = "Assigned To")]
	public string AssignedToUserName { get; set; }
		
	[Required]
	public string Body { get; set; }
    }
```

You can then simply call AutoMapper's `Mapper.Map<EditIssueForm>(issue);` to map from a domain Issue to an EditIssueForm. This of course also works with the Project() method: `Issues.Project().To<EditIssueForm>()`.

Use IHaveCustomMappings for more advanced mapping scenarios. 

```csharp
    public class AssignmentStatsViewModel : IHaveCustomMappings
    {
	public string UserName { get; set; }
	public int Enhancements { get; set; }
	public int Bugs { get; set; }
	public int Support { get; set; }
	public int Other { get; set; }

	public void CreateMappings(IMapperConfigurationExpression configuration)
	{
	    configuration.CreateMap<ApplicationUser, AssignmentStatsViewModel>()
		.ForMember(m => m.Enhancements, opt => opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Enhancement)))
		.ForMember(m => m.Bugs, opt => opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Bug)))
		.ForMember(m => m.Support, opt => opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Support)))
		.ForMember(m => m.Other, opt => opt.MapFrom(u => u.Assignments.Count(i => i.IssueType == IssueType.Other)));
	}
    }
```

## Package maintainer
https://github.com/frederikvig
