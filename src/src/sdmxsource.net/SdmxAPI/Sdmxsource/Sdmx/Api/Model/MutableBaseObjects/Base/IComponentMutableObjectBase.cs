// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComponentMutableObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Base
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.MutableBaseObjects.ConceptScheme;

    #endregion

    /// <summary>
    ///     A component is something that can be conceptualised and where the values of the component
    ///     can also be taken from a Codelist, an example is a Dimensions
    /// </summary>
    public interface IComponentMutableObjectBase : IIdentifiableMutableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the codelist object.
        /// </summary>
        ICodelistMutableObjectBase CodelistObject { get; set; }

        /// <summary>
        ///     Gets or sets the concept object base.
        /// </summary>
        IConceptMutableObjectBase ConceptObjectBase { get; set; }

        #endregion
    }
}