// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossSectionalMeasureMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The CrossSectionalMeasureMutableObject interface.
    /// </summary>
    public interface ICrossSectionalMeasureMutableObject : IComponentMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the code.
        /// </summary>
        string Code { get; set; }

        /// <summary>
        ///     Gets or sets the measure dimension.
        /// </summary>
        string MeasureDimension { get; set; }

        #endregion
    }
}