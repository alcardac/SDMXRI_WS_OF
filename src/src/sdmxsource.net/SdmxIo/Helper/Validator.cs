// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Validator.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This is a utility class for checking a DSD for compliance
//   with producing Compact or Cross-Sectional Data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxParseBase.Helper
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    using Estat.Sri.SdmxParseBase.Properties;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    /// <summary>
    ///     This is a utility class for checking a DSD for compliance
    ///     with producing Compact or Cross-Sectional Data.
    /// </summary>
    public static class Validator
    {
        #region Public Methods and Operators

        /// <summary>
        /// This method checks a DSD for compliance with producing Compact Data.
        ///     In detail, it checks that if a TimeDimension is present and at least
        ///     one dimension is frequency dimension. If there is none an error message
        ///     is returned to the caller
        /// </summary>
        /// <param name="keyFamily">
        /// The <see cref="IDataStructureObject"/>of the DSD to be checked
        /// </param>
        /// <returns>
        /// The error messages in case of invalid DSD or an empty string in case a valid DSD
        /// </returns>
        public static string ValidateForCompact(IDataStructureObject keyFamily)
        {
            string text = string.Empty;
            bool isFrequency = false;

            foreach (IDimension dimension in keyFamily.DimensionList.Dimensions)
            {
                if (dimension.FrequencyDimension)
                {
                    isFrequency = true;
                    break;
                }
            }

            if (keyFamily.TimeDimension == null)
            {
                text = string.Format(
                    CultureInfo.InvariantCulture, Resources.ErrorNoTimeDimensionFormat2, keyFamily.Id, keyFamily.Version);
            }
            else if (!isFrequency)
            {
                // normally it should never reach here
                text = "DSD " + keyFamily.Id + " v" + keyFamily.Version
                       + " does not have a Frequency dimension. According SDMX v2.0: Any DSD which uses the Time dimension must also declare a frequency dimension.";
            }

            return text;
        }

        /// <summary>
        /// This method checks a DSD for compliance with producing  Cross-Sectional Data.
        ///     In detail, it checks all Dimensions and all Attributes for having or not having
        ///     cross-sectional attachment group. If there are components with no attachment level,
        ///     it returns a list with them in the message.
        /// </summary>
        /// <param name="keyFamily">
        /// The <see cref="IDataStructureObject"/>of the DSD to be checked
        /// </param>
        /// <returns>
        /// The error messages in case of invalid DSD or an empty string in case a valid DSD
        /// </returns>
        public static string ValidateForCrossSectional(IDataStructureObject keyFamily)
        {
            var crossSectionalDataStructure = keyFamily as ICrossSectionalDataStructureObject;

            if (crossSectionalDataStructure == null)
            {
                return "Not a cross-sectional DSD";
            }

            return string.Empty;
        }

        #endregion
    }
}