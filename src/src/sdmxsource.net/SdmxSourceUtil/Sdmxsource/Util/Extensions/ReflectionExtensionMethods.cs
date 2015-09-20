// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionExtensionMethods.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Extensions related to Reflection
    /// </summary>
    public static class ReflectionExtensionMethods
    {
        /// <summary>
        /// Get public instance properties of <paramref name="type"/>. Works with interface inheritance 
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The public properties of <paramref name="type"/></returns>
        /// <remarks>Credit goes to <a href="http://stackoverflow.com/a/2444090">An answer at SO</a></remarks>
        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new HashSet<PropertyInfo>();

                var considered = new HashSet<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (!considered.Contains(subInterface))
                        {
                            considered.Add(subInterface);
                            queue.Enqueue(subInterface);
                        }
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    propertyInfos.UnionWith(typeProperties);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy
                | BindingFlags.Public | BindingFlags.Instance);
        }
    }
}