// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttribute.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Attribute describes the definition of a data attribute, which is defined as a characteristic of an object or entity.
    ///     <p />
    ///     This is an immutable Object - this Object can not be modified
    /// </summary>
    public interface IAttributeObject : IComponent
    {
        #region Public Properties

        /// <summary>
        ///     Gets the assignmentStatus attribute indicates whether a
        ///     value must be provided for the attribute when sending documentation along with the data.
        /// </summary>
        /// <value> </value>
        string AssignmentStatus { get; }

        /// <summary>
        ///     Gets the AttachmentGroup value;
        ///     ATTRIBUTE_ATTACHMENT_LEVEL is GROUP then returns a reference to the Group id that this attribute is attached to.  Returns
        ///     null if ATTRIBUTE_ATTACHMENT_LEVEL is not GROUP
        /// </summary>
        /// <value> </value>
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
        ///     Gets a list of cross references to concepts that are used to define the role(s) of this attribute.
        ///     The returned list is a copy of the underlying list
        ///     <p />
        ///     Gets an empty list if this attribute does not reference any concept roles
        /// </summary>
        /// <value> </value>
        IList<ICrossReference> ConceptRoles { get; }

        /// <summary>
        ///     Gets the list of dimension ids that this attribute references, this is only relevant if ATTRIBUTE_ATTACHMENT_LEVEL is
        ///     DIMENSION_GROUP.  The returned list is a copy of the underlying list
        ///     <p />
        ///     Gets an empty list if this attribute does not reference any dimensions
        /// </summary>
        /// <value> </value>
        IList<string> DimensionReferences { get; }

        /// <summary>
        ///     Gets a value indicating whether the getAssignmentStatus()==MANDATORY
        /// </summary>
        /// <value> </value>
        bool Mandatory { get; }

        /// <summary>
        ///     Gets the PrimaryMeasureReference value
        ///     ATTRIBUTE_ATTACHMENT_LEVEL is OBSERVATION then returns a reference to the Primary Measure id that this attribute is attached to.  Returns
        ///     null if ATTRIBUTE_ATTACHMENT_LEVEL is not GROUP
        /// </summary>
        /// <value> </value>
        string PrimaryMeasureReference { get; }

        /// <summary>
        ///      Gets a value indicating whether this Id = TIME_FORMAT
        /// </summary>
        /// <value> </value>
        bool TimeFormat { get; }

        #endregion
    }
}