// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Util.Reflect
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    using Org.Sdmxsource.Util.Extensions;

    /// <summary>
    /// The reflect util.
    /// </summary>
    /// <typeparam name="T">Generic type param
    /// </typeparam>
    public class ReflectUtil<T>
        where T : class
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets composite objects from the properties of <paramref name="inputObject"/> excluding <paramref name="ignoreProperties"/>
        /// </summary>
        /// <param name="inputObject">
        /// The input <see cref="object"/> . 
        /// </param>
        /// <param name="ignoreProperties">
        /// The ignored properties. 
        /// </param>
        /// <returns>
        ///  Returns a <see cref="ISet{T}"/> of the composite objects from the properties of <paramref name="inputObject"/> excluding <paramref name="ignoreProperties"/>
        /// </returns>
        public ISet<T> GetCompositeObjects(object inputObject, params PropertyInfo[] ignoreProperties)
        {
            ISet<T> returnSet = new HashSet<T>();
            Type referencedClass = typeof(T);

            var currentProperties = inputObject.GetType().GetPublicProperties();
            foreach (PropertyInfo currentProperty in currentProperties)
            {
                if (!Contains(currentProperty, ignoreProperties))
                {
                    MethodInfo currentMethod = currentProperty.GetGetMethod();
                    Type returnClass = currentMethod.ReturnType;

                    if (typeof(IEnumerable).IsAssignableFrom(returnClass))
                    {
                        Type returnType = currentMethod.ReturnType;
                        if (returnType.IsGenericType)
                        {
                            Type[] typeArguments = returnType.GetGenericArguments();

                            foreach (Type typeArgument in typeArguments)
                            {
                                if (referencedClass.IsAssignableFrom(typeArgument))
                                {
                                    var colection = (IEnumerable<T>)currentMethod.Invoke(inputObject, null);
                                    if (colection != null)
                                    {
                                        returnSet.UnionWith(colection);
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (referencedClass.IsAssignableFrom(returnClass))
                        {
                            var claz = (T)currentMethod.Invoke(inputObject, null);
                            if (claz != null && !returnSet.Contains(claz))
                            {
                                returnSet.Add(claz);
                            }
                        }
                    }
                }
            }

            return returnSet;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks if a property with the name of <paramref name="property"/> is contained in <paramref name="properties"/>
        /// </summary>
        /// <param name="property">
        /// The property. 
        /// </param>
        /// <param name="properties">
        /// The properties. 
        /// </param>
        /// <returns>
        /// True if a property with the name of <paramref name="property"/> is contained in <paramref name="properties"/> ; otherwise false 
        /// </returns>
        private static bool Contains(PropertyInfo property, IEnumerable<PropertyInfo> properties)
        {
            if (properties == null)
            {
                return false;
            }

            foreach (PropertyInfo currentProperty in properties)
            {
                if (currentProperty.Name.Equals(property.Name))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        ////public static bool IsGetter(MethodInfo method)
        ////{
        ////  if (!method.Name.StartsWith("get")) return false;
        ////  if (new ILOG.J2CsMapping.Reflect.IlrMethodInfoAdapter(method.GetParameters()).GetTypes().Length != 0) return false;
        ////  if (typeof(void).Equals(method.ReturnType)) return false;
        ////  return true;
        ////}
    }
}