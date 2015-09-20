// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryInterfaceWriterV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   This class writes a SDMX Model IMutableObjects object to a stream
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.IO;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxStructureMutableParser.Properties;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Engine;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.StructureRetrieval.Manager;
    using Org.Sdmxsource.Sdmx.Util.Objects.Container;

    /// <summary>
    ///     This class writes a SDMX Model IMutableObjects object to a stream
    /// </summary>
    public class RegistryInterfaceWriterV2 : RegistryInterfaceWriterBaseV2, IStructureWriterEngine
    {
        #region Fields

        /// <summary>
        /// The _close XML writer
        /// </summary>
        private readonly bool _closeXmlWriter;

        /// <summary>
        ///     The internal filed containing the SDMX Model structure object
        /// </summary>
        private IRegistryInfo _registryInterface;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryInterfaceWriterV2"/> class
        /// </summary>
        /// <param name="writer">
        /// The output stream to actually perform the writing
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// writer is null
        /// </exception>
        public RegistryInterfaceWriterV2(Stream writer)
            : this(XmlWriter.Create(writer))
        {
            if (!writer.CanWrite)
            {
                throw new ArgumentException(Resources.ExcepteptionCannotWriteToStream, "writer");
            }

            this._closeXmlWriter = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryInterfaceWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        public RegistryInterfaceWriterV2(XmlWriter writer)
            : this(writer, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryInterfaceWriterV2"/> class.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        public RegistryInterfaceWriterV2(XmlWriter writer, SdmxNamespaces namespaces)
            : base(writer, namespaces)
        {
            this.HeaderRetrievalManager = new HeaderRetrievalManager();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// This is the main method of the class and is used to write the <see cref="IRegistryInfo"/>
        ///     to the <see cref="System.Xml.XmlTextWriter"/> given in the constructor
        /// </summary>
        /// <param name="registryInterface">
        /// The <see cref="IRegistryInfo"/> object we want to write
        /// </param>
        public void WriteRegistryInterface(IRegistryInfo registryInterface)
        {
            if (registryInterface == null)
            {
                throw new ArgumentNullException("registryInterface");
            }

            this._registryInterface = registryInterface;
            this.WriteMessageTag(ElementNameTable.RegistryInterface);

            // print message header
            this.WriteMessageHeader(this._registryInterface.Header);

            // write QueryStructureRequest
            if (this._registryInterface.QueryStructureRequest != null)
            {
                var writer = new QueryStructureRequestWriterV2(this.SdmxMLWriter, this.Namespaces);
                writer.Write(registryInterface);
            }

            // write QueryStructureResponse
            if (this._registryInterface.QueryStructureResponse != null)
            {
                var writer = new QueryStructureResponseWriterV2(this.SdmxMLWriter, this.Namespaces);
                writer.Write(registryInterface);
            }

            // close document
            this.WriteEndElement();
            this.WriteEndDocument();
            if (this._closeXmlWriter)
            {
                this.SdmxMLWriter.Close();
            }
        }

        /// <summary>
        /// Writes the <paramref name="maintainableObject"/> out to the output location in the format specified by the implementation
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainableObject.
        /// </param>
        public void WriteStructure(IMaintainableObject maintainableObject)
        {
            IMutableObjects mutableObjects = new MutableObjectsImpl();
            mutableObjects.AddIdentifiable(maintainableObject.MutableInstance);
            IRegistryInfo registry = new RegistryInfo();
            registry.QueryStructureResponse = new QueryStructureResponseInfo { Structure = mutableObjects };
            this.WriteRegistryInterface(registry);
        }

        /// <summary>
        /// Writes the sdmxObjects to the output location in the format specified by the implementation
        /// </summary>
        /// <param name="sdmxObjects">
        /// SDMX objects
        /// </param>
        public void WriteStructures(ISdmxObjects sdmxObjects)
        {
            IRegistryInfo registry = new RegistryInfo();
            registry.QueryStructureResponse = new QueryStructureResponseInfo { Structure = sdmxObjects.MutableObjects, StatusMessage = new StatusMessageInfo { Status = Status.Success } };
            this.WriteRegistryInterface(registry);
        }

        #endregion
    }
}