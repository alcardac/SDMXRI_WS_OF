// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryInterfaceReaderBaseV2.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The registry interface reader base v 2.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.SdmxStructureMutableParser.Engine.V2
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    using Estat.Sri.SdmxParseBase.Helper;
    using Estat.Sri.SdmxParseBase.Model;
    using Estat.Sri.SdmxStructureMutableParser.Model;
    using Estat.Sri.SdmxXmlConstants;

    using Org.Sdmxsource.Sdmx.Api.Model.Mutable;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Util.Exception;

    /// <summary>
    ///     The registry interface reader base v 2.
    /// </summary>
    public abstract class RegistryInterfaceReaderBaseV2 : StructureReaderBaseV20, IRegistryInterfaceReader
    {
        #region Fields

        /// <summary>
        ///     The sdmx RegistryInterface message object representation that is generated during the xml parsing
        /// </summary>
        private IRegistryInfo _registryInterface;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistryInterfaceReaderBaseV2"/> class.
        /// </summary>
        /// <param name="namespaces">
        /// The namespaces.
        /// </param>
        protected RegistryInterfaceReaderBaseV2(SdmxNamespaces namespaces)
            : base(namespaces)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the sdmx RegistryInterface message object representation that is generated during the xml parsing
        /// </summary>
        public IRegistryInfo RegistryInterface
        {
            get
            {
                return this._registryInterface;
            }

            set
            {
                this._registryInterface = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Parses the reader opened against the stream containing the contents of a SDMX-ML Registry message or
        ///     RegistryInterface structure contents and populates the given <see cref="IRegistryInfo"/> object.
        /// </summary>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="reader"/> is null
        /// </exception>
        /// <exception cref="ParseException">
        /// SDMX structure message parsing error
        /// </exception>
        /// <param name="registry">
        /// The <see cref="IRegistryInfo"/> object to populate
        /// </param>
        /// <param name="reader">
        /// The xml reader opened against the stream containing the structure contents
        /// </param>
        public void Read(IRegistryInfo registry, XmlReader reader)
        {
            if (registry == null)
            {
                throw new ArgumentNullException("registry");
            }

            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            this._registryInterface = registry;
            this.ReadContents(reader, registry);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Create a ConstrainBean object
        /// </summary>
        /// <returns>
        ///     A ConstrainBean object
        /// </returns>
        protected IContentConstraintMutableObject CreateConstrain()
        {
            string value;
            if (this.Attributes.TryGetValue(NameTableCache.GetAttributeName(AttributeNameTable.ConstraintType), out value))
            {
                switch (value)
                {
                    case "Content":
                        return new ContentConstraintMutableCore();
                }
            }

            return null;
        }

        /// <summary>
        ///     Create a StatusMessageBean
        /// </summary>
        /// <returns>
        ///     A StatusMessageBean object
        /// </returns>
        protected IStatusMessageInfo CreateStatusMessage()
        {
            var status = new StatusMessageInfo();
            status.Status = Helper.TrySetEnumFromAttribute(this.Attributes, AttributeNameTable.status, status.Status);
            return status;
        }

        /// <summary>
        /// Handle Member Child elements
        /// </summary>
        /// <param name="parent">
        /// The parent MemberBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        protected ElementActions HandleChildElements(IKeyValuesMutable parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.MemberValue))
            {
                return this.BuildElementActions(parent.KeyValues, DoNothingComplex, this.HandleTextChildElement);
            }

            return null;
        }

        /// <summary>
        /// Handle Constraint Child elements
        /// </summary>
        /// <param name="parent">
        /// The parent IConstraintMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        protected ElementActions HandleChildElements(IContentConstraintMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.CubeRegion))
            {
                var cb = new CubeRegionMutableCore();
                bool isIncluded = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.isIncluded, true);
                if (isIncluded)
                {
                    parent.IncludedCubeRegion = cb;
                }
                else
                {
                    parent.ExcludedCubeRegion = cb;
                }

                actions = this.BuildElementActions(cb, this.HandleChildElements, DoNothing);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.ReleaseCalendar))
            {
                parent.ReleaseCalendar = new ReleaseCalendarMutableCore();
                actions = this.BuildElementActions(parent.ReleaseCalendar, DoNothingComplex, this.HandleTextChildElement);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.ReferencePeriod))
            {
                parent.ReferencePeriod = new ReferencePeriodMutableCore();
                parent.ReferencePeriod.StartTime = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.startTime, parent.ReferencePeriod.StartTime);
                parent.ReferencePeriod.EndTime = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.endTime, parent.ReferencePeriod.EndTime);
                actions = this.BuildElementActions(parent.ReferencePeriod, DoNothingComplex, DoNothing);
            }

            return actions;
        }

        /// <summary>
        /// Handle CubeRegion Child elements
        /// </summary>
        /// <param name="parent">
        /// The parent ICubeRegionMutableObject object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        protected ElementActions HandleChildElements(ICubeRegionMutableObject parent, object localName)
        {
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.Member))
            {
                IKeyValuesMutable member = new KeyValuesMutableImpl();

                ////var isIncluded = Helper.TrySetFromAttribute(this.Attributes, AttributeNameTable.isIncluded, true);
                parent.KeyValues.Add(member);
                actions = this.BuildElementActions(member, this.HandleChildElements, this.HandleTextChildElement);
            }

            return actions;
        }

        /// <summary>
        /// Handle DataflowRef Child elements
        /// </summary>
        /// <param name="parent">
        /// The parent DataflowRefBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        protected ElementActions HandleChildElements(IDataflowReferenceInfo parent, object localName)
        {
            ////if (NameTableCache.IsElement(localName, ElementNameTable.Datasource))
            ////{
            ////    var dataSource = new IDataSourceMutableObject();
            ////    parent.Datasource = dataSource;
            ////    current = dataSource;
            ////}
            ////else 
            ElementActions actions = null;
            if (NameTableCache.IsElement(localName, ElementNameTable.Constraint))
            {
                IContentConstraintMutableObject constrain = this.CreateConstrain();
                parent.Constraint = constrain;
                actions = this.AddNameableAction(constrain, this.HandleChildElements, this.HandleTextChildElement);
            }

            return actions;
        }

        /// <summary>
        /// The handle text child element.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        /// <param name="localName">
        /// The local name.
        /// </param>
        protected void HandleTextChildElement(IContentConstraintMutableObject c, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.ConstraintID))
            {
                c.Id = this.Text;
            }
        }

        /// <summary>
        /// Handle StatusMessage Child simple elements
        /// </summary>
        /// <param name="parent">
        /// The parent StatusMessageBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        protected void HandleTextChildElement(IStatusMessageInfo parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.MessageText))
            {
                parent.MessageTexts.Add(new TextTypeWrapperMutableCore { Value = this.Text, Locale = this.Lang });
            }
        }

        /// <summary>
        /// Handle MemberValue Child Text elements
        /// </summary>
        /// <param name="parent">
        /// The parent MemberValueBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        protected void HandleTextChildElement(IList<string> parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.Value))
            {
                parent.Add(this.Text);
            }
        }

        /// <summary>
        /// Handle Member Child Text elements
        /// </summary>
        /// <param name="parent">
        /// The parent MemberValueBean object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        protected void HandleTextChildElement(IKeyValuesMutable parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.ComponentRef))
            {
                parent.Id = this.Text;
            }
        }

        /// <summary>
        /// Handle A Reference based class Child Text elements
        /// </summary>
        /// <param name="parent">
        /// The parent reference object
        /// </param>
        /// <param name="localName">
        /// The name of the current XML element
        /// </param>
        protected void HandleTextChildElement(IReferenceInfo parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.URN))
            {
                parent.URN = Helper.TrySetUri(this.Text);
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.AgencyID))
            {
                parent.AgencyId = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Version))
            {
                parent.Version = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.MetadataflowID) || NameTableCache.IsElement(localName, ElementNameTable.DataflowID)
                     || NameTableCache.IsElement(localName, ElementNameTable.CodelistID) || NameTableCache.IsElement(localName, ElementNameTable.CategorySchemeID)
                     || NameTableCache.IsElement(localName, ElementNameTable.ConceptSchemeID) || NameTableCache.IsElement(localName, ElementNameTable.OrganisationSchemeID)
                     || NameTableCache.IsElement(localName, ElementNameTable.KeyFamilyID) || NameTableCache.IsElement(localName, ElementNameTable.MetadataStructureID)
                     || NameTableCache.IsElement(localName, ElementNameTable.HierarchicalCodelistID))
            {
                parent.ID = this.Text;
            }
        }

        /// <summary>
        /// Handle top level elements.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="localName">
        /// The local name.
        /// </param>
        /// <returns>
        /// The <see cref="StructureReaderBaseV20.ElementActions"/>.
        /// </returns>
        protected override ElementActions HandleTopLevel(IMutableObjects parent, object localName)
        {
            return null;
        }

        /// <summary>
        /// Read contents from <paramref name="reader"/> to <paramref name="registry"/>.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <param name="registry">
        /// The registry.
        /// </param>
        protected abstract void ReadContents(XmlReader reader, IRegistryInfo registry);

        /// <summary>
        /// Handle <see cref="IReleaseCalendarMutableObject"/> Child elements
        /// </summary>
        /// <param name="parent">
        /// The parent <see cref="IReleaseCalendarMutableObject"/> object
        /// </param>
        /// <param name="localName">
        /// The name of the current xml element
        /// </param>
        private void HandleTextChildElement(IReleaseCalendarMutableObject parent, object localName)
        {
            if (NameTableCache.IsElement(localName, ElementNameTable.Periodicity))
            {
                parent.Periodicity = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Offset))
            {
                parent.Offset = this.Text;
            }
            else if (NameTableCache.IsElement(localName, ElementNameTable.Tolerance))
            {
                parent.Tolerance = this.Text;
            }
        }

        #endregion
    }
}