// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrnUtil.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects
{
    #region Using directives

    using System;
    using System.Globalization;

    using log4net;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    #endregion


    /// <summary>
    /// The urn util.
    /// </summary>
    public class UrnUtil
    {
        #region Static Fields

        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog _log = LogManager.GetLogger(typeof(UrnUtil));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the type of identifiable object that this urn represents
        /// </summary>
        /// <param name="urn">The Uri
        /// </param>
        /// <returns>
        /// The <see cref="SdmxStructureType"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Throws SdmxSemmanticException
        /// </exception>
        public static SdmxStructureType GetIdentifiableType(string urn)
        {
            string[] splitUrn = SplitUrn(urn);
            string urnPrefix = splitUrn[0];
            return SdmxStructureType.ParsePrefix(urnPrefix);
        }

        /// <summary>
        /// Gets the Uri components
        /// </summary>
        /// <param name="urn">The Uri
        /// </param>
        /// <returns>
        /// The uri components
        /// </returns>
        public static string[] GetUrnComponents(string urn)
        {
            string mainUrn = GetUrnPostfix(urn);
            if (GetIdentifiableType(urn).EnumType == SdmxStructureEnumType.Agency)
            {
                string[] agencies = mainUrn.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (agencies.Length == 1)
                {
                    // This is an agency belonging to the default scheme
                    return new[]
                        {
                           AgencyScheme.DefaultScheme, AgencyScheme.FixedId, agencies[0] 
                        };
                }

                return new[]
                    {
                        mainUrn.Substring(0, mainUrn.LastIndexOf('.')), AgencyScheme.FixedId, 
                        agencies[agencies.Length - 1]
                    };
            }

            string urnVersionsRemoved = RemoveVersionsFromUrn(mainUrn);
            string[] majorComponents = urnVersionsRemoved.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            if (majorComponents.Length == 1)
            {
                throw new SdmxSemmanticException(
                    "URN agency id is expected to be followed by a ':' character : '" + urn + "'");
            }

            if (majorComponents.Length != 2)
            {
                throw new SdmxSemmanticException("URN should not contain more than one ':' character: '" + urn + "'");
            }

            string[] minorComponents = majorComponents[1].Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var returnArray = new string[minorComponents.Length + 1];
            returnArray[0] = majorComponents[0];
            for (int i = 0; i < minorComponents.Length; i++)
            {
                returnArray[i + 1] = minorComponents[i];
            }

            return returnArray;
        }

        /// <summary>
        /// The get urn postfix.
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetUrnPostfix(string urn)
        {
            string[] splitUrn = SplitUrn(urn);

            return splitUrn[1];
        }

        /////// <summary>
        /////// The get urn postfix.
        /////// </summary>
        /////// <param name="agencyId">
        /////// The agency id.
        /////// </param>
        /////// <param name="id">
        /////// The id.
        /////// </param>
        /////// <param name="version">
        /////// The version.
        /////// </param>
        /////// <returns>
        /////// The <see cref="string"/>.
        /////// </returns>
        ////public static string GetUrnPostfix(string agencyId, string id, string version)
        ////{
        ////    return agencyId + ":" + id + "(" + version + ")";
        ////}

        /// <summary>
        /// Returns the urn postfix.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id.
        /// </param>
        /// <param name="maintid">
        /// The maintainable id
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The URN postifix which consists <paramref name="agencyId"/>:<paramref name="maintid"/>(<paramref name="version"/>)[.<paramref name="id"/>...]
        /// </returns>
        public static string GetUrnPostfix(string agencyId, string maintid, string version, params string[] id)
        {
            string idPost = id != null && id.Length > 0 ? "." + string.Join(".", id) : string.Empty;
            
            return string.Format(CultureInfo.InvariantCulture, "{0}:{1}({2}){3}", agencyId, maintid, version, idPost);
        }

        /// <summary>
        /// The get urn prefix.
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetUrnPrefix(string urn)
        {
            string[] splitUrn = SplitUrn(urn);
            return splitUrn[0];
        }

        /// <summary>
        /// The get version from urn.
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <returns>
        /// The version from <paramref name="urn"/> else null;
        /// </returns>
        public static string GetVersionFromUrn(string urn)
        {
            if (urn == null)
            {
                return null;
            }

            int paraFirst = urn.IndexOf('(') + 1;
            int paraLast = urn.LastIndexOf(')');
            if (paraFirst > 0 && paraLast > paraFirst)
            {
                int length = paraLast - paraFirst;
                return urn.Substring(paraFirst, length);
            }

            int bracketFirst = urn.IndexOf('[') + 1;
            int bracketLast = urn.IndexOf(']');

            if (bracketFirst > 0 && bracketLast >= bracketFirst)
            {
                int length = bracketLast - bracketFirst;
                _log.Warn("URN using pre 2.1 syntax [#version] upgrading to (#version) : " + urn);
                return urn.Substring(bracketFirst, length);
            }

            ////if (urn.Contains("[") || urn.Contains("]"))
            ////{
            ////    _log.Warn("URN using pre 2.1 syntax [#version] upgrading to (#version) : " + urn);
            ////    urn = urn.Replace("[", "(");
            ////    urn = urn.Replace("]", ")");
            ////}

            ////string mainUrn = GetUrnPostfix(urn);

            ////Match m = _versionRegex.Match(mainUrn);
            ////if (m.Success && m.Groups.Count > 0)
            ////{
            ////    string version = m.Groups[0].Value;
            ////    version = version.Replace("(", string.Empty);
            ////    version = version.Replace(")", string.Empty);
            ////    return version;
            ////}

            return null;
        }

        /// <summary>
        /// The validate urn.
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="structureEnumType">
        /// The _structure enum type.
        /// </param>
        /// <exception cref="SdmxSemmanticException">Throws SdmxSemmanticException
        /// </exception>
        public static void ValidateURN(string urn, SdmxStructureType structureEnumType)
        {
            string[] splitUrn = SplitUrn(urn);
            if (splitUrn.Length == 0)
            {
                throw new SdmxSemmanticException(ExceptionCode.StructureUrnMalformed, urn);
            }

            string urnPrefix = splitUrn[0];
            SdmxStructureType targetEnumType = SdmxStructureType.ParsePrefix(urnPrefix);
            if (targetEnumType != structureEnumType)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.StructureUrnUnexpectedPrefix, urn, structureEnumType.UrnPrefix);
            }

            string[] components = GetUrnComponents(urn);
            if (structureEnumType.IsMaintainable)
            {
                if (components == null)
                {
                    throw new SdmxSemmanticException(ExceptionCode.StructureUrnMalformed, urn);
                }

                if (structureEnumType.EnumType == SdmxStructureEnumType.Subscription)
                {
                    if (components.Length != 4)
                    {
                        throw new SdmxSemmanticException(ExceptionCode.StructureUrnMalformed, urn);
                    }
                }
                else if (components.Length != 2)
                {
                    throw new SdmxSemmanticException(ExceptionCode.StructureUrnMalformed, urn);
                }
            }
            else if (structureEnumType.IsIdentifiable)
            {
                if (components == null || (components.Length <= 2 && !(components.Length == 1 && structureEnumType.EnumType == SdmxStructureEnumType.Agency)))
                {
                    throw new SdmxSemmanticException(ExceptionCode.StructureUrnMalformed, urn);
                }
            }

            /* foreach */
            foreach (string currentComponent in components)
            {
                if (string.IsNullOrWhiteSpace(currentComponent))
                {
                    throw new SdmxSemmanticException(ExceptionCode.StructureUrnMalformed, urn);
                }
            }

            string version = GetVersionFromUrn(urn);
            if (version == null && structureEnumType.EnumType != SdmxStructureEnumType.Agency)
            {
                throw new SdmxSemmanticException(ExceptionCode.StructureUrnMalformed, urn);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The remove versions from urn.
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string RemoveVersionsFromUrn(string urn)
        {
            if (urn == null)
            {
                return null;
            }

            int paraFirst = urn.IndexOf('(');
            int paraLast = urn.LastIndexOf(')') + 1;
            if (paraFirst > 0 && paraLast > paraFirst)
            {
                int i = paraLast - paraFirst;
                return urn.Remove(paraFirst, i);
            }

            int bracketFirst = urn.IndexOf('[');
            int bracketLast = urn.IndexOf(']');

            if (bracketFirst > 0 && bracketLast >= 0)
            {
                int i = bracketLast - bracketFirst;
                if (bracketFirst + i < urn.Length)
                {
                    _log.Warn("URN using pre 2.1 syntax [#version] upgrading to (#version) : " + urn);
                    return urn.Remove(bracketFirst, i);
                }
            }

            return urn;

            //// return urn.Replace("(?=\\().*?(?<=\\))", "");
        }

        /// <summary>
        /// Split <paramref name="urn"/> on '='
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <returns>
        /// The array with left and right parts of <paramref name="urn"/>
        /// </returns>
        private static string[] SplitUrn(string urn)
        {
            return urn.Split('=');
        }

        #endregion
    }
}