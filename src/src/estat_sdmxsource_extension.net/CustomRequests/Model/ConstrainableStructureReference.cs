// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstrainableStructureReference.cs" company="Eurostat">
//   Date Created : 2013-03-22
//   //   Copyright (c) 2013 by the European   Commission, represented by Eurostat.   All rights reserved.
//   //   Licensed under the European Union Public License (EUPL) version 1.1. 
//   //   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   The constrainable structure reference.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Estat.Sri.CustomRequests.Model
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;

    /// <summary>
    ///     The constrainable structure reference.
    /// </summary>
    public class ConstrainableStructureReference : StructureReferenceImpl, IConstrainableStructureReference
    {
        #region Fields

        /// <summary>
        ///     The _constraint.
        /// </summary>
        private readonly IContentConstraintObject _constraint;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainableStructureReference"/> class.
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        public ConstrainableStructureReference(Uri urn)
            : base(urn)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainableStructureReference"/> class.
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="constraint">
        /// The constraint.
        /// </param>
        public ConstrainableStructureReference(Uri urn, IContentConstraintObject constraint)
            : base(urn)
        {
            this._constraint = constraint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainableStructureReference"/> class.
        /// </summary>
        /// <param name="urn">
        /// The urn.
        /// </param>
        /// <param name="constraint">
        /// The constraint.
        /// </param>
        public ConstrainableStructureReference(string urn, IContentConstraintObject constraint)
            : base(urn)
        {
            this._constraint = constraint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainableStructureReference"/> class.
        /// </summary>
        /// <param name="structureReference">
        /// The structure Reference.
        /// </param>
        /// <param name="constraint">
        /// The constraint.
        /// </param>
        public ConstrainableStructureReference(IStructureReference structureReference, IContentConstraintObject constraint)
            : base(structureReference.MaintainableReference, structureReference.TargetReference)
        {
            this._constraint = constraint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainableStructureReference"/> class.
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
        /// <param name="constraint">
        /// The constraint.
        /// </param>
        /// <param name="identfiableIds">
        /// The identifiable ids.
        /// </param>
        public ConstrainableStructureReference(
            string agencyId, string maintainableId, string version, SdmxStructureType targetStructureEnum, IContentConstraintObject constraint, params string[] identfiableIds)
            : base(agencyId, maintainableId, version, targetStructureEnum, identfiableIds)
        {
            this._constraint = constraint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainableStructureReference"/> class.
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
        /// <param name="constraint">
        /// The constraint.
        /// </param>
        /// <param name="identfiableIds">
        /// The identifiable ids.
        /// </param>
        public ConstrainableStructureReference(
            string agencyId, string maintainableId, string version, SdmxStructureEnumType targetStructureEnum, IContentConstraintObject constraint, params string[] identfiableIds)
            : base(agencyId, maintainableId, version, targetStructureEnum, identfiableIds)
        {
            this._constraint = constraint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainableStructureReference"/> class.
        /// </summary>
        /// <param name="crossReference">
        /// The cross reference.
        /// </param>
        /// <param name="structureEnumType">
        /// The structure type.
        /// </param>
        /// <param name="constraint">
        /// The constraint.
        /// </param>
        public ConstrainableStructureReference(IMaintainableRefObject crossReference, SdmxStructureEnumType structureEnumType, IContentConstraintObject constraint)
            : base(crossReference, structureEnumType)
        {
            this._constraint = constraint;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainableStructureReference"/> class.
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
        /// <param name="constraint">
        /// The constraint.
        /// </param>
        public ConstrainableStructureReference(
            string agencyId, string maintainableId, string version, SdmxStructureType targetStructureEnum, IList<string> identfiableIds, IContentConstraintObject constraint)
            : base(agencyId, maintainableId, version, targetStructureEnum, identfiableIds)
        {
            this._constraint = constraint;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the content constraint that this structure reference can hold.
        /// </summary>
        public IContentConstraintObject ConstraintObject
        {
            get
            {
                return this._constraint;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Determines whether the specified <see cref="StructureReferenceImpl"/> is equal to the current
        ///     <see cref="StructureReferenceImpl"/>
        ///     .
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
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((ConstrainableStructureReference)obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="StructureReferenceImpl" /> .
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (this._constraint != null ? this._constraint.GetHashCode() : 0);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected bool Equals(ConstrainableStructureReference other)
        {
            return base.Equals(other) && Equals(this._constraint, other._constraint);
        }

        #endregion
    }
}