// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttributeObjectBase.cs" company="Eurostat">
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

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;

    #endregion

    /// <summary>
    ///     An Attribute is a item of information used to add additional metadata to a key or observation or
    ///     group.
    /// </summary>
    public interface IAttributeObjectBase : IComponentObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the assignmentStatus attribute indicates whether a
        ///     value must be provided for the attribute when sending documentation along with the data.
        /// </summary>
        /// <value> </value>
        string AssignmentStatus { get; }

        /// <summary>
        ///     Gets the attachment group.
        /// </summary>
        string AttachmentGroup { get; }

        /// <summary>
        ///     Gets the ATTRIBUTE_ATTACHMENT_LEVEL attribute indicating the level to which the attribute is attached in time-series formats
        ///     (generic, compact, utility data formats).
        ///     Attributes with an attachment level of Group are only available if the data is organised in groups,
        ///     and should be used appropriately, as the values may not be communicated if the data is not grouped.
        /// </summary>
        /// <value> </value>
        AttributeAttachmentLevel AttachmentLevel { get; }

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        new IAttributeObject BuiltFrom { get; }

        /// <summary>
        ///     Gets the dimension reference.
        /// </summary>
        IList<string> DimensionReferences { get; }

        /// <summary>
        ///     Gets a value indicating whether the attribute is mandayory
        /// </summary>
        /// <value> </value>
        bool Mandatory { get; }

        /// <summary>
        ///     Gets the primary measure reference.
        /// </summary>
        string PrimaryMeasureReference { get; }

        #endregion
    }
}