// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodelistMutableObject.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Model.Mutable.Codelist
{
    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Codelist;

    #endregion

    /// <summary>
    ///     The CodelistMutableObject interface.
    /// </summary>
    public interface ICodelistMutableObject : IItemSchemeMutableObject<ICodeMutableObject>
    {
        #region Public Properties

        /// <summary>
        ///     Gets a representation of itself in a Object which can not be modified, modifications to the mutable Object
        ///     are not reflected in the returned instance of the IMaintainableObject.
        /// </summary>
        /// <value> </value>
        new ICodelistObject ImmutableInstance { get; }

        /// <summary>
        /// 
        /// </summary>
        new bool IsPartial { get; set; }

        #endregion

        /// <summary>
        /// Returns the code with the given id, returns null if no such code exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ICodeMutableObject GetCodeById(string id);
    }
}