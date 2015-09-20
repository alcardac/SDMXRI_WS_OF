// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISchemaWriterFactory.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Api.Factory
{

    #region Using directives

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Format;

    #endregion

    /// <summary>
    /// Returns a SchemaWriterEngine if it understands the SchemaFormat
    /// </summary>
    public interface ISchemaWriterFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns the SchemaWriterEngine that can write schema in the given format, returns null if the format is unknown to this factory
        /// </summary>
        /// <param name="format">
        /// The format to write the schema in
        /// </param>
        /// <returns>
        /// The schema writer engine
        /// </returns>
        ISchemaWriterEngine GetSchemaWriterEngine(ISchemaFormat format);

        #endregion
    }
}
