// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentifiableRefObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Used to reference an SDMX identifiable artifact
    /// </summary>
    public interface IIdentifiableRefObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a reference to the child identifiable artifact that is being referenced.
        ///     returns null if there is no child identifiable being referenced (i.e. this is the end of the reference chain)
        /// </summary>
        /// <value> </value>
        IIdentifiableRefObject ChildReference { get; }

        /// <summary>
        ///     Gets the id of the identifiable that is being referenced (at this level of the referencing hierarchy)
        /// </summary>
        /// <value> </value>
        string Id { get; }

        /// <summary>
        ///     Gets a reference to the parent identifiable artifact (not maintainable) that this identifiable
        ///     artifact is a child of, returns null if the identifiable artifact has no parent identifiable artifact
        /// </summary>
        /// <value> </value>
        IIdentifiableRefObject ParentIdentifiableReference { get; }

        /// <summary>
        ///     Gets a reference to the parent maintainable that this identifiable is a child of, this will never be null
        /// </summary>
        /// <value> </value>
        IStructureReference ParentMaintainableReferece { get; }

        /// <summary>
        ///     Gets the identifiable structure that is being referenced (at this level of the referencing hierarchy)
        /// </summary>
        /// <value> </value>
        SdmxStructureType StructureEnumType { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the matched identifiable @object, from the given identifiable - returns null if no match is found
        /// </summary>
        /// <param name="reference">The referemce. </param>
        /// <returns>
        /// The <see cref="IIdentifiableObject"/> .
        /// </returns>
        IIdentifiableObject GetMatch(IIdentifiableObject reference);

        #endregion
    }
}