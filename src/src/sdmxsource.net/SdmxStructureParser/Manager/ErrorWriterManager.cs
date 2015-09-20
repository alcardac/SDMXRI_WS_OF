using System;
using System.IO;

using Org.Sdmxsource.Sdmx.Api.Model;
using Org.Sdmxsource.Sdmx.Api.Engine;
using Org.Sdmxsource.Sdmx.Api.Factory;
using Org.Sdmxsource.Sdmx.Structureparser.Factory;
using Org.Sdmxsource.Sdmx.Api.Exception;
using Org.Sdmxsource.Sdmx.Api.Manager.Output;

namespace Org.Sdmxsource.Sdmx.Structureparser.Manager
{
    public class ErrorWriterManager : IErrorWriterManager
    {
        #region Static Fields

        /// <summary>
        /// The structure writer factory
        /// </summary>
        private readonly IErrorWriterFactory _errorWriterFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWritingManager"/> class.
        /// </summary>
        public ErrorWriterManager()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWritingManager"/> class.
        /// </summary>
        /// <param name="structureWriterFactory">
        /// The structure writer factory. If set to null the default factory will be used: <see cref="SdmxStructureWriterFactory"/>
        /// </param>
        public ErrorWriterManager(IErrorWriterFactory errorWriterFactory)
        {
            this._errorWriterFactory = errorWriterFactory ?? new SdmxErrorWriterFactory();
        }

        #endregion

        public int  WriteError(Exception ex, Stream outPutStream, IErrorFormat outputFormat)
        {
 	        return GetErrorWriterEngine(outputFormat).WriteError(ex, outPutStream);
        }
	
	    private IErrorWriterEngine GetErrorWriterEngine(IErrorFormat outputFormat) 
        {
            IErrorWriterEngine engine = this._errorWriterFactory.GetErrorWriterEngine(outputFormat);
            if (engine != null)
            {
                return engine;
            }
		    throw new SdmxNotImplementedException("Could not write error out in format: " + outputFormat);
	    }
    }
}
