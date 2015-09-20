// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IConstraintAttachment.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The ConstraintAttachment interface.
    /// </summary>
    public interface IConstraintAttachment : ISdmxStructure
    {
        #region Public Properties

        /// <summary>
        ///   Gets the data provider and dataset id  If this constraint is built from a dataset/metadataset.
        /// </summary>
        /// <value> </value>
        IDataAndMetadataSetReference DataOrMetadataSetReference { get; }

        /// <summary>
        ///     Gets the datasource(s) that this constraint is built from
        /// </summary>
        /// <value> not null </value>
        IList<IDataSource> DataSources { get; }

        /// <summary>
        ///    Gets the structures that this constraint is attached to, this can be one or more of the following:
        ///  <ul>
        /// <li>Data Provider</li>
        /// <li>Data Structure</li>
        /// <li>Metadata Structure</li>
        /// <li>Data Flow</li>
        /// <li>Metadata Flow</li>
        /// <li>Provision Agreement</li>
        /// <li>Registration Object</li>
        /// </ul>  
        /// <b>NOTE: </b> This list of cross references can not be a mixed bag, it can be one or more OF THE SAME TYPE.
        /// </summary>
        ISet<ICrossReference> StructureReference { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The create mutable instance.
        /// </summary>
        /// <returns>
        ///     The <see cref="IConstraintAttachmentMutableObject" /> .
        /// </returns>
        IConstraintAttachmentMutableObject CreateMutableInstance();

        #endregion
    }
}