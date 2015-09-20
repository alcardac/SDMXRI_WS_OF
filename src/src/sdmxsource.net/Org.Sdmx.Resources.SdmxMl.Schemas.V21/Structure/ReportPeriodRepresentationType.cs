// -----------------------------------------------------------------------
// <copyright file="ReportPeriodRepresentationType.cs" company="Eurostat">
//   Date Created : 2012-11-19
//   Copyright (c) 2012 by the European   Commission, represented by Eurostat.   All rights reserved.
// 
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure
{
    /// <summary>
    /// <para>
    /// ReportPeriodRepresentationType defines the possible local representations of a report period target object. The reprentation must be a time period or a subset of this representation.
    /// </para>
    /// </summary>
    public partial class ReportPeriodRepresentationType
    {
        /// <summary>
        /// Add new <see cref="TextFormatType"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="TextFormatType"/>.
        /// </returns>
        public override TextFormatType AddNewTextFormatType()
        {
            return this.TextFormat = new TimeTextFormatType();
        }
    }
}