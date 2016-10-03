Geta.AutoMapper

Add this to your projects appsettings (the assembly with the mapping classes):
<add key="AutoMapper:AssemblyName" value="assembly-name" />

Make sure to add Geta.AutoMapper to TaskRegistry in your project.

scan.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("Foundation") || a.FullName.StartsWith("Geta.AutoMapper));