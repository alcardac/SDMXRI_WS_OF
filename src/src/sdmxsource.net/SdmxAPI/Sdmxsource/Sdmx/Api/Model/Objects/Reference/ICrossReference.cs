// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrossReference.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     A Cross Reference is a fully formed (all parameters present) immutable structure reference.
    ///     <p />
    ///     A Cross Reference uniquely references a single structure
    /// </summary>
    public interface ICrossReference : IStructureReference
    {
        #region Public Properties

        /// <summary>
        ///     Gets the structure that this cross reference belongs to
        /// </summary>
        /// <value> </value>
        ISdmxObject ReferencedFrom { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Creates a mutable instance of this cross reference
        /// </summary>
        /// <returns>
        ///     The <see cref="IStructureReference" /> .
        /// </returns>
        IStructureReference CreateMutableInstance();

        /// <summary>
        /// Gets a value indicating whether the the identifiable @object is a match for this cross reference
        /// </summary>
        /// <param name="identifiableObject">Identifiable Object
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> .
        /// </returns>
        bool IsMatch(IIdentifiableObject identifiableObject);

        #endregion
    }
}