// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGroup.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     A Group is responsible for grouping a subset of the Dimensions in a Data Structure and giving the group an id.
    ///     <p />
    ///     A group then contains its own unique id, which can then be referenced when reporting metadata attribtues.
    /// </summary>
    public interface IGroup : IIdentifiableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets reference to an attachment constraint which defines the cube to which the metadata can attach
        /// </summary>
        /// <value> </value>
        ICrossReference AttachmentConstraintRef { get; }

        /// <summary>
        ///     Gets the list of dimensions that this group is referencing - the list is in the order that the dimensions appear in the <c>KeyFamlyObject</c>.
        ///     The list is mutually exclusive with getAttachmentConstraintRef(), and will have a size of 1 or more only if getAttachmentConstraintRef()
        ///     is null, if getAttachmentConstraintRef() is not null then this method will return an empty list
        ///     <p />
        ///     <b>NOTE</b>The list is a copy so modify the returned set will not
        ///     be reflected in the IGroup instance
        /// </summary>
        /// <value> </value>
        IList<string> DimensionRefs { get; }

        #endregion
    }
}