// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossReferenceImpl.cs" company="Eurostat">
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

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    #endregion


    /// <summary>
    /// The cross reference implementation.
    /// </summary>
    [Serializable]
    public class CrossReferenceImpl : StructureReferenceImpl, ICrossReference
    {
        #region Fields

        /// <summary>
        /// The referenced from <see cref="ISdmxObject" />.
        /// </summary>
        private readonly ISdmxObject _referencedFrom;

        #endregion

        // FUNC this constructor does not account the sRef having the incorrect SdmxStructureType (i.e from a mutable agencySchemeMutableObject)
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceImpl"/> class.
        /// </summary>
        /// <param name="referencedFrom">
        /// The referenced from. 
        /// </param>
        /// <param name="structureReference">
        /// The structure reference 
        /// </param>
        public CrossReferenceImpl(ISdmxObject referencedFrom, IStructureReference structureReference)
            : base(
                structureReference.MaintainableReference.AgencyId, 
                structureReference.MaintainableReference.MaintainableId, 
                structureReference.MaintainableReference.Version, 
                structureReference.TargetReference, 
                structureReference.IdentifiableIds)
        {
            this._referencedFrom = referencedFrom;
            this.ValidateReference();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceImpl"/> class.
        /// </summary>
        /// <param name="referencedFrom">
        /// The referenced from. 
        /// </param>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="maintainableId">
        /// The maintainable id. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        public CrossReferenceImpl(
            ISdmxObject referencedFrom, 
            string agencyId, 
            string maintainableId, 
            string version, 
            SdmxStructureType structureType)
            : base(agencyId, maintainableId, version, structureType)
        {
            this._referencedFrom = referencedFrom;
            this.ValidateReference();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceImpl"/> class.
        /// </summary>
        /// <param name="referencedFrom">
        /// The referenced from. 
        /// </param>
        /// <param name="agencyId">
        /// The agency id. 
        /// </param>
        /// <param name="maintainableId">
        /// The maintainable id. 
        /// </param>
        /// <param name="version">
        /// The version. 
        /// </param>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="identifiableIds">
        /// The identifiable ids. 
        /// </param>
        public CrossReferenceImpl(
            ISdmxObject referencedFrom, 
            string agencyId, 
            string maintainableId, 
            string version, 
            SdmxStructureEnumType structureType, 
            params string[] identifiableIds)
            : base(agencyId, maintainableId, version, structureType, identifiableIds)
        {
            this._referencedFrom = referencedFrom;
            this.ValidateReference();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceImpl"/> class.
        /// </summary>
        /// <param name="referencedFrom">
        /// The referenced from. 
        /// </param>
        /// <param name="urn">
        /// The urn. 
        /// </param>
        public CrossReferenceImpl(ISdmxObject referencedFrom, string urn)
            : base(urn)
        {
            this._referencedFrom = referencedFrom;
            this.ValidateReference();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceImpl"/> class.
        /// </summary>
        /// <param name="referencedFrom">
        /// The referenced from. 
        /// </param>
        /// <param name="urn">
        /// The urn. 
        /// </param>
        public CrossReferenceImpl(ISdmxObject referencedFrom, Uri urn)
            : base(urn)
        {
            this._referencedFrom = referencedFrom;
            this.ValidateReference();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets the agency id.
        /// </summary>
        /// <exception cref="SdmxNotImplementedException">
        /// <see cref="AgencyId" />/// -
        /// <see cref="CrossReferenceImpl" />   is immutable</exception>
        public override string AgencyId
        {
            set
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported, "CrossReferenceImpl.AgencyId -  CrossReferenceImpl is immutable");
            }
        }

        /////// <summary>
        /////// Sets the child reference.
        /////// </summary>
        /////// <exception cref="SdmxNotImplementedException">
        /////// <see cref="ChildReference" />
        /////// -
        /////// <see cref="CrossReferenceImpl" />
        /////// is immutable</exception>
        ////public override IIdentifiableRefObject ChildReference
        ////{
        ////    set
        ////    {
        ////        throw new SdmxNotImplementedException(
        ////            ExceptionCode.Unsupported, "CrossReferenceImpl.ChildReference -  CrossReferenceImpl is immutable");
        ////    }
        ////}

        /// <summary>
        /// Sets the maintainable id.
        /// </summary>
        /// <exception cref="SdmxNotImplementedException">
        /// <see cref="StructureReferenceImpl.MaintainableId" />
        /// -
        /// <see cref="CrossReferenceImpl" />
        /// is immutable</exception>
        public override string MaintainableId
        {
            set
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported, "CrossReferenceImpl.MaintainableId -  CrossReferenceImpl is immutable");
            }
        }

        /// <summary>
        /// Sets the maintainable structure type.
        /// </summary>
        /// <exception cref="SdmxNotImplementedException">
        /// <see cref="MaintainableStructureType" />
        /// -
        /// <see cref="CrossReferenceImpl" />
        /// is immutable</exception>
        public SdmxStructureType MaintainableStructureType
        {
            set
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported, 
                    "CrossReferenceImpl.MaintainableStructureType -  CrossReferenceImpl is immutable");
            }
        }

        /// <summary>
        /// Gets the referenced from.
        /// </summary>
        public virtual ISdmxObject ReferencedFrom
        {
            get
            {
                return this._referencedFrom;
            }
        }

        /// <summary>
        /// Sets the target structure type.
        /// </summary>
        /// <exception cref="SdmxNotImplementedException">
        /// <see cref="TargetStructureType" />
        /// -
        /// <see cref="CrossReferenceImpl" />
        /// is immutable</exception>
        public override SdmxStructureType TargetStructureType
        {
            set
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported, 
                    "CrossReferenceImpl.TargetStructureType -  CrossReferenceImpl is immutable");
            }
        }

        /// <summary>
        /// Sets the version.
        /// </summary>
        /// <exception cref="SdmxNotImplementedException">
        /// <see cref="Version" />
        /// -
        /// <see cref="CrossReferenceImpl" />
        /// is immutable</exception>
        public override string Version
        {
            set
            {
                throw new SdmxNotImplementedException(
                    ExceptionCode.Unsupported, "CrossReferenceImpl.Version -  CrossReferenceImpl is immutable");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets an array of identifiable id from the specified <paramref name="xref"/>
        /// </summary>
        /// <param name="xref">
        /// The cross reference. 
        /// </param>
        /// <returns>
        /// The an array of identifiable id from the specified <paramref name="xref"/> ; otherwise null 
        /// </returns>
        public static string[] GetIdentifiableIds(IIdentifiableRefObject xref)
        {
            if (xref == null)
            {
                return null;
            }

            var ids = new List<string>();
            while (xref.ChildReference != null)
            {
                ids.Add(xref.ChildReference.Id);
            }

            return ids.ToArray();
        }

        /// <summary>
        /// The create mutable instance.
        /// </summary>
        /// <returns> The <see cref="IStructureReference" /> . </returns>
        public virtual IStructureReference CreateMutableInstance()
        {
            return new StructureReferenceImpl(this.TargetUrn);
        }

        /// <summary>
        /// The is match.
        /// </summary>
        /// <param name="identifiableObject">
        /// The identifiable agencySchemeMutableObject. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public virtual bool IsMatch(IIdentifiableObject identifiableObject)
        {
            if (identifiableObject.StructureType.EnumType == this.TargetReference.EnumType)
            {
                return this.TargetUrn.Equals(identifiableObject.Urn);
            }

            return false;
        }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            return "Cross Reference : " + this.TargetUrn;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The validate reference.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">
        /// <see cref="_referencedFrom" />
        /// is null</exception>
        /// <exception cref="SdmxSemmanticException">
        /// <see cref="StructureReferenceImpl.MaintainableReference" />
        /// is not valid</exception>
        private void ValidateReference()
        {
            if (this._referencedFrom == null)
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectMissingRequiredElement, "referencedFrom");
            }

            if (string.IsNullOrWhiteSpace(this.MaintainableReference.AgencyId))
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectIncompleteReference, "Agency Id");
            }

            if (string.IsNullOrWhiteSpace(this.MaintainableReference.MaintainableId))
            {
                throw new SdmxSemmanticException(ExceptionCode.ObjectIncompleteReference, "Maintainable Id");
            }

            if (string.IsNullOrWhiteSpace(this.MaintainableReference.Version))
            {
                base.Version = "1.0";
            }

            if (!this.TargetReference.IsMaintainable)
            {
                if (this.ChildReference == null)
                {
                    throw new SdmxSemmanticException(
                        "Reference to " + this.TargetReference.StructureType + " missing identifiable parameters");
                }
            }
        }

        #endregion
    }
}