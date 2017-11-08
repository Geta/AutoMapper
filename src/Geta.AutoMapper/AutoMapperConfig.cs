﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using Foundation.Core.Infrastructure.Tasks;

namespace Geta.AutoMapper
{
    public class AutoMapperConfig : IRunAtInit
    {
        private readonly IMapperConfiguration _configuration;

        public AutoMapperConfig(IMapperConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Execute()
        {
            var assemblyName = ConfigurationManager.AppSettings["AutoMapper:AssemblyName"];

            if (string.IsNullOrEmpty(assemblyName))
            {
                return;
            }

            // To scan all assemblies starting with:
            var types = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith(assemblyName)).SelectMany(a => a.GetExportedTypes()).ToList();

            LoadStandardMappings(types);

            LoadCustomMappings(types);
        }

        private void LoadCustomMappings(IEnumerable<Type> types)
        {
            var maps = (from t in types
                        from i in t.GetInterfaces()
                        where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                              !t.IsAbstract &&
                              !t.IsInterface
                        select (IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps)
            {
                map.CreateMappings(_configuration);
            }
        }

        private void LoadStandardMappings(IEnumerable<Type> types)
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

            foreach (var map in maps)
            {
                _configuration.CreateMap(map.Source, map.Destination);
            }
        }
    }
}
