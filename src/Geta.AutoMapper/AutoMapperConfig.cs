// Copyright (c) Geta Digital. All rights reserved.
// Licensed under Apache-2.0. See the LICENSE file in the project root for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Geta.AutoMapper
{
    public static class AutoMapperConfig
    {
        /// <summary>
        ///     Scans for AutoMapper mappings in classes that are marked with "AutoMap" attribute
        ///     or implements "ICustomMappings" interface and adds them to AutoMapper configuration.
        /// </summary>
        /// <param name="mapConfig">AutoMapper IMapperConfigurationExpression</param>
        /// <param name="assembliesToScan">Assemblies to scan</param>
        public static void LoadMappings(IMapperConfigurationExpression mapConfig, params Assembly[] assembliesToScan)
        {
            foreach (var assembly in assembliesToScan)
            {
                mapConfig.AddMaps(assembly);
            }
            
            var types = assembliesToScan.SelectMany(a => a.GetExportedTypes()).ToList();
            LoadCustomMappings(types, mapConfig);
        }

        private static void LoadCustomMappings(IEnumerable<Type> types, IMapperConfigurationExpression mapConfig)
        {
            var maps = (from t in types
                from i in t.GetInterfaces()
                where typeof(ICustomMapping).IsAssignableFrom(t) &&
                      !t.IsAbstract &&
                      !t.IsInterface
                select (ICustomMapping) Activator.CreateInstance(t)).ToArray();

            foreach (var map in maps) map.CreateMapping(mapConfig);
        }
    }
}