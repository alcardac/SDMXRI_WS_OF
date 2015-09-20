// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureWritingEngineV21.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure writing engine for SDMX v21.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.Structureparser.Engine.Writing
{
    using System;
    using System.IO;
    using System.Xml;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21;
    using Org.Sdmxsource.Sdmx.Structureparser.Builder.XmlSerialization.V21.RegistryRequest;

    using Xml.Schema.Linq;

    /// <summary>
    ///     The structure writing engine for SDMX v21.
    /// </summary>
    public class StructureWriterEngineV21 : AbstractWritingEngine
    {
        #region Fields

        /// <summary>
        ///     The registration xml bean builder bean.
        /// </summary>
        private readonly SubmitRegistrationBuilder _registrationXmlBuilderBean = new SubmitRegistrationBuilder();

        /// <summary>
        ///     The structure xml bean builder bean.
        /// </summary>
        private readonly StructureXmlBuilder _structureXmlBuilderBean = new StructureXmlBuilder();

        /// <summary>
        ///     The submit subscription builder.
        /// </summary>
        private readonly SubmitSubscriptionBuilder _submitSubscriptionBuilder = new SubmitSubscriptionBuilder();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineV21"/> class. 
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> is null
        /// </exception>
        public StructureWriterEngineV21(XmlWriter writer)
            : base(writer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineV21"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        /// <param name="prettyFy">
        /// controls if output should be pretty (indented and no duplicate namespaces)
        /// </param>
        public StructureWriterEngineV21(Stream outputStream, bool prettyFy)
            : base(SdmxSchemaEnumType.VersionTwoPointOne, outputStream, prettyFy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureWriterEngineV21"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        public StructureWriterEngineV21(Stream outputStream)
            : base(SdmxSchemaEnumType.VersionTwoPointOne, outputStream, true)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Build the XSD generated class objects from the specified <paramref name="beans"/>
        /// </summary>
        /// <param name="beans">
        /// The beans.
        /// </param>
        /// <returns>
        /// the XSD generated class objects from the specified <paramref name="beans"/>
        /// </returns>
        protected internal override XTypedElement Build(ISdmxObjects beans)
        {
            bool hasStructures = beans.HasStructures();
            bool hasRegistrations = beans.HasRegistrations();
            bool hasSubscriptions = beans.HasSubscriptions();
            if (hasStructures && hasRegistrations)
            {
                throw new ArgumentException(
                    "Container sent to be written contains both structures and registrations, this can not be written out to a single SDMX Message");
            }

            if (hasStructures && hasRegistrations)
            {
                throw new ArgumentException(
                    "Container sent to be written contains both structures and subscriptions, this can not be written out to a single SDMX Message");
            }

            if (hasSubscriptions && hasStructures)
            {
                throw new ArgumentException(
                    "Container sent to be written contains both structures and subscriptions, this can not be written out to a single SDMX Message");
            }

            if (hasSubscriptions && hasRegistrations)
            {
                throw new ArgumentException(
                    "Container sent to be written contains both registrations and subscriptions, this can not be written out to a single SDMX Message");
            }

            if (hasRegistrations)
            {
                return this._registrationXmlBuilderBean.BuildRegistryInterfaceDocument(
                    beans.Registrations, DatasetActionEnumType.Append);
            }

            if (hasSubscriptions)
            {
                return this._submitSubscriptionBuilder.BuildRegistryInterfaceDocument(
                    beans.Subscriptions, DatasetActionEnumType.Append);
            }

            return this._structureXmlBuilderBean.Build(beans);
        }

        #endregion
    }
}