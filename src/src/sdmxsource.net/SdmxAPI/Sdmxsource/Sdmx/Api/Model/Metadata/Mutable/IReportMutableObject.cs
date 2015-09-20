// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReportMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Metadata.Mutable
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;

    #endregion

    /// <summary>
    ///     The ReportMutableObject interface.
    /// </summary>
    public interface IReportMutableObject : IIdentifiableMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the reported attributes.
        /// </summary>
        IList<IReportedAttributeObject> ReportedAttributes { get; }

        /// <summary>
        ///     Gets or sets the target.
        /// </summary>
        ITarget Target { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add reported attribute.
        /// </summary>
        /// <param name="reportedAttribute">
        /// The reported attribute.
        /// </param>
        void AddReportedAttribute(IReportedAttributeObject reportedAttribute);

        #endregion
    }
}