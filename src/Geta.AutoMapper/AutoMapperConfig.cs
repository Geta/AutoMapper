using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;

namespace Geta.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void Execute()
        {
            var assemblyName = ConfigurationManager.AppSettings["AutoMapper:AssemblyName"];

            if (string.IsNullOrEmpty(assemblyName)) return;

            // To scan all assemblies starting with:
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith(assemblyName))
                .SelectMany(a => a.GetExportedTypes())
                .ToList();

            LoadStandardMappings(types);

            LoadCustomMappings(types);
        }

        private static void LoadCustomMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                from i in t.GetInterfaces()
                where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                      !t.IsAbstract &&
                      !t.IsInterface
                select (IHaveCustomMappings) Activator.CreateInstance(t)).ToArray();

            Mapper.Initialize(
                c =>
                {
                    foreach (var map in maps) map.CreateMappings(c);
                });
        }

        private static void LoadStandardMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                from i in t.GetInterfaces()
                where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                      !t.IsAbstract &&
                      !t.IsInterface
                select new
                {
                    Source = i.GetGenericArguments()[0],
                    Destination = t
                }).ToArray();

            Mapper.Initialize(
                c =>
                {
                    foreach (var map in maps) c.CreateMap(map.Source, map.Destination);
                });
        }
    }
}