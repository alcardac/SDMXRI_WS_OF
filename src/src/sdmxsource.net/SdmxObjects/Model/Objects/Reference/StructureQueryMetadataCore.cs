// -----------------------------------------------------------------------
// <copyright file="StructureQueryMetadataCore.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference
{
    #region Using directives

    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Query;

    #endregion

    /// <summary>
    ///   TODO: Update summary.
    /// </summary>
    public class StructureQueryMetadataCore : IStructureQueryMetadata
    {
        private StructureQueryDetail structureQueryDetail = StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Full);

        private StructureReferenceDetail structureReferenceDetail =
            StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.None);

        private SdmxStructureType specificStructureReference;

        private bool isReturnLatest;

        #region Implementation of IStructureQueryMetadata

        public bool IsReturnLatest
        {
            get
            {
                return isReturnLatest;
            }
        }

        public StructureQueryDetail StructureQueryDetail
        {
            get
            {
                return structureQueryDetail;
            }
        }

        public StructureReferenceDetail StructureReferenceDetail
        {
            get
            {
                return structureReferenceDetail;
            }
        }

        public SdmxStructureType SpecificStructureReference
        {
            get
            {
                return specificStructureReference;
            }
        }

        #endregion

        public StructureQueryMetadataCore(
            StructureQueryDetail structureQueryDetail,
            StructureReferenceDetail structureReferenceDetail,
            SdmxStructureType specificStructureReference,
            bool isReturnLatest)
        {
            if (structureReferenceDetail == StructureReferenceDetail.GetFromEnum(StructureReferenceDetailEnumType.Specific)
                && specificStructureReference == null)
            {
                throw new SdmxSemmanticException("SpecificStructureReference is null and specific reference detail was requested");
            }

            if (specificStructureReference != null && !specificStructureReference.IsMaintainable)
            {
               throw new SdmxSemmanticException("SpecificStructureReference is not maintainable");
            }

            if (structureQueryDetail != StructureQueryDetail.GetFromEnum(StructureQueryDetailEnumType.Null))
            {
                this.structureQueryDetail = structureQueryDetail;
            }

            if (structureReferenceDetail != null)
            {
                this.structureReferenceDetail = structureReferenceDetail;
            }

            if (specificStructureReference != null)
            {
                this.specificStructureReference = specificStructureReference;
            }

            this.isReturnLatest = isReturnLatest;
        }

        public StructureQueryMetadataCore(string[] querystring, IDictionary<string, string> queryParameters)
        {
            ParserQuerystring(querystring);
            ParserQueryParameters(queryParameters);
        }

        private void ParserQuerystring(string[] querystring)
        {
            if (querystring.Length >= 4)
            {
                if ("latest".Equals(querystring[3], StringComparison.OrdinalIgnoreCase))
                {
                    this.isReturnLatest = true;
                }
            }
            else
            {
                this.isReturnLatest = true;
            }
        }

        private void ParserQueryParameters(IDictionary<string, string> queryParameters)
        {
            if (queryParameters != null)
            {
                foreach (string key in queryParameters.Keys)
                {
                    string value = queryParameters[key];
                    if (key.Equals("detail", StringComparison.OrdinalIgnoreCase))
                    {
                        var structureQueryDetailEnum = StructureQueryDetailEnumType.Full;
                        if (!Enum.TryParse(value, true, out structureQueryDetailEnum))
                        {
                            structureQueryDetail = StructureQueryDetail.GetFromEnum(structureQueryDetailEnum);
                            throw new SdmxSemmanticException("unable to parse value for key " + key);
                        }
                      
                        structureQueryDetail = StructureQueryDetail.GetFromEnum(structureQueryDetailEnum);
                    }
                    else if (key.Equals("references", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            structureReferenceDetail = StructureReferenceDetail.ParseString(value);

                            if (structureReferenceDetail.EnumType == StructureReferenceDetailEnumType.Specific)
                            {
                                specificStructureReference = SdmxStructureType.ParseClass(value);
                            }
                        }
                        catch (SdmxSemmanticException e)
                        {
                            throw new SdmxSemmanticException("unable to parse value for key " + key, e);
                        }
                    }
                    else
                    {
                        throw new SdmxSemmanticException("Unknown query parameter : " + key);
                    }
                }
            }
        }
    }
}
