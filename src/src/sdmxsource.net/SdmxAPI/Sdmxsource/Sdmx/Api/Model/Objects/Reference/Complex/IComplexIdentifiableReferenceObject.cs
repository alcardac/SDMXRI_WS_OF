// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComplexIdentifiableReferenceObject.cs" company="Eurostat">
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
    /// Used to idenftify identifiable structures
    /// </summary>
    public interface IComplexIdentifiableReferenceObject : IComplexNameableReference
    {
        /// <summary>
        /// Gets the Id of the item to be returned
        /// </summary>
        IComplexTextReference Id { get; }

        /// <summary>
        /// Gets a child reference, null if there is not child reference 
        /// </summary>
        IComplexIdentifiableReferenceObject ChildReference { get; }

    }
}
