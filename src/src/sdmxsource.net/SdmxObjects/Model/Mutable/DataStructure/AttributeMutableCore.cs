// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttributeMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The attribute mutable core.
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
    ///   The attribute mutable core.
    /// </summary>
    [Serializable]
    public class AttributeMutableCore : ComponentMutableCore, IAttributeMutableObject
    {
        #region Fields

        /// <summary>
        ///   The concept roles.
        /// </summary>
        private readonly IList<IStructureReference> _conceptRoles;

        /// <summary>
        ///   The dimension reference.
        /// </summary>
        private readonly IList<string> _dimensionReferences;

        /// <summary>
        ///   The assignment status.
        /// </summary>
        private string _assignmentStatus;

        /// <summary>
        ///   The attachment group.
        /// </summary>
        private string _attachmentGroup;

        /// <summary>
        ///   The attachment level.
        /// </summary>
        private AttributeAttachmentLevel _attachmentLevel;

        /// <summary>
        ///   The primary measure reference.
        /// </summary>
        private string _primaryMeasureReference;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="AttributeMutableCore" /> class.
        /// </summary>
        public AttributeMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.DataAttribute))
        {
            this._dimensionReferences = new List<string>();
            this._conceptRoles = new List<IStructureReference>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AttributeMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The agencySchemeMutable target. 
        /// </param>
        public AttributeMutableCore(IAttributeObject objTarget)
            : base(objTarget)
        {
            this._dimensionReferences = new List<string>();
            this._conceptRoles = new List<IStructureReference>();
            this._attachmentLevel = objTarget.AttachmentLevel;
            this._assignmentStatus = objTarget.AssignmentStatus;
            this._attachmentGroup = objTarget.AttachmentGroup;
            this._dimensionReferences = objTarget.DimensionReferences;
            this._primaryMeasureReference = objTarget.PrimaryMeasureReference;
            if (objTarget.ConceptRoles != null)
            {
                foreach (ICrossReference currentConceptRole in objTarget.ConceptRoles)
                {
                    this._conceptRoles.Add(currentConceptRole.CreateMutableInstance());
                }
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the assignment status.
        /// </summary>
        public string AssignmentStatus
        {
            get
            {
                return this._assignmentStatus;
            }

            set
            {
                this._assignmentStatus = value;
            }
        }

        /// <summary>
        ///   Gets or sets the attachment group.
        /// </summary>
        public string AttachmentGroup
        {
            get
            {
                return this._attachmentGroup;
            }

            set
            {
                this._attachmentGroup = value;
            }
        }

        /// <summary>
        ///   Gets or sets the attachment level.
        /// </summary>
        public AttributeAttachmentLevel AttachmentLevel
        {
            get
            {
                return this._attachmentLevel;
            }

            set
            {
                this._attachmentLevel = value;
            }
        }

        /// <summary>
        ///   Gets the concept roles.
        /// </summary>
        public IList<IStructureReference> ConceptRoles
        {
            get
            {
                return this._conceptRoles;
            }
        }

        /// <summary>
        ///   Gets the dimension reference.
        /// </summary>
        public IList<string> DimensionReferences
        {
            get
            {
                return this._dimensionReferences;
            }
        }

        /// <summary>
        ///   Gets or sets the primary measure reference.
        /// </summary>
        public string PrimaryMeasureReference
        {
            get
            {
                return this._primaryMeasureReference;
            }

            set
            {
                this._primaryMeasureReference = value;
            }
        }

        public void AddConceptRole(IStructureReference structureReference)
        {
            _conceptRoles.Add(structureReference);
        }

        #endregion
    }
}