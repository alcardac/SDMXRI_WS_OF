// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataTransform.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Transform
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.DataParser.Model;

    #endregion

    /// <summary>
    /// The DataTransform interface.
    /// </summary>
    public interface IDataTransform
    {
        #region Public Methods and Operators

        /// <summary>
        /// Transforms the specified data parse metadata.
        /// </summary>
        /// <param name="dataParseMetadata">The data parse metadata</param>
        void Transform(DataParseMetadata dataParseMetadata);

        #endregion
    }
}
