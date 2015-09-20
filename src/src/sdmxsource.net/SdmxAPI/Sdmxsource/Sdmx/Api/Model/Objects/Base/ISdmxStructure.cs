// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISdmxStructure.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Objects.Base
{
    #region Using directives

    using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     An ISdmxStructure is a @object which represents an SDMX Structural metadata artefact, such as a Code / Codelist etc.
    /// </summary>
    public interface ISdmxStructure : ISdmxObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets a set of identifiable that are contained within this identifiable
        /// </summary>
        /// <value> </value>
        ISet<IIdentifiableObject> IdentifiableComposites { get; }

        /// <summary>
        ///     Gets the first identifiable parent of this SDMXObject
        ///     <p />
        ///     If this is a MaintainableObject, then there will be no parent to return, so will return a value of null
        /// </summary>
        /// <value> </value>
        IIdentifiableObject IdentifiableParent { get; }

        /// <summary>
        ///     Gets the maintainable parent, by recurring up the parent tree to find
        ///     If this is a maintainable then it will return a reference to itself.
        /// </summary>
        /// <value> </value>
        IMaintainableObject MaintainableParent { get; }

        /// <summary>
        ///     Gets the parent that this SdmxObject belongs to.
        ///     If this is a Maintainable Object, then there will be no parent to return, so will return a value of null
        /// </summary>
        /// <value> </value>
        new ISdmxStructure Parent { get; }

        #endregion
    }
}