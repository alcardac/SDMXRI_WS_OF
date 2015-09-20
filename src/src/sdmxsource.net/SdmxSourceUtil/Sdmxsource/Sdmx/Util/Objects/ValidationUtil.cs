// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationUtil.cs" company="Eurostat">
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

    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    #endregion


    /// <summary>
    /// The validation util.
    /// </summary>
    public class ValidationUtil
    {
        #region Static Fields

        /// <summary>
        /// The _id pattern.
        /// </summary>
        private static readonly Regex _idPattern = new Regex(
            "^([A-Z]|[a-z])+([A-Z]|[a-z]|\\\\|\\*|@|[0-9]|_|\\$|\\-)*$", RegexOptions.Compiled);

        /// <summary>
        /// The _id pattern int allowed.
        /// </summary>
        private static readonly Regex _idPatternIntAllowed = new Regex(
            "^([A-Z]|[a-z]|\\\\|\\*|@|[0-9]|_|\\$|\\-)*$", RegexOptions.Compiled);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The clean and validate id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="startWithIntAllowed">
        /// The start with int allowed.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="SdmxSemmanticException">Throws SdmxSemmanticException
        /// </exception>
        public static string CleanAndValidateId(string id, bool startWithIntAllowed)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            string trimedId = id.Trim();
            Regex idPattern;

            if (startWithIntAllowed)
            {
                idPattern = _idPatternIntAllowed;
            }
            else
            {
                idPattern = _idPattern;
            }

            if (!idPattern.IsMatch(trimedId))
            {
                if (startWithIntAllowed)
                {
                    throw new SdmxSemmanticException(ExceptionCode.StructureInvalidId, trimedId);
                }

                throw new SdmxSemmanticException(ExceptionCode.StructureInvalidIdStartAlpha, trimedId);
            }

            return trimedId;
        }

        /// <summary>
        /// Validates that the locale in the text type is unique
        /// </summary>
        /// <param name="textTypes">List of text types
        /// </param>
        /// <param name="validPatternStr">
        /// The valid Pattern Str.
        /// </param>
        /// <exception cref="SdmxSemmanticException">Throws SdmxSemmanticException
        /// </exception>
        public static void ValidateTextType(IList<ITextTypeWrapper> textTypes, string validPatternStr)
        {
            Regex regex = null;

            if (!string.IsNullOrWhiteSpace(validPatternStr))
            {
                regex = new Regex(validPatternStr, RegexOptions.Compiled);
            }

            ISet<string> languages = new HashSet<string>();
            if (textTypes != null)
            {
                foreach (ITextTypeWrapper currentTextType in textTypes)
                {
                    if (regex != null)
                    {
                        if (!regex.IsMatch(currentTextType.Value))
                        {
                            throw new SdmxSemmanticException(
                                currentTextType.Value + " invalid with respect to allowed string : " + validPatternStr);
                        }
                    }

                    if (languages.Contains(currentTextType.Locale))
                    {
                        throw new SdmxSemmanticException(ExceptionCode.DuplicateLanguage, currentTextType.Locale);
                    }

                    languages.Add(currentTextType.Locale);
                }
            }
        }

        #endregion
    }
}