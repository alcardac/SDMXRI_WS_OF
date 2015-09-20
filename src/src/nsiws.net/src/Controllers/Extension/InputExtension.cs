// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputExtension.cs" company="Eurostat">
//   Date Created : 2013-10-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The input extension.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.Ws.Controllers.Extension
{
    using System.IO;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    using Estat.Sri.Ws.Controllers.Constants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Util.Io;

    /// <summary>
    ///     The input extension.
    /// </summary>
    public static class InputExtension
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the readable data location.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
        /// The <see cref="IReadableDataLocation"/>.
        /// </returns>
        public static IReadableDataLocation GetReadableDataLocation(this XElement element)
        {
            return new MemoryReadableLocation(Encoding.UTF8.GetBytes(element.ToString()));
        }

        /// <summary>
        /// Gets the readable data location.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <param name="qualifiedName">
        /// The qualified Name.
        /// </param>
        /// <returns>
        /// The <see cref="IReadableDataLocation"/>.
        /// </returns>
        public static IReadableDataLocation GetReadableDataLocation(this Message element, XmlQualifiedName qualifiedName)
        {
            if (element == null || element.IsEmpty)
            {
                throw new SdmxException("Missing message", SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SemanticError));
            }

            var stream = new MemoryStream();
            using (var reader = element.GetReaderAtBodyContents())
            {
                if (!reader.ReadToDescendant(qualifiedName.Name, qualifiedName.Namespace))
                {
                    throw new SdmxException("Missing message", SdmxErrorCode.GetFromEnum(SdmxErrorCodeEnumType.SyntaxError));
                }

                using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings { Encoding = Encoding.UTF8 }))
                {
                    writer.WriteNode(reader, true);
                    writer.Flush();
                }
            }

            return new MemoryReadableLocation(stream.ToArray());
        }

        /// <summary>
        /// Gets the readable data location.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
        /// The <see cref="IReadableDataLocation"/>.
        /// </returns>
        public static IReadableDataLocation GetReadableDataLocation(this XmlNode element)
        {
            return new XmlDocReadableDataLocation(element);
        }

        /// <summary>
        /// Gets the SOAP operation.
        /// </summary>
        /// <param name="dataFormat">
        /// The data format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        /// <returns>
        /// The <see cref="SoapOperation"/>.
        /// </returns>
        public static SoapOperation GetSoapOperation(this BaseDataFormat dataFormat, SdmxSchemaEnumType sdmxSchema)
        {
            switch (sdmxSchema)
            {
                case SdmxSchemaEnumType.Edi:
                case SdmxSchemaEnumType.Ecv:
                case SdmxSchemaEnumType.Csv:
                case SdmxSchemaEnumType.Json:
                    return SoapOperation.Null;
            }

            switch (dataFormat.EnumType)
            {
                case BaseDataFormatEnumType.Generic:
                    return SoapOperation.GetGenericData;
                case BaseDataFormatEnumType.Compact:
                    return sdmxSchema == SdmxSchemaEnumType.VersionTwoPointOne ? SoapOperation.GetStructureSpecificData : SoapOperation.GetCompactData;
                case BaseDataFormatEnumType.Utility:
                    return sdmxSchema == SdmxSchemaEnumType.VersionTwoPointOne ? SoapOperation.Null : SoapOperation.GetUtilityData;
                case BaseDataFormatEnumType.CrossSectional:
                    return sdmxSchema == SdmxSchemaEnumType.VersionTwoPointOne ? SoapOperation.Null : SoapOperation.GetCrossSectionalData;
            }

            return SoapOperation.Null;
        }

        /// <summary>
        /// Gets the SOAP operation.
        /// </summary>
        /// <param name="structureOutputFormat">
        /// The structure output format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        /// <param name="structure">
        /// Type of the structure requested. (Only in SDMX v2.1 ).
        /// </param>
        /// <returns>
        /// The <see cref="SoapOperation"/>.
        /// </returns>
        public static SoapOperation GetSoapOperation(this StructureOutputFormatEnumType structureOutputFormat, SdmxSchemaEnumType sdmxSchema, SdmxStructureEnumType structure)
        {
            switch (structureOutputFormat)
            {
                case StructureOutputFormatEnumType.Null:
                    break;
                case StructureOutputFormatEnumType.SdmxV1StructureDocument:
                    break;
                case StructureOutputFormatEnumType.SdmxV2StructureDocument:
                    break;
                case StructureOutputFormatEnumType.SdmxV2RegistrySubmitDocument:
                    break;
                case StructureOutputFormatEnumType.SdmxV2RegistryQueryResponseDocument:
                    return SoapOperation.QueryStructure;
                case StructureOutputFormatEnumType.SdmxV21StructureDocument:
                    switch (structure)
                    {
                        case SdmxStructureEnumType.Dataflow:
                            return SoapOperation.GetDataflow;
                        case SdmxStructureEnumType.CodeList:
                            return SoapOperation.GetCodelist;
                        case SdmxStructureEnumType.Categorisation:
                            return SoapOperation.GetCategorisation;
                        case SdmxStructureEnumType.CategoryScheme:
                            return SoapOperation.GetCategoryScheme;
                        case SdmxStructureEnumType.ConceptScheme:
                            return SoapOperation.GetConceptScheme;
                        case SdmxStructureEnumType.Dsd:
                            return SoapOperation.GetDataStructure;
                        case SdmxStructureEnumType.HierarchicalCodelist:
                            return SoapOperation.GetHierarchicalCodelist;
                        default:
                            return SoapOperation.GetStructures;
                    }

                case StructureOutputFormatEnumType.SdmxV21RegistrySubmitDocument:
                    break;
                case StructureOutputFormatEnumType.SdmxV21QueryResponseDocument:
                    break;
                case StructureOutputFormatEnumType.Edi:
                    break;
                case StructureOutputFormatEnumType.Csv:
                    break;
                default:
                    return SoapOperation.Null;
            }

            return SoapOperation.Null;
        }

        /// <summary>
        /// Gets the SOAP operation response.
        /// </summary>
        /// <param name="dataFormat">
        /// The data format.
        /// </param>
        /// <param name="sdmxSchema">
        /// The SDMX schema.
        /// </param>
        /// <returns>
        /// The SOAP operation response.
        /// </returns>
        public static SoapOperationResponse GetSoapOperationResponse(this BaseDataFormat dataFormat, SdmxSchemaEnumType sdmxSchema)
        {
            return dataFormat.GetSoapOperation(sdmxSchema).GetResponse();
        }

        #endregion
    }
}