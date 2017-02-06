﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DbLocalizationProvider.Internal;

namespace DbLocalizationProvider.Sync
{
    internal class EnumTypeScanner : IResourceTypeScanner
    {
        public bool ShouldScan(Type target)
        {
            return target.BaseType == typeof(Enum);
        }

        public string GetResourceKeyPrefix(Type target, string keyPrefix = null)
        {
            var resourceAttribute = target.GetCustomAttribute<LocalizedResourceAttribute>();

            return !string.IsNullOrEmpty(resourceAttribute?.KeyPrefix)
                       ? resourceAttribute.KeyPrefix
                       : (string.IsNullOrEmpty(keyPrefix) ? target.FullName : keyPrefix);
        }

        public ICollection<DiscoveredResource> GetClassLevelResources(Type target, string resourceKeyPrefix)
        {
            return Enumerable.Empty<DiscoveredResource>().ToList();
        }

        public ICollection<DiscoveredResource> GetResources(Type target, string resourceKeyPrefix)
        {
            return target.GetMembers(BindingFlags.Public | BindingFlags.Static)
                         .Select(mi => new DiscoveredResource(mi,
                                                              ResourceKeyBuilder.BuildResourceKey(resourceKeyPrefix, mi),
                                                              mi.Name,
                                                              mi.Name,
                                                              target,
                                                              Enum.GetUnderlyingType(target),
                                                              Enum.GetUnderlyingType(target).IsSimpleType())).ToList();
        }
    }
}
