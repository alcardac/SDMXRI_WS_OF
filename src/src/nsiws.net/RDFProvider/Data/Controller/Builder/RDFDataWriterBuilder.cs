namespace RDFProvider.Controller.Builder
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.ServiceModel;
    using System.Xml;

    //using Org.Sdmxsource.Sdmx.DataParser.Engine;
    using Estat.Sri.Ws.Controllers.Engine;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Exception;    
    using RDFProvider.Writer.Engine;
    using RDFProvider.Controller.Engine;

    /// <summary>
    ///     The <see cref="IDataWriterEngine" /> engine builder from <see cref="XmlWriter" /> or <see cref="Stream" />
    /// </summary>
    public class RDFDataWriterBuilder : IWriterBuilderRDF<IRDFDataWriterEngine, XmlWriter>, IWriterBuilderRDF<IRDFDataWriterEngine, Stream>
    {
        #region Fields

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataWriterBuilder"/> class.
        /// </summary>
        /// <param name="dataFormat">
        /// The data Format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The sdmx Schema.
        /// </param>
        public RDFDataWriterBuilder()
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Builds the specified writer.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="actions">The actions.</param>
        /// <returns>
        /// The <see cref="IDataWriterEngine" />.
        /// </returns>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNotImplementedException">Not supported IDataWriterEngine for XmlWriter output</exception>
        public IRDFDataWriterEngine Build(XmlWriter writer, Queue<Action> actions)
        {                          
            return new RDFDataWriterEngine(writer);   
        }

        /// <summary>
        /// Builds the specified writer engine.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <exception cref="Org.Sdmxsource.Sdmx.Api.Exception.SdmxNotImplementedException">
        /// Not supported IDataWriterEngine for Stream output
        /// </exception>
        /// <returns>
        /// The <see cref="IDataWriterEngine"/>.
        /// </returns>
        public IRDFDataWriterEngine Build(Stream writer, Queue<Action> actions)
        {
            return this.Build(XmlWriter.Create(writer), actions);
        }

        #endregion
    }
}