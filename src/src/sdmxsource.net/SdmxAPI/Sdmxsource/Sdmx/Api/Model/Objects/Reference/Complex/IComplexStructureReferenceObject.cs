// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexStructureReferenceObject.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference.Complex
{
    /// <summary>
    ///     Defines the structure parameters for querying for a structure.
    /// </summary>
    public interface IComplexStructureReferenceObject : IComplexNameableReference
    {
        #region Public Properties

        /// <summary>
        ///     Gets the agency id.
        /// </summary>
        IComplexTextReference AgencyId { get; }

        /// <summary>
        ///     Gets the child reference
        /// </summary>
        IComplexIdentifiableReferenceObject ChildReference { get; }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        IComplexTextReference Id { get; }

        /// <summary>
        ///     Gets the version reference.
        /// </summary>
        IComplexVersionReference VersionReference { get; }

        #endregion
    }
}