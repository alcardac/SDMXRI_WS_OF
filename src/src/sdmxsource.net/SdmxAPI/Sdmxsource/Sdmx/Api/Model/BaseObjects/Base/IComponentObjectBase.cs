// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IComponentObjectBase.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Base
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.Codelist;
    using Org.Sdmxsource.Sdmx.Api.Model.BaseObjects.ConceptScheme;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     A component is something that can be conceptualised and where the values of the component
    ///     can also be taken from a Codelist, an example is a Dimensions
    /// </summary>
    public interface IComponentObjectBase : IIdentifiableObjectBase
    {
        #region Public Properties

        /// <summary>
        ///     Gets the built from.
        /// </summary>
        new IComponent BuiltFrom { get; }

        /// <summary>
        ///     Gets the concept, this is mandatory and will always return a value
        /// </summary>
        /// <value> </value>
        IConceptObjectBase Concept { get; }

        /// <summary>
        ///     Gets the text format, this may be null if there is none
        /// </summary>
        /// <value> </value>
        ITextFormat TextFormat { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the codelist, this may be null if there is none
        /// </summary>
        /// <param name="useConceptIfRequired">
        /// if the representation is uncoded, but the concept has default representation, then the codelist for the concept will be returned if this parameter is set to true
        /// </param>
        /// <returns>
        /// The <see cref="ICodelistObjectBase"/> .
        /// </returns>
        ICodelistObjectBase GetCodelist(bool useConceptIfRequired);

        #endregion
    }
}