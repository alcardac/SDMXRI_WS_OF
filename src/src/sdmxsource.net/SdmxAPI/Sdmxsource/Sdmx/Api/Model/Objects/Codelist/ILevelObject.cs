// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILevelObject.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    #endregion

    /// <summary>
    ///     Represents an SDMX Level as defined by a IHierarchy
    /// </summary>
    /// <seealso cref="T:Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist.IHierarchy" />
    public interface ILevelObject : INameableObject
    {
        #region Public Properties

        /// <summary>
        ///     Gets the child level, returns null if there is no child
        /// </summary>
        /// <value> </value>
        ILevelObject ChildLevel { get; }

        /// <summary>
        ///     Gets the coding format.
        /// </summary>
        /// <value> the codeing format for this level </value>
        ITextFormat CodingFormat { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     Gets a value indicating whether the there is a child level, in which case the call to getChildLevel will always return a value
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        bool HasChild();

        #endregion
    }
}