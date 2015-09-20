// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureReferenceImpl.cs" company="Eurostat">
//   Date Created : 2013-04-01
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   // 
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.Util.Objects.Reference
{
    #region Using directives

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Util;

    #endregion

    /// <summary>
    /// This bean guarantees that agencyId, MaintainableId and Version will always be non null.
    /// <p />
    /// If this bean is constructed with no version information then version 1.0 is assumed
    /// </summary>
    [Serializable]
    public class StructureReferenceImpl : IStructureReference
    {
        //// Maintainable Reference Parameters
        #region Fields

        /// <summary>
        /// The agency id.
        /// </summary>
        private string _agencyId;

        /// <summary>
        /// The child reference.
        /// </summary>
        private IIdentifiableRefObject _childReference;

        /// <summary>
        /// The hash code.
        /// </summary>
        private int _hashcode;

        /// <summary>
        /// The maintainable id.
        /// </summary>
        private string _maintainableId;

        /// <summary>
        /// The structure type.
        /// </summary>
        private SdmxStructureType _structureEnumType;

        /// <summary>
        /// The target structure type.
        /// </summary>
        private SdmxStructureType _targetStructureType;

        /// <summary>
        /// The version.
        /// </summary>
        private string _version;

        #endregion

        // Child Reference (If Applicable)
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl" /> class.
        /// </summary>
        public StructureReferenceImpl()
        {
            this._hashcode = -1;

            // Default Constructor
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="urn">
        /// The urn. 
        /// </param>
        public StructureReferenceImpl(string urn)
            : this(new Uri(urn))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="urn">
        /// The urn. 
        /// </param>
        public StructureReferenceImpl(Uri urn)
        {
            if (urn == null)
            {
                throw new ArgumentNullException("urn");
            }

            // TODO make UrnUtil work with Uri
            string urnvalue = urn.ToString();
            this._hashcode = -1;
            SdmxStructureType targetStructureEnum = UrnUtil.GetIdentifiableType(urnvalue);
            UrnUtil.ValidateURN(urnvalue, targetStructureEnum);
            string[] components = UrnUtil.GetUrnComponents(urnvalue);
            this._agencyId = components[0];
            this._maintainableId = components[1];
            this._version = UrnUtil.GetVersionFromUrn(urnvalue);
            if (string.IsNullOrEmpty(this._version))
            {
                this._version = MaintainableObject.DefaultVersion;
            }

            this._targetStructureType = targetStructureEnum;

            if (!targetStructureEnum.IsMaintainable && targetStructureEnum.IsIdentifiable)
            {
                var identfiableIds = new string[components.Length - 2];

                // $$$ String[] identfiableIds = Arrays.copyOfRange(components, 2, components.length);
                Array.ConstrainedCopy(components, 2, identfiableIds, 0, components.Length - 2);
                this._childReference = new IdentifiableRefObjetcImpl(this, identfiableIds, targetStructureEnum);
                while (true)
                {
                    targetStructureEnum = targetStructureEnum.ParentStructureType;
                    if (targetStructureEnum.IsMaintainable)
                    {
                        this._structureEnumType = targetStructureEnum;
                        break;
                    }
                }
            }
            else
            {
                this._structureEnumType = targetStructureEnum;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="xref">
        /// The cross-reference <see cref="IMaintainableObject"/> . 
        /// </param>
        /// <param name="structureEnumType">
        /// The structure type. 
        /// </param>
        public StructureReferenceImpl(IMaintainableRefObject xref, SdmxStructureType structureEnumType)
        {
            this._hashcode = -1;
            if (xref != null)
            {
                this.SetInformation(xref.AgencyId, xref.MaintainableId, xref.Version, structureEnumType);
            }
            else
            {
                this.SetInformation(null, null, null, structureEnumType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="structureEnumType">
        /// The structure type. 
        /// </param>
        public StructureReferenceImpl(SdmxStructureType structureEnumType)
            : this(null, structureEnumType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="identifiable">
        /// The <see cref="IIdentifiableObject"/> to reference 
        /// </param>
        public StructureReferenceImpl(IIdentifiableObject identifiable)
            : this(identifiable.Urn)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="parentMaint">
        /// The maintainable parent
        /// </param>
        /// <param name="targetStructure">
        /// The structure type.
        /// </param>
        /// <param name="identfiableIds">
        /// The identifiable ids. 
        /// </param>
        public StructureReferenceImpl(IMaintainableObject parentMaint, SdmxStructureType targetStructure, IList<string> identfiableIds)
        {
            this.SetInformationFromList(parentMaint.AgencyId, parentMaint.Id, parentMaint.Version, targetStructure, identfiableIds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="maintainableId">
        /// The maintainable id. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="targetStructureEnum">
        /// The target structure type. 
        /// </param>
        /// <param name="identfiableIds">
        /// The identifiable ids. 
        /// </param>
        public StructureReferenceImpl(
            string agencyId,
            string maintainableId,
            string version,
            SdmxStructureType targetStructureEnum,
            IList<string> identfiableIds)
        {
            this._hashcode = -1;
            this.SetInformationFromList(agencyId, maintainableId, version, targetStructureEnum, identfiableIds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="maintainableId">
        /// The maintainable id. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="targetStructureEnum">
        /// The target structure type. 
        /// </param>
        /// <param name="identfiableIds">
        /// The identifiable ids. 
        /// </param>
        public StructureReferenceImpl(
            string agencyId,
            string maintainableId,
            string version,
            SdmxStructureType targetStructureEnum,
            params string[] identfiableIds)
        {
            this._hashcode = -1;
            this.SetInformation(agencyId, maintainableId, version, targetStructureEnum, identfiableIds);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="maintainableId">
        /// The maintainable id. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="targetStructureEnum">
        /// The target structure type. 
        /// </param>
        /// <param name="identfiableIds">
        /// The identifiable ids. 
        /// </param>
        public StructureReferenceImpl(
            string agencyId,
            string maintainableId,
            string version,
            SdmxStructureEnumType targetStructureEnum,
            params string[] identfiableIds)
            : this(agencyId, maintainableId, version, SdmxStructureType.GetFromEnum(targetStructureEnum), identfiableIds)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureReferenceImpl"/> class.
        /// </summary>
        /// <param name="crossReference">
        /// The cross reference. 
        /// </param>
        /// <param name="structureEnumType">
        /// The structure type. 
        /// </param>
        public StructureReferenceImpl(IMaintainableRefObject crossReference, SdmxStructureEnumType structureEnumType)
            : this(crossReference, SdmxStructureType.GetFromEnum(structureEnumType))
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the agency id.
        /// </summary>
        public virtual string AgencyId
        {
            get
            {
                return this._agencyId;
            }

            set
            {
                this._agencyId = !string.IsNullOrWhiteSpace(value) ? value : null;
            }
        }

        /// <summary>
        /// Gets the child reference.
        /// </summary>
        public virtual IIdentifiableRefObject ChildReference
        {
            get
            {
                return this._childReference;
            }
        }

        /// <summary>
        /// Gets the full id.
        /// </summary>
        public string FullId
        {
            get
            {
                if (this._childReference == null)
                {
                    return null;
                }

                string returnString = string.Empty;

                string concat = string.Empty;

                foreach (string currentId in IdentifiableIds)
                {
                    returnString += concat + currentId;
                    concat = ".";
                }

                if (this._targetStructureType.EnumType == SdmxStructureEnumType.Agency)
                {
                    if (this._agencyId != null && !this._agencyId.Equals(AgencyScheme.DefaultScheme))
                    {
                        returnString = this._agencyId + "." + returnString;
                    }
                }

                return returnString;
            }
        }

        /// <summary>
        /// Gets the identifiable ids.
        /// </summary>
        public virtual IList<string> IdentifiableIds
        {
            get
            {
                if (this._childReference == null)
                {
                    return new string[0];
                }

                var ids = new List<string>();
                IIdentifiableRefObject currentRef = this._childReference;
                while (currentRef != null)
                {
                    ids.Add(currentRef.Id);

                    currentRef = currentRef.ChildReference;
                }

                return ids.ToArray();
            }
        }

        /// <summary>
        /// Gets or sets the maintainable id.
        /// </summary>
        public virtual string MaintainableId
        {
            get
            {
                return this._maintainableId;
            }

            set
            {
                this._maintainableId = !string.IsNullOrWhiteSpace(value) ? value : null;
            }
        }

        /// <summary>
        /// Gets the maintainable reference.
        /// </summary>
        public virtual IMaintainableRefObject MaintainableReference
        {
            get
            {
                return new MaintainableRefObjectImpl(this._agencyId, this._maintainableId, this._version);
            }
        }

        /// <summary>
        /// Gets or sets the maintainable structure type.
        /// </summary>
        public virtual SdmxStructureType MaintainableStructureEnumType
        {
            get
            {
                return this._structureEnumType;
            }

            set
            {
                this._structureEnumType = value;

                if (!value.IsIdentifiable)
                    throw new SdmxException("Can not set maintainable structure type to '" + value + "' as it is not identifiable or maintainable ");

                if (value != null && !value.IsMaintainable)
                {
                    SdmxStructureType targetStructureEnum = value;
                    while (true)
                    {
                        targetStructureEnum = targetStructureEnum.ParentStructureType;
                        if (targetStructureEnum.IsMaintainable)
                        {
                            this._structureEnumType = targetStructureEnum;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the maintainable urn.
        /// </summary>
        public virtual Uri MaintainableUrn
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this._agencyId) && !string.IsNullOrWhiteSpace(this._maintainableId) && !string.IsNullOrWhiteSpace(this._version))
                {
                    return this._structureEnumType.GenerateUrn(this._agencyId, this._maintainableId, this._version);
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the target reference.
        /// </summary>
        public virtual SdmxStructureType TargetReference
        {
            get
            {
                return this._targetStructureType;
            }
        }

        /// <summary>
        /// Gets or sets the target structure type.
        /// </summary>
        //// TODO Check is we need to have <see cref="SdmxStructureType "/> or SdmxStructureEnumType
        public virtual SdmxStructureType TargetStructureType
        {
            get
            {
                return this._targetStructureType;
            }

            set
            {
                this._targetStructureType = value;
                this.MaintainableStructureEnumType = value;
            }
        }

        /// <summary>
        /// Gets the target urn.
        /// </summary>
        public virtual Uri TargetUrn
        {
            get
            {
                if (this._targetStructureType == SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Agency)
                    && this._childReference != null && !string.IsNullOrWhiteSpace(this._childReference.Id))
                {
                    if (this._agencyId.Equals(AgencyScheme.DefaultScheme))
                    {
                        return new Uri(this._targetStructureType.UrnPrefix + this._childReference.Id);
                    }

                    return new Uri(this._targetStructureType.UrnPrefix + this._agencyId + "." + this._childReference.Id);
                }

                if (!string.IsNullOrWhiteSpace(this._agencyId) && !string.IsNullOrWhiteSpace(this._maintainableId) && !string.IsNullOrWhiteSpace(this._version))
                {
                    var ids = new List<string>();
                    if (this._childReference != null)
                    {
                        IIdentifiableRefObject currentRef = this._childReference;
                        while (currentRef != null)
                        {
                            //Ignore identifiable targets that do no have the ids in the URN
                            switch (currentRef.StructureEnumType.EnumType)
                            {
                                case SdmxStructureEnumType.AttributeDescriptor:
                                case SdmxStructureEnumType.DimensionDescriptor:
                                case SdmxStructureEnumType.MeasureDescriptor:
                                    if (this._targetStructureType == currentRef.StructureEnumType)
                                    {
                                        ids.Add(currentRef.Id);
                                    }
                                    break;

                                default:
                                    ids.Add(currentRef.Id);
                                    break;
                            }

                            currentRef = currentRef.ChildReference;
                        }
                    }

                    return this._targetStructureType.GenerateUrn(
                        this._agencyId, this._maintainableId, this._version, ids.ToArray());
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public virtual string Version
        {
            get
            {
                return this._version;
            }

            set
            {
                this._version = !string.IsNullOrWhiteSpace(value) ? VersionableUtil.FormatVersion(value) : null;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create copy.
        /// </summary>
        /// <returns> The <see cref="IStructureReference" /> . </returns>
        public virtual IStructureReference CreateCopy()
        {
            return new StructureReferenceImpl(
                this._agencyId, this._maintainableId, this._version, this._targetStructureType, this.IdentifiableIds);
        }

        /// <summary>
        /// Determines whether the specified <see cref="StructureReferenceImpl"/> is equal to the current <see cref="StructureReferenceImpl"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="StructureReferenceImpl"/> ; otherwise, false. 
        /// </returns>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> to compare with the current <see cref="StructureReferenceImpl"/> . 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            var that = obj as IStructureReference;
            if (that != null)
            {
                IMaintainableRefObject thatMRef = that.MaintainableReference;
                IMaintainableRefObject thisMRef = this.MaintainableReference;

                return ObjectUtil.Equivalent(thatMRef.MaintainableId, thisMRef.MaintainableId)
                       && ObjectUtil.Equivalent(thatMRef.AgencyId, thisMRef.AgencyId)
                       && ObjectUtil.Equivalent(thatMRef.Version, thisMRef.Version)
                       && ObjectUtil.Equivalent(this.ChildReference, that.ChildReference);
            }

            return false;
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns> A hash code for the current <see cref="StructureReferenceImpl" /> . </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            // TODO _hashcode & _childReference need to be read-only
            // TODO Need to check if this is used
            if (this._hashcode < 0)
            {
                string concat = string.Empty;
                if (this._childReference != null)
                {
                    concat = " - Identifiable Reference " + this._childReference;
                }

                IMaintainableRefObject maintainableReference = this.MaintainableReference;
                string str = "Target: " + this.TargetReference + "Agency Id: " + maintainableReference.AgencyId + " - "
                             + "Maintainable Id: " + maintainableReference.MaintainableId + " - " + "Version: "
                             + maintainableReference.Version + concat;

                this._hashcode = str.GetHashCode();
            }

            return this._hashcode;
        }

        /// <summary>
        /// The get match.
        /// </summary>
        /// <param name="maintainableObject">
        /// The maintainable bean. 
        /// </param>
        /// <returns>
        /// The <see cref="IIdentifiableObject"/> . 
        /// </returns>
        public virtual IIdentifiableObject GetMatch(IMaintainableObject maintainableObject)
        {
            if (this.MaintainableStructureEnumType == maintainableObject.StructureType)
            {
                if (ObjectUtil.ValidString(this.AgencyId))
                {
                    // If the AgencyId has the wildcard character then we will use Java's matches method to see if the value matches
                    // otherwise just use the equals method. Only use 'matches' when the wildcard is present to avoid any other side-effects
                    // of RegEx (e.g. the dot character)
                    if (AgencyId.Contains("*"))
                    {
                        string y = Regex.Replace(AgencyId, "\\*", ".*");
                        if (!Regex.IsMatch(maintainableObject.AgencyId, y))
                            return null;
                    }
                    else
                    {
                        if (!this.AgencyId.Equals(maintainableObject.AgencyId))
                        {
                            return null;
                        }
                    }
                }

                if (ObjectUtil.ValidString(this.MaintainableId))
                {
                    if (MaintainableId.Contains("*"))
                    {
                        string y = Regex.Replace(MaintainableId, "\\*", ".*");
                        if (!Regex.IsMatch(maintainableObject.Id, y))
                            return null;
                    }
                    else
                    {
                        if (!this.MaintainableId.Equals(maintainableObject.Id))
                            return null;
                    }
                }

                if (ObjectUtil.ValidString(this.Version))
                {
                    if (!Version.Equals("*"))
                    {
                        if (!Version.Equals(maintainableObject.Version))
                        {
                            return null;
                        }
                    }
                }

                if (this.ChildReference != null)
                {
                    foreach (IIdentifiableObject currentComposite in maintainableObject.IdentifiableComposites)
                    {
                        IIdentifiableObject matchedIdentifiable = this.ChildReference.GetMatch(currentComposite);
                        if (matchedIdentifiable != null)
                        {
                            return matchedIdentifiable;
                        }
                    }
                }
                else
                {
                    return maintainableObject;
                }
            }

            return null;
        }

        /// <summary>
        ///     Gets a value indicating whether the there is an agency Id set
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool HasAgencyId()
        {
            return ObjectUtil.ValidString(_agencyId);
        }

        /// <summary>
        /// The has child reference.
        /// </summary>
        /// <returns> The <see cref="bool" /> . </returns>
        public virtual bool HasChildReference()
        {
            return this.ChildReference != null;
        }

        /// <summary>
        ///     Gets a value indicating whether the there is a maintainable id set
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool HasMaintainableId()
        {
            return ObjectUtil.ValidString(_maintainableId);
        }

        /// <summary>
        /// The has maintainable urn.
        /// </summary>
        /// <returns> The <see cref="bool" /> . </returns>
        public virtual bool HasMaintainableUrn()
        {
            return this.TargetUrn != null;
        }

        /// <summary>
        /// The has target urn.
        /// </summary>
        /// <returns> The <see cref="bool" /> . </returns>
        public virtual bool HasTargetUrn()
        {
            return this.TargetUrn != null;
        }

        /// <summary>
        ///     Gets a value indicating whether the there is a value for version set
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" /> .
        /// </returns>
        public bool HasVersion()
        {
            return ObjectUtil.ValidString(_version);
        }

        /// <summary>
        /// The is match.
        /// </summary>
        /// <param name="maintainbleBean">
        /// The maintainable bean. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public virtual bool IsMatch(IMaintainableObject maintainbleBean)
        {
            return this.GetMatch(maintainbleBean) != null;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            string concat = string.Empty;
            if (this._childReference != null)
            {
                concat = " - Identifiable Reference: " + this._childReference;
            }

            IMaintainableRefObject maintainableReference = this.MaintainableReference;
            return "Target :" + this.TargetReference + " - " + "Agency Id: " + maintainableReference.AgencyId + " - "
                   + "Maintainable Id: " + maintainableReference.MaintainableId + " - " + "Version: "
                   + maintainableReference.Version + concat;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the information.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="maintainableId">
        /// The maintainable id. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="targetStructureEnum">
        /// The target structure. 
        /// </param>
        /// <param name="identfiableIds">
        /// The identifiable ids. 
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetStructureEnum"/>
        /// is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Can not create reference, target structure is not maintainable, and no identifiable reference parameters present
        /// </exception>
        private void SetInformation(
            string agencyId,
            string maintainableId,
            string version,
            SdmxStructureType targetStructureEnum,
            params string[] identfiableIds)
        {
            this.SetInformationFromList(agencyId, maintainableId, version, targetStructureEnum, identfiableIds);
        }

        /// <summary>
        /// Sets the information.
        /// </summary>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="maintainableId">
        /// The maintainable id. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="targetStructureEnum">
        /// The target structure. 
        /// </param>
        /// <param name="identfiableIds">
        /// The identifiable ids. 
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetStructureEnum"/>
        /// is null
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Can not create reference, target structure is not maintainable, and no identifiable reference parameters present
        /// </exception>
        private void SetInformationFromList(
            string agencyId,
            string maintainableId,
            string version,
            SdmxStructureType targetStructureEnum,
            IList<string> identfiableIds)
        {
            this._agencyId = agencyId;
            this._maintainableId = maintainableId;

            if (!string.IsNullOrWhiteSpace(version))
            {
                this._version = VersionableUtil.FormatVersion(version);
            }

            if (targetStructureEnum == null)
            {
                throw new ArgumentNullException("targetStructureEnum");
            }

            if (!ObjectUtil.ValidCollection(identfiableIds) && !targetStructureEnum.IsMaintainable
                && targetStructureEnum.EnumType != SdmxStructureEnumType.Any)
            {
                throw new ArgumentException(
                    "Can not create reference, target structure is not maintainable, and no identifiable reference parameters present");
            }

            this._targetStructureType = targetStructureEnum;

            if (!targetStructureEnum.IsMaintainable && targetStructureEnum.IsIdentifiable)
            {
                if (identfiableIds != null && identfiableIds.Count > 0)
                {
                    //// TODO set this._childReference only on ctor to make it read-only for hash method
                    this._childReference = new IdentifiableRefObjetcImpl(this, identfiableIds, targetStructureEnum);
                }

                while (true)
                {
                    targetStructureEnum = targetStructureEnum.ParentStructureType;
                    if (targetStructureEnum.IsMaintainable)
                    {
                        this._structureEnumType = targetStructureEnum;
                        break;
                    }
                }
            }
            else
            {
                this._structureEnumType = targetStructureEnum;
            }
        }

        #endregion
    }
}