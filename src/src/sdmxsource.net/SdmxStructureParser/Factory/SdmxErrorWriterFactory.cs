using Org.Sdmxsource.Sdmx.Api.Engine;
using Org.Sdmxsource.Sdmx.Api.Factory;
using Org.Sdmxsource.Sdmx.Api.Model;
using Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing;
using Org.Sdmxsource.Sdmx.Structureparser.Model;

namespace Org.Sdmxsource.Sdmx.Structureparser.Factory
{
    /// <summary>
    /// The SDMX error writer factory.
    /// </summary>
    public class SdmxErrorWriterFactory : IErrorWriterFactory
    {
        /// <summary>
        /// The error writer engine.
        /// </summary>
        private readonly ErrorWriterEngineV21 _errorWriterEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxErrorWriterFactory"/> class.
        /// </summary>
        public SdmxErrorWriterFactory()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxErrorWriterFactory"/> class.
        /// </summary>
        /// <param name="errorWriterEngine">
        /// The error writer engine.
        /// </param>
        public SdmxErrorWriterFactory(ErrorWriterEngineV21 errorWriterEngine)
        {
            this._errorWriterEngine = errorWriterEngine ?? new ErrorWriterEngineV21();
        }

        /// <summary>
        /// Returns an error writer in the format specified.  Returns null if the format is unknown by the implementation
        /// </summary>
        /// <param name="format">
        /// The format
        /// </param>
        /// <returns>
        /// Engine, or null if the format is unknown to this factory
        /// </returns>
        public IErrorWriterEngine GetErrorWriterEngine(IErrorFormat format)
        {
            if (format is SdmxErrorFormat) 
            {
			    return this._errorWriterEngine;
		    }
		    return null;
        }
    }
}
