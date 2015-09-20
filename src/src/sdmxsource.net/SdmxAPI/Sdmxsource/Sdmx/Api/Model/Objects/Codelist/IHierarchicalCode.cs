// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHierarchicalCode.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist
{
    #region Using directives

    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion

    /// <summary>
    ///     Represents an SDMX IHierarchical Code
    /// </summary>
    public interface IHierarchicalCode : IIdentifiableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the code id of the code being referenced, to be used in conjunction with the codelist alias ref
        /// </summary>
        /// <value> </value>
        string CodeId { get; }

        /// <summary>
        ///     Gets the code referenced by this CodeRef, this will never be null as it will be resolved from the codelist alias and code id
        /// </summary>
        /// <value> </value>
        ICrossReference CodeReference { get; }

        /// <summary>
        ///     Gets any child IHierarchicalCode Objects as a copy of the underlying list.
        ///     <p />
        ///     Gets an empty list if there are no child Objects
        /// </summary>
        IList<IHierarchicalCode> CodeRefs { get; }

        /// <summary>
        ///     Gets the codelist alias used to resolve this codelist reference, returns null if the reference is achieved by using the
        ///     ICrossReference (getCodeReference() returns a value)
        /// </summary>
        /// <value> </value>
        string CodelistAliasRef { get; }

        /// <summary>
        ///     Gets the level of this code ref @object in the hierarchy, 0 indexed
        /// </summary>
        /// <value> </value>
        int LevelInHierarchy { get; }

        /// <summary>
        ///     Gets the valid from.
        /// </summary>
        ISdmxDate ValidFrom { get; }

        /// <summary>
        ///     Gets the valid to.
        /// </summary>
        ISdmxDate ValidTo { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get level.
        /// </summary>
        /// <param name="acceptDefault">
        /// level was not explicitly set on creation, if false, then will return the level if it was explictly set, and will return null if it wasn't.
        /// </param>
        /// <returns>
        /// the level associated with this code
        /// </returns>
        ILevelObject GetLevel(bool acceptDefault);

        #endregion
    }
}