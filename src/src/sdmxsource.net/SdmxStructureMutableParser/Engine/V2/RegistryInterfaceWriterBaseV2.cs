// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryInterfaceWriterBaseV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registry interface writer base v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;

    /// <summary>
    ///     The registry interface writer base v 2.
    /// </summary>
    public abstract class RegistryInterfaceWriterBaseV2 : StructureWriterBaseV2
    {
        #region Fields

        /// <summary>
        ///     Contains the common prefix that will be used by this message.
        /// </summary>
        private readonly string _commonPrefix;

        /// <summary>
        ///     The default ns.
        /// </summary>
        private readonly NamespacePrefixPair _defaultNs;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryInterfaceWriterBaseV2"/> class.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// writer is null
        /// </exception>
        /// <param name="writer">
        /// The XmlTextWriter object use to actually perform the writing
        /// </param>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        protected RegistryInterfaceWriterBaseV2(XmlWriter writer, SdmxNamespaces namespaces)
            : base(writer, namespaces)
        {
            this._commonPrefix = this.Namespaces.Common.Prefix;
            this._defaultNs = this.Namespaces.Registry;
        }

        #endregion
        /// <summary>
        ///     Gets the default ns.
        /// </summary>
        protected override NamespacePrefixPair DefaultNS
        {
            get
            {
                return this._defaultNs;
            }
        }

        #region Methods

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="codelistRef">
        /// The reference to write
        /// </param>
        protected void WriteCodeListRef(IReferenceInfo codelistRef)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.CodelistRef);
            this.WriteCommonRef(codelistRef, ElementNameTable.CodelistID);
        }

        /// <summary>
        /// Write Constraint
        /// </summary>
        /// <param name="constrain">
        /// The IConstraintMutableObject to write
        /// </param>
        protected void WriteConstrain(IContentConstraintMutableObject constrain)
        {
            if (constrain != null)
            {
                this.WriteStartElement(this.DefaultNS, ElementNameTable.Constraint);
                this.TryWriteAttribute(AttributeNameTable.ConstraintType, "Content");
                this.TryToWriteElement(this._commonPrefix, ElementNameTable.ConstraintID, constrain.Id);
                this.WriteCubeRegion(constrain.IncludedCubeRegion, true);
                this.WriteCubeRegion(constrain.ExcludedCubeRegion, false);

                //////this.WriteMetadataConceptSet(constrain.);
                this.WriteKeySet(constrain.IncludedSeriesKeys, true);
                this.WriteKeySet(constrain.ExcludedSeriesKeys, false);

                this.WriteReleaseCalendar(constrain.ReleaseCalendar);
                this.WriteReferencePeriod(constrain.ReferencePeriod);
                this.WriteEndElement();
            }
        }

        /// <summary>
        /// Write Data source
        /// </summary>
        /// <param name="dataSource">
        /// The IDataSourceMutableObject to write
        /// </param>
        protected void WriteDataSource(IDataSourceMutableObject dataSource)
        {
            if (dataSource != null)
            {
                this.WriteStartElement(this.DefaultNS, ElementNameTable.Datasource);
                this.TryToWriteElement(this.DefaultNS, ElementNameTable.SimpleDatasource, dataSource.SimpleDatasource);
                {
                    this.WriteStartElement(this.DefaultNS, ElementNameTable.QueryableDatasource);
                    this.WriteAttribute(AttributeNameTable.isRESTDatasource, dataSource.RESTDatasource);
                    this.WriteAttribute(AttributeNameTable.isWebServiceDatasource, dataSource.WebServiceDatasource);
                    this.TryToWriteElement(this.DefaultNS, ElementNameTable.DataUrl, dataSource.DataUrl);
                    this.TryToWriteElement(this.DefaultNS, ElementNameTable.WSDLUrl, dataSource.WSDLUrl);
                    this.WriteEndElement();
                }

                this.WriteEndElement();
            }
        }

        /// <summary>
        /// Write a Ref type element
        /// </summary>
        /// <param name="refBean">
        /// The <see cref="IReferenceInfo"/> based object
        /// </param>
        protected void WriteRef(IReferenceInfo refBean)
        {
            switch (refBean.SdmxStructure)
            {
                case SdmxStructureEnumType.Dataflow:
                    var dataflowReferenceInfo = refBean as IDataflowReferenceInfo;
                    if (dataflowReferenceInfo != null)
                    {
                        this.WriteDataflowRef(dataflowReferenceInfo);
                    }
                    else
                    {
                        this.WriteDataflowRefBasic(refBean);
                    }

                    break;
                case SdmxStructureEnumType.CategoryScheme:
                    this.WriteCategorySchemeRef(refBean);
                    break;
                case SdmxStructureEnumType.CodeList:
                    this.WriteCodeListRef(refBean);
                    break;
                case SdmxStructureEnumType.ConceptScheme:
                    this.WriteConceptSchemeRef(refBean);
                    break;
                case SdmxStructureEnumType.OrganisationScheme:
                    this.WriteOrganisationSchemeRef(refBean);
                    break;
                case SdmxStructureEnumType.Dsd:
                    this.WriteDsdRef(refBean);
                    break;
                case SdmxStructureEnumType.Msd:
                    this.WriteMetadataStructureRef(refBean);
                    break;
                case SdmxStructureEnumType.MetadataFlow:
                    this.WriteMetadataflowRef(refBean);
                    break;
                case SdmxStructureEnumType.HierarchicalCodelist:
                    this.WriteHierarchicalCodeListRef(refBean);
                    break;

                default:
                    return;
            }

            this.WriteEndElement();
        }

        /// <summary>
        /// Write StatusMessage
        /// </summary>
        /// <param name="statusMessage">
        /// The StatusMessageBean to write
        /// </param>
        protected void WriteStatusMessage(IStatusMessageInfo statusMessage)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.StatusMessage);
            this.TryWriteAttribute(AttributeNameTable.status, statusMessage.Status.ToString());
            foreach (ITextTypeWrapperMutableObject text in statusMessage.MessageTexts)
            {
                this.WriteTextType(this.DefaultNS, text, ElementNameTable.MessageText);
            }

            this.WriteEndElement(); // </StatusMessage>
        }

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="categorySchemeRefBean">
        /// The reference to write
        /// </param>
        private void WriteCategorySchemeRef(IReferenceInfo categorySchemeRefBean)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.CategorySchemeRef);
            this.WriteCommonRef(categorySchemeRefBean, ElementNameTable.CategorySchemeID);
        }

        /// <summary>
        /// Write common IReferenceInfo elements
        /// </summary>
        /// <param name="refBean">
        /// The IReferenceInfo to write
        /// </param>
        /// <param name="id">
        /// The name of the element
        /// </param>
        private void WriteCommonRef(IReferenceInfo refBean, ElementNameTable id)
        {
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.URN, refBean.URN);
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.AgencyID, refBean.AgencyId);
            this.TryToWriteElement(this.DefaultNS, id, refBean.ID);
            this.TryToWriteElement(this.DefaultNS, ElementNameTable.Version, refBean.Version);
        }

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="conceptScheme">
        /// The reference to write
        /// </param>
        private void WriteConceptSchemeRef(IReferenceInfo conceptScheme)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.ConceptSchemeRef);
            this.WriteCommonRef(conceptScheme, ElementNameTable.ConceptSchemeID);
        }

        /// <summary>
        /// Write CubeRegion
        /// </summary>
        /// <param name="cubeRegion">
        /// The ICubeRegionMutableObject to write
        /// </param>
        /// <param name="isIncluded">
        /// Specifies whether the <paramref name="cubeRegion"/> is included
        /// </param>
        private void WriteCubeRegion(ICubeRegionMutableObject cubeRegion, bool isIncluded)
        {
            if (cubeRegion != null)
            {
                this.WriteStartElement(this._commonPrefix, ElementNameTable.CubeRegion);
                this.WriteAttribute(AttributeNameTable.isIncluded, isIncluded);
                foreach (IKeyValuesMutable member in cubeRegion.KeyValues)
                {
                    this.WriteMember(member, isIncluded);
                }

                this.WriteEndElement();
            }
        }

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="dataflow">
        /// The reference to write
        /// </param>
        private void WriteDataflowRef(IDataflowReferenceInfo dataflow)
        {
            this.WriteDataflowRefBasic(dataflow);

            ////this.WriteDataSource(dataflow.Datasource);
            this.WriteConstrain(dataflow.Constraint);
        }

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="dataflow">
        /// The reference to write
        /// </param>
        private void WriteDataflowRefBasic(IReferenceInfo dataflow)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.DataflowRef);
            this.WriteCommonRef(dataflow, ElementNameTable.DataflowID);
        }

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="bean">
        /// The reference to write
        /// </param>
        private void WriteDsdRef(IReferenceInfo bean)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.KeyFamilyRef);
            this.WriteCommonRef(bean, ElementNameTable.KeyFamilyID);
        }

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="bean">
        /// The reference to write
        /// </param>
        private void WriteHierarchicalCodeListRef(IReferenceInfo bean)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.HierarchicalCodelistRef);
            this.WriteCommonRef(bean, ElementNameTable.HierarchicalCodelistID);
        }

        /// <summary>
        /// Write Key
        /// </summary>
        /// <param name="key">
        /// Write the KeyBean
        /// </param>
        private void WriteKey(IEnumerable<IKeyValue> key)
        {
            if (key != null)
            {
                this.WriteStartElement(this.DefaultNS, ElementNameTable.Key);
                foreach (IKeyValue keyValue in key)
                {
                    this.TryToWriteElement(this.DefaultNS, ElementNameTable.ComponentRef, keyValue.Concept);
                    this.TryToWriteElement(this.DefaultNS, ElementNameTable.Value, keyValue.Code);
                }

                this.WriteEndElement();
            }
        }

        /// <summary>
        /// Write KeySet
        /// </summary>
        /// <param name="set">
        /// The KeySetBean to write
        /// </param>
        /// <param name="isIncluded">
        /// A value indication whether the <paramref name="set"/> is included
        /// </param>
        private void WriteKeySet(IConstraintDataKeySetMutableObject set, bool isIncluded)
        {
            if (set != null)
            {
                foreach (IConstrainedDataKeyMutableObject keyValue in set.ConstrainedDataKeys)
                {
                    this.WriteStartElement(this.DefaultNS, ElementNameTable.KeySet);
                    this.WriteAttribute(AttributeNameTable.isIncluded, isIncluded);
                    this.WriteKey(keyValue.KeyValues);
                    this.WriteEndElement();
                }
            }
        }

        /// <summary>
        /// Write Member
        /// </summary>
        /// <param name="member">
        /// The MemberBean to write
        /// </param>
        /// <param name="isIncluded">
        /// A value indication whether the <paramref name="member"/> is included
        /// </param>
        private void WriteMember(IKeyValuesMutable member, bool isIncluded)
        {
            if (member != null)
            {
                this.WriteStartElement(this._commonPrefix, ElementNameTable.Member);
                this.WriteAttribute(AttributeNameTable.isIncluded, isIncluded);
                this.TryToWriteElement(this._commonPrefix, ElementNameTable.ComponentRef, member.Id);
                foreach (string memberValue in member.KeyValues)
                {
                    this.WriteMemberValue(memberValue);
                }

                this.WriteEndElement();
            }
        }

        /// <summary>
        /// Write MemberValue
        /// </summary>
        /// <param name="memberValue">
        /// The MemberValueBean to write
        /// </param>
        private void WriteMemberValue(string memberValue)
        {
            if (memberValue != null)
            {
                this.WriteStartElement(this._commonPrefix, ElementNameTable.MemberValue);
                this.TryToWriteElement(this._commonPrefix, ElementNameTable.Value, memberValue);
                this.WriteEndElement();
            }
        }

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="bean">
        /// The reference to write
        /// </param>
        private void WriteMetadataStructureRef(IReferenceInfo bean)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.MetadataStructureRef);
            this.WriteCommonRef(bean, ElementNameTable.MetadataStructureID);
        }

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="dataflow">
        /// The reference to write
        /// </param>
        private void WriteMetadataflowRef(IReferenceInfo dataflow)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.MetadataflowRef);
            this.WriteCommonRef(dataflow, ElementNameTable.MetadataflowID);

            ////this.WriteDataSource(dataflow.Datasource);
            ////this.WriteConstrain(dataflow.Constraint);
        }

        /// <summary>
        /// Write the specified reference
        /// </summary>
        /// <param name="bean">
        /// The reference to write
        /// </param>
        private void WriteOrganisationSchemeRef(IReferenceInfo bean)
        {
            this.WriteStartElement(this.DefaultNS, ElementNameTable.OrganisationSchemeRef);
            this.WriteCommonRef(bean, ElementNameTable.OrganisationSchemeID);
        }

        /// <summary>
        /// Write ReferencePeriod
        /// </summary>
        /// <param name="period">
        /// The IReferencePeriodMutableObject to write
        /// </param>
        private void WriteReferencePeriod(IReferencePeriodMutableObject period)
        {
            if (period != null)
            {
                this.WriteStartElement(this._commonPrefix, ElementNameTable.ReferencePeriod);
                this.TryWriteAttribute(AttributeNameTable.startTime, period.StartTime);
                this.TryWriteAttribute(AttributeNameTable.endTime, period.EndTime);
                this.WriteEndElement();
            }
        }

        /// <summary>
        /// Write ReleaseCalendar
        /// </summary>
        /// <param name="releaseCalendar">
        /// The IReleaseCalendarMutableObject to write
        /// </param>
        private void WriteReleaseCalendar(IReleaseCalendarMutableObject releaseCalendar)
        {
            if (releaseCalendar != null)
            {
                this.WriteStartElement(this._commonPrefix, ElementNameTable.ReleaseCalendar);
                this.TryToWriteElement(this._commonPrefix, ElementNameTable.Periodicity, releaseCalendar.Periodicity);
                this.TryToWriteElement(this._commonPrefix, ElementNameTable.Offset, releaseCalendar.Offset);
                this.TryToWriteElement(this._commonPrefix, ElementNameTable.Tolerance, releaseCalendar.Tolerance);
                this.WriteEndElement();
            }
        }

        #endregion
    }
}