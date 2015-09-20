// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The group mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The group mutable core.
    /// </summary>
    [Serializable]
    public class GroupMutableCore : IdentifiableMutableCore, IGroupMutableObject
    {
        #region Fields

        /// <summary>
        ///   The _dimension ref.
        /// </summary>
        private readonly IList<string> _dimensionRef;

        /// <summary>
        ///   The _attachment constraint ref.
        /// </summary>
        private IStructureReference _attachmentConstraintRef;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="GroupMutableCore" /> class.
        /// </summary>
        public GroupMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Group))
        {
            this._dimensionRef = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public GroupMutableCore(IGroup objTarget)
            : base(objTarget)
        {
            if (objTarget.AttachmentConstraintRef != null)
            {
                this._attachmentConstraintRef = objTarget.AttachmentConstraintRef;
            }

            this._dimensionRef = objTarget.DimensionRefs;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the attachment constraint ref.
        /// </summary>
        public IStructureReference AttachmentConstraintRef
        {
            get
            {
                return this._attachmentConstraintRef;
            }

            set
            {
                this._attachmentConstraintRef = value;
            }
        }

        /// <summary>
        ///   Gets the dimension ref.
        /// </summary>
        public IList<string> DimensionRef
        {
            get
            {
                return this._dimensionRef;
            }
        }

        #endregion
    }
}