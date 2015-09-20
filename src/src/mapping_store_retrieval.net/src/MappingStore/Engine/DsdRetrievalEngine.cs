// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DsdRetrievalEngine.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The DSD retrieval engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.MappingStoreRetrieval.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Globalization;
    using System.Linq;
    using System.Xml;

    using Estat.Sri.MappingStoreRetrieval.Builder;
    using Estat.Sri.MappingStoreRetrieval.Constants;
    using Estat.Sri.MappingStoreRetrieval.Extensions;
    using Estat.Sri.MappingStoreRetrieval.Helper;
    using Estat.Sri.MappingStoreRetrieval.Manager;
    using Estat.Sri.MappingStoreRetrieval.Model;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Util;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Util.Objects;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util.Collections;
    using Org.Sdmxsource.Util.Extensions;

    using TextType = Org.Sdmxsource.Sdmx.Api.Constants.TextType;
    using VersionQueryType = Estat.Sri.MappingStoreRetrieval.Constants.VersionQueryType;

    /// <summary>
    ///     The DSD retrieval engine.
    /// </summary>
    internal class DsdRetrievalEngine : ArtefactRetrieverEngine<IDataStructureMutableObject>
    {
        #region Static Fields

        /// <summary>
        ///     The _cross DSD builder.
        /// </summary>
        private static readonly CrossDsdBuilder _crossDsdBuilder = new CrossDsdBuilder();

        #endregion

        #region Fields

        /// <summary>
        ///     The _attribute group query info.
        /// </summary>
        private readonly SqlQueryInfo _attributeGroupQueryInfo;

        /// <summary>
        ///     The _command builder.
        /// </summary>
        private readonly ItemCommandBuilder _commandBuilder;

        /// <summary>
        ///     The _component query info.
        /// </summary>
        private readonly SqlQueryInfo _componentQueryInfo;

        /// <summary>
        ///     The _group query info.
        /// </summary>
        private readonly SqlQueryInfo _groupQueryInfo;

        /// <summary>
        /// The attribute dimension refs information
        /// </summary>
        private readonly SqlQueryInfo _attributeDimensionRefsInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DsdRetrievalEngine"/> class.
        ///     Initializes a new instance of the <see cref="DataflowRetrievalEngine"/> class.
        /// </summary>
        /// <param name="mappingStoreDb">
        /// The mapping store DB.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="mappingStoreDb"/> is null.
        /// </exception>
        public DsdRetrievalEngine(Database mappingStoreDb)
            : base(mappingStoreDb, DsdConstant.TableInfo)
        {
            var sqlQueryBuilder = new ReferencedSqlQueryBuilder(this.MappingStoreDb, null);
            this._attributeGroupQueryInfo = sqlQueryBuilder.Build(DsdConstant.AttributeAttachmentGroupQueryFormat);
            this._groupQueryInfo = sqlQueryBuilder.Build(DsdConstant.GroupQueryFormat);
            this._attributeDimensionRefsInfo = sqlQueryBuilder.Build(DsdConstant.AttributeDimensionFormat);
            this._componentQueryInfo = sqlQueryBuilder.Build(DsdConstant.ComponentQueryFormat);
            this._componentQueryInfo.OrderBy = DsdConstant.ComponentOrderBy;
            this._commandBuilder = new ItemCommandBuilder(this.MappingStoreDb);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Retrieve the <see cref="IDataStructureMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="versionConstraints">
        /// The version constraints.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataStructureMutableObject}"/>.
        /// </returns>
        public override ISet<IDataStructureMutableObject> Retrieve(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, VersionQueryType versionConstraints)
        {
            var sqlInfo = versionConstraints == VersionQueryType.Latest ? this.SqlQueryInfoForLatest : this.SqlQueryInfoForAll;
            return this.GetDataStructureMutableObjects(maintainableRef, detail, sqlInfo);
        }

        /// <summary>
        /// Retrieve the <see cref="IDataStructureMutableObject"/> with the latest version group by ID and AGENCY from Mapping
        ///     Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <returns>
        /// The <see cref="IDataStructureMutableObject"/>.
        /// </returns>
        public override IDataStructureMutableObject RetrieveLatest(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail)
        {
            ISet<IDataStructureMutableObject> mutableObjects = this.GetDataStructureMutableObjects(maintainableRef, detail, this.SqlQueryInfoForLatest);
            return mutableObjects.GetOneOrNothing();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a new instance of <see cref="IDataStructureMutableObject" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="IDataStructureMutableObject" />.
        /// </returns>
        protected override IDataStructureMutableObject CreateArtefact()
        {
            return new DataStructureMutableCore();
        }

        /// <summary>
        /// Returns the referenced from inner joins to use with <see cref="ArtefactParentsSqlBuilder"/>
        /// </summary>
        /// <param name="structureEnumType">
        /// The structure  type.
        /// </param>
        /// <returns>
        /// The referenced from inner joins
        /// </returns>
        protected override string GetReferencedFromInnerJoins(SdmxStructureEnumType structureEnumType)
        {
            string innerJoin = null;
            switch (structureEnumType)
            {
                case SdmxStructureEnumType.CodeList:
                    innerJoin = CodeListConstant.ReferencingFromDsd;
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    innerJoin = ConceptSchemeConstant.ReferencingFromDsd;
                    break;
            }

            return innerJoin;
        }

        /// <summary>
        /// Retrieve details for the specified <paramref name="artefact"/> with MAPPING STORE ARTEFACT.ART_ID equal to
        ///     <paramref name="sysId"/>
        /// </summary>
        /// <param name="artefact">
        /// The artefact.
        /// </param>
        /// <param name="sysId">
        /// The MAPPING STORE ARTEFACT.ART_ID value
        /// </param>
        /// <returns>
        /// The <see cref="IDataStructureMutableObject"/>.
        /// </returns>
        protected override IDataStructureMutableObject RetrieveDetails(IDataStructureMutableObject artefact, long sysId)
        {
            this.PopulateGroups(sysId, artefact);
            return this.FillComponents(artefact, sysId);
        }

        /// <summary>
        /// Convert the <paramref name="facetValue"/> to decimal.
        /// </summary>
        /// <param name="facetValue">
        /// The facet value.
        /// </param>
        /// <returns>
        /// the <paramref name="facetValue"/> as decimal; otherwise null.
        /// </returns>
        private static decimal? FacetToDecimal(string facetValue)
        {
            if (string.IsNullOrWhiteSpace(facetValue))
            {
                return null;
            }

            decimal value;
            if (decimal.TryParse(facetValue, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// Convert the <paramref name="facetValue"/> to integer.
        /// </summary>
        /// <param name="facetValue">
        /// The facet value.
        /// </param>
        /// <returns>
        /// the <paramref name="facetValue"/> as integer; otherwise null.
        /// </returns>
        private static long? FacetToInteger(string facetValue)
        {
            if (string.IsNullOrWhiteSpace(facetValue))
            {
                return null;
            }

            long value;
            if (long.TryParse(facetValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// Convert the <paramref name="facetValue"/> to <see cref="TertiaryBool"/>
        /// </summary>
        /// <param name="facetValue">
        /// The facet value.
        /// </param>
        /// <returns>
        /// the <paramref name="facetValue"/> as <see cref="TertiaryBool"/>
        /// </returns>
        private static TertiaryBool FacetToTristateBool(string facetValue)
        {
            if (string.IsNullOrWhiteSpace(facetValue))
            {
                return TertiaryBool.GetFromEnum(TertiaryBoolEnumType.Unset);
            }

            return TertiaryBool.ParseBoolean(XmlConvert.ToBoolean(facetValue.ToLowerInvariant()));
        }

        /// <summary>
        /// The populate attributes.
        /// </summary>
        /// <param name="groupAttributes">
        /// The group attributes.
        /// </param>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="dataReader">
        /// The data reader.
        /// </param>
        /// <returns>
        /// The <see cref="IComponentMutableObject"/>.
        /// </returns>
        /// <exception cref="SdmxNotImplementedException">
        /// Unsupported attachment level at COMPONENT.ATT_ASS_LEVEL
        /// </exception>
        private static IComponentMutableObject PopulateAttributes(IDictionary<long, IAttributeMutableObject> groupAttributes, IDataStructureMutableObject parent, IDataRecord dataReader)
        {
            var attribute = new AttributeMutableCore { AssignmentStatus = DataReaderHelper.GetString(dataReader, "ATT_STATUS") };

            // TODO pending support already in Java
            ////attribute.TimeFormat = DataReaderHelper.GetBoolean(dataReader, "ATT_IS_TIME_FORMAT")
            ////                           ? "true"
            ////                           : "false";
            string attachmentLevel = DataReaderHelper.GetString(dataReader, "ATT_ASS_LEVEL");

            switch (attachmentLevel)
            {
                case AttachmentLevelConstants.DataSet:
                    attribute.AttachmentLevel = AttributeAttachmentLevel.DataSet;
                    break;
                case AttachmentLevelConstants.Group:
                    attribute.AttachmentLevel = AttributeAttachmentLevel.Group;
                    long sysId = DataReaderHelper.GetInt64(dataReader, "COMP_ID");
                    groupAttributes.Add(sysId, attribute);
                    break;
                case AttachmentLevelConstants.Observation:
                    attribute.AttachmentLevel = AttributeAttachmentLevel.Observation;
                    break;
                case AttachmentLevelConstants.Series:
                    attribute.AttachmentLevel = AttributeAttachmentLevel.DimensionGroup;

                    break;
                default:
                    throw new SdmxNotImplementedException(ExceptionCode.Unsupported, "Attachment:" + attachmentLevel);
            }

            IComponentMutableObject component = attribute;
            parent.AddAttribute(attribute);
            return component;
        }

        /// <summary>
        /// Populates the text format.
        /// </summary>
        /// <param name="enumName">
        /// Name of the enumeration.
        /// </param>
        /// <param name="enumValue">
        /// The enumeration value.
        /// </param>
        /// <param name="textFormat">
        /// The text format.
        /// </param>
        /// <param name="facetValue">
        /// The facet value.
        /// </param>
        private static void PopulateTextFormat(string enumName, string enumValue, ITextFormatMutableObject textFormat, string facetValue)
        {
            switch (enumName)
            {
                case "DataType":
                    {
                        TextEnumType textType;
                        if (Enum.TryParse(enumValue, true, out textType))
                        {
                            textFormat.TextType = TextType.GetFromEnum(textType);
                        }
                    }

                    break;
                case "FacetType":
                    {
                        switch (enumValue)
                        {
                            case "isSequence":
                                textFormat.Sequence = FacetToTristateBool(facetValue);
                                break;
                            case "minLength":
                                textFormat.MinLength = FacetToInteger(facetValue);
                                break;
                            case "maxLength":
                                textFormat.MaxLength = FacetToInteger(facetValue);
                                break;
                            case "minValue":
                                textFormat.MinValue = FacetToDecimal(facetValue);
                                break;
                            case "maxValue":
                                textFormat.MaxValue = FacetToDecimal(facetValue);
                                break;
                            case "startValue":
                                textFormat.StartValue = FacetToDecimal(facetValue);
                                break;
                            case "endValue":
                                textFormat.EndValue = FacetToDecimal(facetValue);
                                break;
                            case "decimals":
                                textFormat.Decimals = FacetToInteger(facetValue);
                                break;
                            case "interval":
                                textFormat.Interval = FacetToDecimal(facetValue);
                                break;
                            case "timeInterval":
                                textFormat.TimeInterval = facetValue;
                                break;
                            case "pattern":
                                textFormat.Pattern = facetValue;
                                break;
                            case "isMultiLingual":
                                textFormat.Multilingual = FacetToTristateBool(facetValue);
                                break;
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// Retrieve component type.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="dataReader">
        /// The data reader.
        /// </param>
        /// <param name="groupAttributes">
        /// The group attributes.
        /// </param>
        /// <param name="crossSectionalMeasures">
        /// The cross sectional measures.
        /// </param>
        /// <returns>
        /// The <see cref="IComponentMutableObject"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Invalid DSD Component type.
        /// </exception>
        private static IComponentMutableObject RetrieveComponentType(
            IDataStructureMutableObject parent, 
            IDataRecord dataReader, 
            IDictionary<long, IAttributeMutableObject> groupAttributes, 
            ICollection<ICrossSectionalMeasureMutableObject> crossSectionalMeasures)
        {
            string type = DataReaderHelper.GetString(dataReader, "TYPE");
            SdmxComponentType componentType;
            if (!Enum.TryParse(type, out componentType))
            {
                componentType = SdmxComponentType.None;
            }

            IComponentMutableObject component;
            switch (componentType)
            {
                case SdmxComponentType.Dimension:
                    var dimension = new DimensionMutableCore
                                        {
                                            FrequencyDimension = DataReaderHelper.GetBoolean(dataReader, "IS_FREQ_DIM"), 
                                            MeasureDimension = DataReaderHelper.GetBoolean(dataReader, "IS_MEASURE_DIM")
                                        };
                    component = dimension;
                    parent.AddDimension(dimension);
                    break;
                case SdmxComponentType.Attribute:
                    component = PopulateAttributes(groupAttributes, parent, dataReader);

                    break;
                case SdmxComponentType.TimeDimension:
                    var timeDimension = new DimensionMutableCore { TimeDimension = true };
                    component = timeDimension;
                    parent.AddDimension(timeDimension);
                    break;
                case SdmxComponentType.PrimaryMeasure:
                    var primaryMeasure = new PrimaryMeasureMutableCore();
                    component = primaryMeasure;
                    parent.PrimaryMeasure = primaryMeasure;
                    break;
                case SdmxComponentType.CrossSectionalMeasure:
                    {
                        var crossSectionalMeasureBean = new CrossSectionalMeasureMutableCore { Code = DataReaderHelper.GetString(dataReader, "XS_MEASURE_CODE") };

                        component = crossSectionalMeasureBean;
                        crossSectionalMeasures.Add(crossSectionalMeasureBean);
                    }

                    break;
                default:
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Invalid DSD Component type.  Mapping Store Database,  COMPONENT.TYPE = '{0}'", type));
            }

            return component;
        }

        /// <summary>
        /// Retrieve the concept reference.
        /// </summary>
        /// <param name="dataReader">
        /// The data reader.
        /// </param>
        /// <param name="component">
        /// The component.
        /// </param>
        private static void RetrieveConceptRef(IDataRecord dataReader, IComponentMutableObject component)
        {
            //// TODO cache category references
            string conceptRef = DataReaderHelper.GetString(dataReader, "CONCEPTREF");
            string conceptSchemeRef = DataReaderHelper.GetString(dataReader, "CONCEPTSCHEME_ID");
            string conceptSchemeVersion = DataReaderHelper.GetString(dataReader, "CONCEPT_VERSION");
            string conceptSchemeAgency = DataReaderHelper.GetString(dataReader, "CONCEPT_AGENCY");
            component.ConceptRef = new StructureReferenceImpl(conceptSchemeAgency, conceptSchemeRef, conceptSchemeVersion, SdmxStructureEnumType.Concept, conceptRef);
        }

        /// <summary>
        /// Retrieve cross sectional attachments.
        /// </summary>
        /// <param name="dataReader">
        /// The data reader.
        /// </param>
        /// <param name="crossDataSet">
        /// The list of components attached to cross-sectional data set.
        /// </param>
        /// <param name="conceptRef">
        /// The concept id.
        /// </param>
        /// <param name="crossGroup">
        /// The list of components attached to cross-sectional group.
        /// </param>
        /// <param name="crossSection">
        /// The list of components attached to cross-sectional section.
        /// </param>
        /// <param name="crossObs">
        /// The list of components attached to cross-sectional observation level
        /// </param>
        private static void RetrieveCrossSectionalAttachments(
            IDataRecord dataReader, 
            ISet<string> crossDataSet, 
            string conceptRef, 
            ISet<string> crossGroup, 
            ISet<string> crossSection, 
            ISet<string> crossObs)
        {
            TertiaryBool crossSectionalAttachDataSet = DataReaderHelper.GetTristate(dataReader, "XS_ATTLEVEL_DS");
            if (crossSectionalAttachDataSet.IsTrue)
            {
                crossDataSet.Add(conceptRef);
            }

            TertiaryBool crossSectionalAttachGroup = DataReaderHelper.GetTristate(dataReader, "XS_ATTLEVEL_GROUP");
            if (crossSectionalAttachGroup.IsTrue)
            {
                crossGroup.Add(conceptRef);
            }

            TertiaryBool crossSectionalAttachSection = DataReaderHelper.GetTristate(dataReader, "XS_ATTLEVEL_SECTION");
            if (crossSectionalAttachSection.IsTrue)
            {
                crossSection.Add(conceptRef);
            }

            TertiaryBool crossSectionalAttachObservation = DataReaderHelper.GetTristate(dataReader, "XS_ATTLEVEL_OBS");
            if (crossSectionalAttachObservation.IsTrue)
            {
                crossObs.Add(conceptRef);
            }
        }

        /// <summary>
        /// Retrieve the component representation reference.
        /// </summary>
        /// <param name="dataReader">
        /// The data reader.
        /// </param>
        /// <param name="component">
        /// The component.
        /// </param>
        /// <returns>
        /// The <see cref="IStructureReference"/> of the representation.
        /// </returns>
        private static IStructureReference RetrieveRepresentationReference(IDataRecord dataReader, IComponentMutableObject component)
        {
            IStructureReference measureCodelistRepresentation = null;
            string codelist = DataReaderHelper.GetString(dataReader, "CODELIST_ID");
            if (!string.IsNullOrWhiteSpace(codelist))
            {
                string codelistAgency = DataReaderHelper.GetString(dataReader, "CODELIST_AGENCY");
                string codelistVersion = DataReaderHelper.GetString(dataReader, "CODELIST_VERSION");
                var codelistRepresentation = new StructureReferenceImpl(codelistAgency, codelist, codelistVersion, SdmxStructureEnumType.CodeList);
                component.Representation = new RepresentationMutableCore { Representation = codelistRepresentation };
            }

            // Important. Concept scheme must be checked *after* codelist.
            var conceptSchemeRepresentation = DataReaderHelper.GetString(dataReader, "REP_CS_ID");
            if (!string.IsNullOrWhiteSpace(conceptSchemeRepresentation))
            {
                var measureDimension = component as IDimensionMutableObject;
                if (measureDimension != null && measureDimension.MeasureDimension)
                {
                    string agency = DataReaderHelper.GetString(dataReader, "REP_CS_AGENCY");
                    string version = DataReaderHelper.GetString(dataReader, "REP_CS_VERSION");
                    if (component.Representation != null)
                    {
                        measureCodelistRepresentation = component.Representation.Representation;
                    }

                    measureDimension.Representation = new RepresentationMutableCore
                                                          {
                                                              Representation =
                                                                  new StructureReferenceImpl(
                                                                  agency, 
                                                                  conceptSchemeRepresentation, 
                                                                  version, 
                                                                  SdmxStructureEnumType.ConceptScheme)
                                                          };
                }
            }

            return measureCodelistRepresentation;
        }

        /// <summary>
        /// Setup attribute attachment level.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="dimensionRefs">The dimension references.</param>
        private static void SetupAttributeAttachmentLevel(IDataStructureMutableObject parent, IDictionary<string, ISet<string>> dimensionRefs)
        {
            foreach (IAttributeMutableObject attribute in parent.AttributeList.Attributes)
            {
                switch (attribute.AttachmentLevel)
                {
                    case AttributeAttachmentLevel.Observation:
                        attribute.PrimaryMeasureReference = parent.PrimaryMeasure.Id;
                        break;
                    case AttributeAttachmentLevel.DimensionGroup:
                        {
                            ISet<string> attributeDimensionSet;
                            if (!dimensionRefs.TryGetValue(attribute.Id, out attributeDimensionSet))
                            {
                                attributeDimensionSet = new HashSet<string>(parent.Dimensions.Where(o => !o.TimeDimension).Select(o => o.Id));
                            }
                            
                            attribute.DimensionReferences.AddAll(attributeDimensionSet);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Setup cross sectional DSD.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="crossDataSet">
        /// The list of components attached to cross-sectional data set.
        /// </param>
        /// <param name="crossGroup">
        /// The list of components attached to cross-sectional group.
        /// </param>
        /// <param name="crossSection">
        /// The list of components attached to cross-sectional section.
        /// </param>
        /// <param name="crossObs">
        /// The list of components attached to cross-sectional observation level
        /// </param>
        /// <param name="crossSectionalMeasures">
        /// The cross sectional measures.
        /// </param>
        /// <param name="measureCodelistRepresentation">
        /// The SDMX v2.0 measure dimension codelist representation; otherwise null
        /// </param>
        /// <returns>
        /// The <see cref="IDataStructureMutableObject"/>.
        /// </returns>
        private static IDataStructureMutableObject SetupCrossSectionalDsd(
            IDataStructureMutableObject parent, 
            ICollection<string> crossDataSet, 
            ICollection<string> crossGroup, 
            ICollection<string> crossSection, 
            ICollection<string> crossObs, 
            IEnumerable<ICrossSectionalMeasureMutableObject> crossSectionalMeasures, 
            IStructureReference measureCodelistRepresentation)
        {
            if (
                parent.Dimensions.All(
                    o =>
                    o.FrequencyDimension || o.TimeDimension || o.MeasureDimension || crossDataSet.Contains(o.Id) || crossGroup.Contains(o.Id) || crossSection.Contains(o.Id) || crossObs.Contains(o.Id)))
            {
                if (parent.AttributeList == null
                    || parent.AttributeList.Attributes.All(o => crossDataSet.Contains(o.Id) || crossGroup.Contains(o.Id) || crossSection.Contains(o.Id) || crossObs.Contains(o.Id)))
                {
                    ICrossSectionalDataStructureMutableObject crossDsd = _crossDsdBuilder.Build(parent);
                    parent = crossDsd;
                    crossDsd.CrossSectionalAttachDataSet.AddAll(crossDataSet);
                    crossDsd.CrossSectionalAttachSection.AddAll(crossSection);
                    crossDsd.CrossSectionalAttachGroup.AddAll(crossGroup);
                    crossDsd.CrossSectionalAttachObservation.AddAll(crossObs);
                    IDimensionMutableObject measure = crossDsd.Dimensions.FirstOrDefault(o => o.MeasureDimension);
                    if (measure != null)
                    {
                        foreach (ICrossSectionalMeasureMutableObject crossSectionalMeasureMutableObject in crossSectionalMeasures)
                        {
                            crossSectionalMeasureMutableObject.MeasureDimension = measure.Id;
                            crossDsd.CrossSectionalMeasures.Add(crossSectionalMeasureMutableObject);
                        }

                        if (measureCodelistRepresentation == null)
                        {
                            DataStructureUtil.ConvertMeasureRepresentation(crossDsd);
                        }
                    }
                }
            }

            return parent;
        }

        /// <summary>
        /// Fill the given attribute with the Attachment Groups
        /// </summary>
        /// <param name="attribute">
        /// The attribute that needs to be populated
        /// </param>
        /// <param name="compId">
        /// The primary key of the attribute in Mapping Store
        /// </param>
        /// <remarks>
        /// TODO merge with Component query
        /// </remarks>
        private void FillAttachmentGroups(IAttributeMutableObject attribute, long compId)
        {
            using (DbCommand command = this._commandBuilder.Build(new ItemSqlQuery(this._attributeGroupQueryInfo, compId)))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    while (dataReader.Read())
                    {
                        attribute.AttachmentGroup = DataReaderHelper.GetString(dataReader, "ID");
                    }
                }
            }
        }

        /// <summary>
        /// Fill the components of a KeyFamilyBean object
        /// </summary>
        /// <param name="parent">
        /// The KeyFamilyBean to populate
        /// </param>
        /// <param name="parentSysId">
        /// The DSD.DSD_ID PrimaryKey
        /// </param>
        /// <returns>
        /// The <see cref="IDataStructureMutableObject"/> which will be the same as <paramref name="parent"/> unless the DSD
        ///     is cross sectional.
        /// </returns>
        private IDataStructureMutableObject FillComponents(IDataStructureMutableObject parent, long parentSysId)
        {
            var crossSectionalMeasures = new List<ICrossSectionalMeasureMutableObject>();
            var crossDataSet = new HashSet<string>();
            var crossGroup = new HashSet<string>();
            var crossSection = new HashSet<string>();
            var crossObs = new HashSet<string>();
            var groupAttributes = new Dictionary<long, IAttributeMutableObject>();
            var componentMap = new Dictionary<long, IComponentMutableObject>();
            IStructureReference measureCodelistRepresentation = null;
            using (DbCommand command = this._commandBuilder.Build(new ItemSqlQuery(this._componentQueryInfo, parentSysId)))
            using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
            {
                var idIdx = dataReader.GetOrdinal("ID");
                var compIdIdx = dataReader.GetOrdinal("COMP_ID");

                while (dataReader.Read())
                {
                    IComponentMutableObject component = RetrieveComponentType(parent, dataReader, groupAttributes, crossSectionalMeasures);
                    componentMap.Add(dataReader.GetInt64(compIdIdx), component);

                    component.Id = DataReaderHelper.GetString(dataReader, idIdx);

                    measureCodelistRepresentation = RetrieveRepresentationReference(dataReader, component);
                    RetrieveConceptRef(dataReader, component);

                    RetrieveCrossSectionalAttachments(dataReader, crossDataSet, component.Id, crossGroup, crossSection, crossObs);
                }
            }

            if (parent.AttributeList != null)
            {
                this.SetupGroupAttributes(groupAttributes);

                SetupAttributeAttachmentLevel(parent, this.GetAttributeDimensionRefs(parentSysId));
            }

            parent = SetupCrossSectionalDsd(parent, crossDataSet, crossGroup, crossSection, crossObs, crossSectionalMeasures, measureCodelistRepresentation);

            this.GetTextFormatInformation(parentSysId, componentMap);

            return parent;
        }

        /// <summary>
        /// Retrieve the <see cref="IDataStructureMutableObject"/> from Mapping Store.
        /// </summary>
        /// <param name="maintainableRef">
        /// The maintainable reference which may contain ID, AGENCY ID and/or VERSION.
        /// </param>
        /// <param name="detail">
        /// The <see cref="StructureQueryDetail"/> which controls if the output will include details or not.
        /// </param>
        /// <param name="queryInfo">
        /// The query Info.
        /// </param>
        /// <returns>
        /// The <see cref="ISet{IDataStructureMutableObject}"/>.
        /// </returns>
        private ISet<IDataStructureMutableObject> GetDataStructureMutableObjects(IMaintainableRefObject maintainableRef, ComplexStructureQueryDetailEnumType detail, SqlQueryInfo queryInfo)
        {
            var artefactSqlQuery = new ArtefactSqlQuery(queryInfo, maintainableRef);

            return this.RetrieveArtefacts(artefactSqlQuery, detail);
        }

        /// <summary>
        /// Gets the text format information.
        /// </summary>
        /// <param name="parentSysId">
        /// The parent system unique identifier.
        /// </param>
        /// <param name="componentMap">
        /// The component map.
        /// </param>
        private void GetTextFormatInformation(long parentSysId, IDictionary<long, IComponentMutableObject> componentMap)
        {
            var textFormats = new Dictionary<long, ITextFormatMutableObject>();
            using (var command = this.MappingStoreDb.GetSqlStringCommandFormat(DsdConstant.TextFormatQueryFormat, this.MappingStoreDb.CreateInParameter("dsdId", DbType.Int64, parentSysId)))
            using (var reader = this.MappingStoreDb.ExecuteReader(command))
            {
                var compIdIdx = reader.GetOrdinal("COMP_ID");
                var enumNameIdx = reader.GetOrdinal("ENUM_NAME");
                var enumValueIdx = reader.GetOrdinal("ENUM_VALUE");
                var facetValueIdx = reader.GetOrdinal("FACET_VALUE");

                while (reader.Read())
                {
                    var compId = reader.GetInt64(compIdIdx);

                    ITextFormatMutableObject textFormat;
                    if (!textFormats.TryGetValue(compId, out textFormat))
                    {
                        textFormat = new TextFormatMutableCore();
                        textFormats.Add(compId, textFormat);
                    }

                    var enumName = DataReaderHelper.GetString(reader, enumNameIdx);
                    var enumValue = DataReaderHelper.GetString(reader, enumValueIdx);
                    var facetValue = DataReaderHelper.GetString(reader, facetValueIdx);
                    PopulateTextFormat(enumName, enumValue, textFormat, facetValue);
                }
            }

            foreach (var textFormat in textFormats)
            {
                IComponentMutableObject component = componentMap[textFormat.Key];
                if (component.Representation == null)
                {
                    component.Representation = new RepresentationMutableCore();
                }

                component.Representation.TextFormat = textFormat.Value;
            }
        }

        /// <summary>
        /// Retrieve the Dimension references from mapping store 
        /// </summary>
        /// <param name="parentSysId">
        /// The DSD primary key in the Mapping Store Database
        /// </param>
        /// <returns>
        /// The <see cref="IDictionaryOfSets{String, String}"/>.
        /// </returns>
        private IDictionaryOfSets<string, string> GetAttributeDimensionRefs(long parentSysId)
        {
            IDictionaryOfSets<string, string> mappedAttributes = new DictionaryOfSets<string, string>(StringComparer.Ordinal);
            using (DbCommand command = this._commandBuilder.Build(new ItemSqlQuery(this._attributeDimensionRefsInfo, parentSysId)))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    while (dataReader.Read())
                    {
                        string currGroupId = DataReaderHelper.GetString(dataReader, "AID");

                        ISet<string> dimensionList;
                        if (!mappedAttributes.TryGetValue(currGroupId, out dimensionList))
                        {
                            dimensionList = new HashSet<string>(StringComparer.Ordinal);
                            mappedAttributes.Add(currGroupId, dimensionList);
                        }

                        dimensionList.Add(DataReaderHelper.GetString(dataReader, "DID"));
                    }
                }
            }

            return mappedAttributes;
        }

        /// <summary>
        /// Retrieve the Groups from mapping store and populate <paramref name="artefact"/>
        /// </summary>
        /// <param name="parentSysId">
        /// The DSD primary key in the Mapping Store Database
        /// </param>
        /// <param name="artefact">
        /// The <see cref="IDataStructureMutableObject"/> to add the groups
        /// </param>
        private void PopulateGroups(long parentSysId, IDataStructureMutableObject artefact)
        {
            using (DbCommand command = this._commandBuilder.Build(new ItemSqlQuery(this._groupQueryInfo, parentSysId)))
            {
                using (IDataReader dataReader = this.MappingStoreDb.ExecuteReader(command))
                {
                    var groupMap = new Dictionary<long, IGroupMutableObject>();
                    while (dataReader.Read())
                    {
                        long currGroupId = DataReaderHelper.GetInt64(dataReader, "GR_ID");
                        IGroupMutableObject group;
                        if (!groupMap.TryGetValue(currGroupId, out group))
                        {
                            group = new GroupMutableCore { Id = DataReaderHelper.GetString(dataReader, "GROUP_ID") };
                            artefact.AddGroup(group);
                            groupMap.Add(currGroupId, group);
                        }

                        group.DimensionRef.Add(DataReaderHelper.GetString(dataReader, "DIMENSION_REF"));
                    }
                }
            }
        }

        /// <summary>
        /// Setup group attributes.
        /// </summary>
        /// <param name="sysIdAttributes">
        /// The sys id attributes.
        /// </param>
        private void SetupGroupAttributes(IEnumerable<KeyValuePair<long, IAttributeMutableObject>> sysIdAttributes)
        {
            foreach (KeyValuePair<long, IAttributeMutableObject> attr in sysIdAttributes)
            {
                this.FillAttachmentGroups(attr.Value, attr.Key);

                // TODO add support for Attachment Measure when Mapping Store supports it
            }
        }

        #endregion
    }
}