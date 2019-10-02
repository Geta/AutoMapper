# Changelog

All notable changes to this project will be documented in this file.

## [2.0.0]

### Added
- .NET Standard 2.0 support;

### Changed
- `AutoMapperConfig.Execute()` changed to `AutoMapperConfig.LoadMappings(IMapperConfigurationExpression mapConfig, params Assembly[] assembliesToScan)`;
- `IHaveCustomMappings` renamed to `ICustomMapping` and method `CreateMappings` from plural to singular `CreateMapping`.

### Removed
- `IMapFrom` - use AutoMapper `AutoMap` attribute on destination type instead;
- App setting `AutoMapper:AssemblyName` - Assamblies are provided as parameter to LoadMappings.


## [1.0.0]

Initial release.