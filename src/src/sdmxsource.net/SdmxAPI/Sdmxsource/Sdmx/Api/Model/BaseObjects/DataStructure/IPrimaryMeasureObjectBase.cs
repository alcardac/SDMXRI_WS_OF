// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPrimaryMeasureObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.DataStructure
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;

    #endregion

    /// <summary>
    ///     This is the measure used in time series data.  There can only be one primary measure.
    /// </summary>
    public interface IPrimaryMeasureObjectBase : IComponentObjectBase
    {
    }
}