// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EdiParseManager.cs" company="Eurostat">
//   Date Created : 2014-07-29
//   //   Copyright (c) 2014 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The EDI parse manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.EdiParser.Manager
{
    using System.Collections.Generic;
    using System.IO;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Factory;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.EdiParser.Engine;
    using Org.Sdmxsource.Sdmx.EdiParser.Model;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     The EDI parse manager.
    /// </summary>
    public class EdiParseManager : IEdiParseManager
    {
        #region Fields

        /// <summary>
        /// The _edi parse engine.
        /// </summary>
        private readonly IEdiParseEngine _ediParseEngine;

        /// <summary>
        /// The _writeable data location factory.
        /// </summary>
        private readonly IWriteableDataLocationFactory _writeableDataLocationFactory;

        /// <summary>
        /// The EDI structure engine.
        /// </summary>
        private readonly IEdiStructureWriterEngine _ediStructureEngine;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiParseManager"/> class.
        /// </summary>
        public EdiParseManager()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EdiParseManager"/> class.
        /// </summary>
        /// <param name="writeableDataLocationFactory">
        /// The writeable data location factory.
        /// </param>
        /// <param name="parseEngine">
        /// The parse engine.
        /// </param>
        /// <param name="ediStructureEngine">
        /// The edi structure engine.
        /// </param>
        public EdiParseManager(IWriteableDataLocationFactory writeableDataLocationFactory, IEdiParseEngine parseEngine, IEdiStructureWriterEngine ediStructureEngine)
        {
            this._ediStructureEngine = ediStructureEngine;
            this._writeableDataLocationFactory = writeableDataLocationFactory ?? new WriteableDataLocationFactory();
            this._ediParseEngine = parseEngine ?? new EdiParseEngine();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Processes an EDI message and returns a workspace containing the SDMX structures and data that were contained in the
        ///     message
        /// </summary>
        /// <param name="ediMessageLocation">
        /// The EDI message location.
        /// </param>
        /// <returns>
        /// The <see cref="IEdiWorkspace"/>.
        /// </returns>
        public IEdiWorkspace ParseEdiMessage(IReadableDataLocation ediMessageLocation)
        {
            return new EdiWorkspace(ediMessageLocation, this._writeableDataLocationFactory, this._ediParseEngine);
        }

        /// <summary>
        /// Writes the <paramref name="objects"/> contents out as EDI-TS to the <paramref name="output"/>
        /// </summary>
        /// <param name="objects">
        /// The objects.
        /// </param>
        /// <param name="output">
        /// The output.
        /// </param>
        public void WriteToEdi(ISdmxObjects objects, Stream output)
        {
            if (this._ediStructureEngine == null)
            {
                throw new SdmxNotImplementedException("IEdiStructureWriterEngine not provided.");
            }

            // Check that what is being written is supported by EDI
            ValidateSupport(objects);

            this._ediStructureEngine.WriteToEDI(objects, output);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates all the Maintainable Artefacts in the beans container are supported by the SDMX v1.0 syntax
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <exception cref="SdmxNotImplementedException">
        /// Structure not supported by SDMX-EDI
        /// </exception>
        private static void ValidateSupport(ISdmxObjects beans)
        {
            IList<SdmxStructureEnumType> supportedStructres = new List<SdmxStructureEnumType>();
            supportedStructres.Add(SdmxStructureEnumType.AgencyScheme);
            supportedStructres.Add(SdmxStructureEnumType.Dsd);
            supportedStructres.Add(SdmxStructureEnumType.ConceptScheme);
            supportedStructres.Add(SdmxStructureEnumType.CodeList);

            foreach (var maintainableBean in beans.GetAllMaintainables())
            {
                if (!supportedStructres.Contains(maintainableBean.StructureType.EnumType))
                {
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, maintainableBean.StructureType.StructureType + " is not a supported by SDMX-EDI");
                }
            }
        }

        #endregion
    }
}