// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeListUtil.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Utility methods for dealing with CodeLists.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Util
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist;

    /// <summary>
    ///   Utility methods for dealing with CodeLists.
    /// </summary>
    public class CodeListUtil
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns the size of the longest code contained in the CodeList.
        ///   Zero will be returned if there are no codes in the CodeList.
        /// </summary>
        /// <param name="codelistObject">CodeList object
        /// </param>
        /// <returns>
        /// The size of the longest code or zero. 
        /// </returns>
        public static int DetermineMaxCodeLength(ICodelistObjectBase codelistObject)
        {
            IList<ICodeObjectBase> codes = codelistObject.Codes;
            int longestCodeLength = 0;
            foreach (ICodeObjectBase codeObjectBase in codes)
            {
                longestCodeLength = Math.Max(DetermineMaxCodeLength(codeObjectBase), longestCodeLength);
            }

            return longestCodeLength;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The determine max code length.
        /// </summary>
        /// <param name="codeObject">
        /// The code object. 
        /// </param>
        /// <returns>
        /// The <see cref="int"/> . 
        /// </returns>
        private static int DetermineMaxCodeLength(ICodeObjectBase codeObject)
        {
            int longestCodeLength = codeObject.Id.Length;
            if (codeObject.HasChildren())
            {
                foreach (ICodeObjectBase child in codeObject.Children)
                {
                    longestCodeLength = Math.Max(DetermineMaxCodeLength(child), longestCodeLength);
                }
            }

            return longestCodeLength;
        }

        #endregion
    }
}