// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The group core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.DataStructure
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Structure;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.DataStructure;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Util;
    using Org.Sdmxsource.Sdmx.Util.Objects.Reference;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The group core.
    /// </summary>
    [Serializable]
    public class GroupCore : IdentifiableCore, IGroup
    {
        #region Fields

        /// <summary>
        ///   The attachment constraint ref.
        /// </summary>
        private readonly ICrossReference attachmentConstraintRef;

        /// <summary>
        ///   The dimension ref.
        /// </summary>
        private readonly IList<string> dimensionRef;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECTS              //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupCore"/> class.
        /// </summary>
        /// <param name="itemMutableObject">
        /// The agencyScheme. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public GroupCore(IGroupMutableObject itemMutableObject, ISdmxStructure parent)
            : base(itemMutableObject, parent)
        {
            this.dimensionRef = new List<string>();
            if (itemMutableObject.AttachmentConstraintRef != null)
            {
                this.attachmentConstraintRef = new CrossReferenceImpl(this, itemMutableObject.AttachmentConstraintRef);
            }

            if (itemMutableObject.DimensionRef != null)
            {
                this.dimensionRef = new List<string>(itemMutableObject.DimensionRef);
            }

            this.ValidateGroupAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupCore"/> class.
        /// </summary>
        /// <param name="group">
        /// The group. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public GroupCore(GroupType group, ISdmxStructure parent)
            : base(group, SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Group), parent)
        {
            this.dimensionRef = new List<string>();
            if (group.AttachmentConstraint != null)
            {
                this.attachmentConstraintRef = RefUtil.CreateReference(this, group.AttachmentConstraint);
            }

            if (group.GroupDimension != null)
            {
                this.dimensionRef = new List<string>();
                foreach (GroupDimension each in group.GroupDimension)
                {
                    this.dimensionRef.Add(RefUtil.CreateLocalIdReference(each.DimensionReference));
                }
            }

            this.ValidateGroupAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupCore"/> class.
        /// </summary>
        /// <param name="group">
        /// The group. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public GroupCore(Org.Sdmx.Resources.SdmxMl.Schemas.V20.structure.GroupType group, ISdmxStructure parent)
            : base(
                group, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Group), 
                group.id, 
                null, 
                group.Annotations, 
                parent)
        {
            this.dimensionRef = new List<string>();

            /*
             * We do NOT support Attachment Constraint References in Version 2 since they are essentially useless.
             */
            if (group.DimensionRef != null)
            {
                this.dimensionRef = new List<string>(group.DimensionRef);
            }

            this.ValidateGroupAttributes();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupCore"/> class.
        /// </summary>
        /// <param name="group">
        /// The group. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public GroupCore(Org.Sdmx.Resources.SdmxMl.Schemas.V10.structure.GroupType group, ISdmxStructure parent)
            : base(
                group, 
                SdmxStructureType.GetFromEnum(SdmxStructureEnumType.Group), 
                group.id, 
                null, 
                group.Annotations, 
                parent)
        {
            this.dimensionRef = new List<string>();
            if (group.DimensionRef != null)
            {
                this.dimensionRef = new List<string>(group.DimensionRef);
            }

            this.ValidateGroupAttributes();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the attachment constraint ref.
        /// </summary>
        public virtual ICrossReference AttachmentConstraintRef
        {
            get
            {
                return this.attachmentConstraintRef;
            }
        }

        /// <summary>
        ///   Gets the dimension ref.
        /// </summary>
        public virtual IList<string> DimensionRefs
        {
            get
            {
                return new List<string>(this.dimensionRef);
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencyScheme. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null) return false;
            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IGroup)sdmxObject;
                if (!ObjectUtil.EquivalentCollection(this.dimensionRef, that.DimensionRefs))
                {
                    return false;
                }

                if (!ObjectUtil.Equivalent(this.attachmentConstraintRef, that.AttachmentConstraintRef))
                {
                    return false;
                }

                return this.DeepEqualsInternal(that, includeFinalProperties);
            }

            return false;
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATION                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Methods

        /// <summary>
        ///   The validate group attributes.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        protected internal void ValidateGroupAttributes()
        {
            if (this.attachmentConstraintRef == null && (this.dimensionRef == null || this.dimensionRef.Count == 0))
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMissingRequiredElement, "Group", "DimensionRefs/AttachmentConstraintRef");
            }

            if (this.attachmentConstraintRef != null && this.dimensionRef.Count > 0)
            {
                throw new SdmxSemmanticException(
                    ExceptionCode.ObjectMutuallyExclusive, "DimensionRefs", "AttachmentConstraintRef", "Group");
            }
        }

        #endregion
    }
}