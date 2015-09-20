// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataWriterManager.cs" company="Eurostat">
//   Date Created : 2013-08-19
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.DataParser.Manager
{
    #region Using directives

    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.DataParser.Factory;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    ///     The data writer manager.
    /// </summary>
    public class DataWriterManager : IDataWriterManager
    {
         #region Fields

         /// <summary>
         ///     The factory
         /// </summary>
         private readonly IDataWriterFactory[] _factory;

         #endregion

         #region Constructors and Destructors

         /// <summary>
         /// Initializes a new instance of the <see cref="DataWriterManager"/> class.
         /// </summary>
         /// <param name="factory">
         /// The factory.
         /// </param>
         public DataWriterManager(params IDataWriterFactory[] factory)
         {
             this._factory = ObjectUtil.ValidArray(factory) ? factory : new IDataWriterFactory[] { new DataWriterFactory() };
         }

         #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get data writer engine.
        /// </summary>
        /// <param name="dataFormat">
        /// The data format. 
        /// </param>
        /// <param name="outPutStream">
        /// The output stream. 
        /// </param>
        /// <returns>
        /// The <see cref="IDataWriterEngine"/>.
        /// </returns>
        public virtual IDataWriterEngine GetDataWriterEngine(IDataFormat dataFormat, Stream outPutStream) 
        {
            foreach (IDataWriterFactory dwf in this._factory)
            {
                IDataWriterEngine dwe = dwf.GetDataWriterEngine(dataFormat, outPutStream);
                if (dwe != null)
                {
                    return dwe;
                }
            }

            throw new SdmxNotImplementedException("Could not write data out in type: " + dataFormat);
        }

        #endregion
    }
}
