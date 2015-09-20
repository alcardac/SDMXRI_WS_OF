// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAttributeMutableObject.cs" company="Eurostat">
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

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     The AttributeMutableObject interface.
    /// </summary>
    public interface IAttributeMutableObject : IComponentMutableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the assignment status.
        ///     The assignmentStatus attribute indicates whether a value must be provided for the attribute when sending documentation along with the data.
        /// </summary>
        string AssignmentStatus { get; set; }

        /// <summary>
        ///     Gets or sets the attachment group.
        /// </summary>
        string AttachmentGroup { get; set; }

        /// <summary>
        ///     Gets or sets the attachment level.
        ///     Attributes with an attachment level of Group are only available if the data is organized in groups,
        ///     and should be used appropriately, as the values may not be communicated if the data is not grouped.
        /// </summary>
        AttributeAttachmentLevel AttachmentLevel { get; set; }

        /// <summary>
        ///     Gets the concept roles.
        /// </summary>
        IList<IStructureReference> ConceptRoles { get; }

        /// <summary>
        ///     Gets the dimension reference.
        /// </summary>
        IList<string> DimensionReferences { get; }

        /// <summary>
        ///     Gets or sets the primary measure reference.
        /// </summary>
        string PrimaryMeasureReference { get; set; }

        #endregion

        /// <summary>
        /// Adds a concept Role to the existing list of concept roles
        /// </summary>
        /// <param name="structureReference"></param>
        void AddConceptRole(IStructureReference structureReference);
    }
}