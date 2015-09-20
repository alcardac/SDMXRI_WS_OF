// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxStructureTypeExtensions.cs" company="Eurostat">
//   Date Created : 2013-05-30
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The SDMX structure type extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    using Estat.Sri.MappingStoreRetrieval.Manager;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///     The SDMX structure type extensions.
    /// </summary>
    public static class SdmxStructureTypeExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Check if <paramref name="structureType"/> is one of <paramref name="structureTypes"/>.
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <param name="structureTypes">
        /// The structure types.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsOneOf(this SdmxStructureEnumType structureType, params SdmxStructureEnumType[] structureTypes)
        {
            if (structureTypes != null)
            {
                for (int i = 0; i < structureTypes.Length; i++)
                {
                    if (structureType == structureTypes[i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check if <paramref name="structureType"/> is one of <paramref name="structureTypes"/>.
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <param name="structureTypes">
        /// The structure types.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsOneOf(this SdmxStructureType structureType, params SdmxStructureEnumType[] structureTypes)
        {
            return structureType != null && IsOneOf(structureType.EnumType, structureTypes);
        }

        /// <summary>
        /// Returns a value indicating whether categorisations are needed. (For <c>SDMX v2.0</c>)
        /// </summary>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <returns>
        /// True if categorisations are needed. (For <c>SDMX v2.0</c>); otherwise false.
        /// </returns>
        public static bool NeedsCategorisation(this IList<IStructureReference> queries)
        {
            return queries.Any(structureReference => structureReference.MaintainableStructureEnumType.NeedsCategorisation());
        }

        /// <summary>
        /// Returns a value indicating whether categorisations are needed. (For <c>SDMX v2.0</c>)
        /// </summary>
        /// <param name="structureType">
        /// The structure Type.
        /// </param>
        /// <returns>
        /// True if categorisations are needed. (For <c>SDMX v2.0</c>); otherwise false.
        /// </returns>
        public static bool NeedsCategorisation(this SdmxStructureType structureType)
        {
            return structureType.IsOneOf(SdmxStructureEnumType.Dataflow, SdmxStructureEnumType.CategoryScheme);
        }

        /// <summary>
        /// Returns the <c>SDMX v2.1</c> artefacts.
        /// </summary>
        /// <param name="maintainables">
        /// The maintainables.
        /// </param>
        /// <typeparam name="T">
        /// The type of maintainable
        /// </typeparam>
        /// <returns>
        /// The <see cref="IEnumerable{T}"/>.
        /// </returns>
        public static IEnumerable<T> GetSdxmV21<T>(this IEnumerable<T> maintainables)
            where T : IMaintainableMutableObject
        {
            return maintainables.Where(maintainable => !maintainable.IsSdmxV20Only());
        }

        /// <summary>
        /// Check if the specified <paramref name="maintainable"/> is <c>SDMX v2.0</c> only.
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable.
        /// </param>
        /// <returns>
        /// True if the specified <paramref name="maintainable"/> is <c>SDMX v2.0</c> only; otherwise false.
        /// </returns>
        public static bool IsSdmxV20Only(this IMaintainableMutableObject maintainable)
        {
            if (maintainable != null)
            {
                switch (maintainable.StructureType.EnumType)
                {
                    case SdmxStructureEnumType.Dsd:
                        var crossDsd = maintainable as ICrossSectionalDataStructureMutableObject;
                        if (crossDsd != null)
                        {
                            return true;
                        }

                        break;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if <paramref name="structureType"/> requires authentication
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool NeedsAuth(this SdmxStructureEnumType structureType)
        {
            return IsOneOf(structureType, SdmxStructureEnumType.Dataflow, SdmxStructureEnumType.Categorisation);
        }

        /// <summary>
        /// Check if <paramref name="structureType"/> requires authentication
        /// </summary>
        /// <param name="structureType">
        /// The structure type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool NeedsAuth(this SdmxStructureType structureType)
        {
            return structureType != null && NeedsAuth(structureType.EnumType);
        }

        /// <summary>
        /// Returns true if the the specified <paramref name="artefact"/> is it's the default value. Null for class based objects. 
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="artefact">The object. to check.</param>
        /// <returns>true if the the specified <paramref name="artefact"/> is it's the default value; otherwise false.</returns>
        public static bool IsDefault<T>(this T artefact)
        {
            return EqualityComparer<T>.Default.Equals(artefact, default(T));
        }

        /// <summary>
        /// Split version of the specified <paramref name="maintainableRefObject"/>.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The maintainable reference object.
        /// </param>
        /// <returns>
        /// The list of version tokens
        /// </returns>
        public static IList<long?> SplitVersion(this IMaintainableRefObject maintainableRefObject)
        {
            var version = SplitVersion(maintainableRefObject, 3);
            version[0] = version[0] ?? 0;
            version[1] = version[1] ?? 0;
            return version;
        }

        /// <summary>
        /// Split version of the specified <paramref name="maintainableRefObject"/>.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The maintainable reference object.
        /// </param>
        /// <param name="size">
        /// The number of tokens to return.
        /// </param>
        /// <returns>
        /// The list of version tokens
        /// </returns>
        public static IList<long?> SplitVersion(this IMaintainableRefObject maintainableRefObject, int size)
        {
            var version = new long? [size];
            if (maintainableRefObject.HasVersion())
            {
                for (int i = 0; i < version.Length; i++)
                {
                    version[i] = null;
                }

                var versionArray = maintainableRefObject.Version.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                int len = Math.Min(versionArray.Length, version.Length);
                for (int i = 0; i < len; i++)
                {
                    int value;
                    if (int.TryParse(versionArray[i], out value))
                    {
                        version[i] = value;
                    }
                }
            }
            else
            {
                for (int i = 0; i < version.Length; i++)
                {
                    version[i] = null;
                }
            }

            return version;
        }

        /// <summary>
        /// Generate version parameters.
        /// </summary>
        /// <param name="maintainableRefObject">
        /// The maintainable ref object.
        /// </param>
        /// <param name="database">
        /// The database.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="versionFieldPrefix">
        /// The version field prefix.
        /// </param>
        /// <param name="parameterNameBuilder">
        /// The parameter name builder.
        /// </param>
        /// <returns>
        /// The WHERE clause that contains the where statement.
        /// </returns>
        public static string GenerateVersionParameters(this IMaintainableRefObject maintainableRefObject, Database database, IList<DbParameter> parameters, string versionFieldPrefix, Func<string, string> parameterNameBuilder)
        {
            var sql = new StringBuilder();
            if (maintainableRefObject.HasVersion())
            {
                var version = maintainableRefObject.SplitVersion();
                
                // update this when Mapping Store stored function isEqualVersion changes.
                Debug.Assert(version.Count == 3, "Version particles not 3. Either update this assert or fix the code.");
                
                if (version.Count  > 0)
                {
                    sql.Append("dbo.isEqualVersion(");
                    var parameterNames = new string[version.Count];
                    for (int i = 0; i < version.Count; i++)
                    {
                        var particle = version[i];
                        var versionNo = (i + 1).ToString(CultureInfo.InvariantCulture);
                        sql.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}, ", versionFieldPrefix, versionNo);
                        var parameterName = parameterNameBuilder(versionNo);
                        parameterNames[i] = database.BuildParameterName(parameterName);
                        parameters.Add(database.CreateInParameter(parameterName, DbType.Int64, particle.ToDbValue()));
                    }

                    sql.Append(string.Join(",", parameterNames));

                    sql.Append(")=1");
                }
            }

            return sql.ToString();
        }

        #endregion
    }
}