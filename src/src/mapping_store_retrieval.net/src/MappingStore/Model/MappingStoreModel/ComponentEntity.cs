// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentEntity.cs" company="Eurostat">
//   Date Created : 2011-10-21
//   Copyright (c) 2011 by the European   Commission, represented by Eurostat. All rights reserved.
//   Licensed under the European Union Public License (EUPL) version 1.1. 
//   If you do not accept this license, you are not allowed to make any use of this file.
// </copyright>
// <summary>
//   This is a class represenging a component (Dimension, Attribute, Time Dimension)
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Estat.Sri.MappingStoreRetrieval.Model.MappingStoreModel
{
    using System;
    using System.Collections.ObjectModel;

    using Estat.Sri.MappingStoreRetrieval.Constants;

    /// <summary>
    /// This is a class representing a component (Dimension, Attribute, Time Dimension)
    /// </summary>
    public class ComponentEntity : PersistentEntityBase
    {
        #region Constants and Fields

        /// <summary>
        /// The list of componenet attribute assigmnet groups
        /// </summary>
        private readonly Collection<GroupEntity> _attAssignmentGroups;

        /// <summary>
        /// The component attribute status
        /// </summary>
        private AssignmentStatus _attStatus;

        /// <summary>
        /// The component attribute assignment level
        /// </summary>
        private AttachmentLevel _attributeAttachmentLevel;

        /// <summary>
        /// The component type
        /// </summary>
        private SdmxComponentType _componentType;

        /// <summary>
        /// This field holds the currect cross sectional attachment levels
        /// </summary>
        private CrossSectionalLevels _crossSectionalAttachmentLevel;

        /// <summary>
        /// This field holds the effective cross sectional attachment levels
        /// </summary>
        private CrossSectionalLevels _effectiveCrossSectionalAttachmentLevel;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentEntity"/> class. 
        /// Default contstructor used to initialize the <see cref="ComponentEntity._attAssignmentGroups"/>
        /// </summary>
        /// <param name="sysId">
        /// The sys Id.
        /// </param>
        public ComponentEntity(long sysId)
            : base(sysId)
        {
            this._attAssignmentGroups = new Collection<GroupEntity>();
        }

        #endregion

        #region Enums

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the list of component attribute assignment groups
        /// </summary>
        public Collection<GroupEntity> AttAssignmentGroups
        {
            get
            {
                return this._attAssignmentGroups;
            }
        }

        /// <summary>
        /// Gets or sets the component attribute status
        /// </summary>
        public AssignmentStatus AttStatus
        {
            get
            {
                return this._attStatus;
            }

            set
            {
                this._attStatus = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the attribute is time format
        /// </summary>
        public bool AttTimeFormat { get; set; }

        /// <summary>
        /// Gets or sets the component attribute assignement level 
        /// </summary>
        public AttachmentLevel AttributeAttachmentLevel
        {
            get
            {
                return this._attributeAttachmentLevel;
            }

            set
            {
                this._attributeAttachmentLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the component codelist
        /// </summary>
        public CodeListEntity CodeList { get; set; }

        /// <summary>
        /// Gets or sets the components type
        /// </summary>
        public SdmxComponentType ComponentType
        {
            get
            {
                return this._componentType;
            }

            set
            {
                this._componentType = value;
            }
        }

        /// <summary>
        /// Gets or sets the component concept
        /// </summary>
        public ConceptEntity Concept { get; set; }

        /// <summary>
        /// Gets or sets the component ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets the currect cross sectional attachment levels
        /// </summary>
        public CrossSectionalLevels CrossSectionalAttachmentLevel
        {
            get
            {
                return this._crossSectionalAttachmentLevel;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component attribute has a cross sectional dataset level
        /// </summary>
        public bool CrossSectionalLevelDataSet
        {
            get
            {
                return (this._crossSectionalAttachmentLevel & CrossSectionalLevels.DataSet) != 0;
            }

            set
            {
                this.SetLevel(value, CrossSectionalLevels.DataSet);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component attribute has a cross sectional group level
        /// </summary>
        public bool CrossSectionalLevelGroup
        {
            get
            {
                return (this._crossSectionalAttachmentLevel & CrossSectionalLevels.Group) != 0;
            }

            set
            {
                this.SetLevel(value, CrossSectionalLevels.Group);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component attribute has a cross sectional observation level
        /// </summary>
        public bool CrossSectionalLevelObs
        {
            get
            {
                return (this._crossSectionalAttachmentLevel & CrossSectionalLevels.Observation) != 0;
            }

            set
            {
                this.SetLevel(value, CrossSectionalLevels.Observation);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component attribute has a cross sectional section level
        /// </summary>
        public bool CrossSectionalLevelSection
        {
            get
            {
                return (this._crossSectionalAttachmentLevel & CrossSectionalLevels.Section) != 0;
            }

            set
            {
                this.SetLevel(value, CrossSectionalLevels.Section);
            }
        }

        /// <summary>
        /// Gets or sets the component cross sectioonal measure code
        /// </summary>
        public string CrossSectionalMeasureCode { get; set; }

        /// <summary>
        /// Gets or sets the effective cross sectional attachment level
        /// </summary>
        public CrossSectionalLevels EffectiveCrossSectionalAttachmentLevel
        {
            get
            {
                return this._effectiveCrossSectionalAttachmentLevel;
            }

            set
            {
                this.SetEffectiveXsLevel(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the component is frequency dimension 
        /// </summary>
        public bool FrequencyDimension { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component is a measure dimension
        /// </summary>
        public bool MeasureDimension { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The method is used to add a <see cref="GroupEntity"/> 
        /// to the component attribute assignment groups
        /// </summary>
        /// <param name="group">
        /// The <see cref="GroupEntity"/> 
        /// that needs to be added
        /// </param>
        public void AddAttAssignmentGroups(GroupEntity group)
        {
            this._attAssignmentGroups.Add(group);
        }

        /// <summary>
        /// Set <see cref="AttStatus"/> from string. If the string is not valid then <see cref="AttStatus"/> is set to <see cref="AssignmentStatus.None"/>
        /// </summary>
        /// <param name="status">
        /// The assignment status
        /// </param>
        public void SetAssignmentStatus(string status)
        {
            if (string.IsNullOrEmpty(status) || !Enum.TryParse(status, true, out this._attStatus))
            {
                this._attStatus = AssignmentStatus.None; // redundant but specified for readability
            }
        }

        /// <summary>
        /// Set <see cref="AttributeAttachmentLevel"/> from string. If the string is not valid then <see cref="AttributeAttachmentLevel"/> is set to <see cref="AttachmentLevel.None"/>
        /// </summary>
        /// <param name="level">
        /// The attachment level
        /// </param>
        public void SetAttachmentLevel(string level)
        {
            if (string.IsNullOrEmpty(level) || !Enum.TryParse(level, true, out this._attributeAttachmentLevel))
            {
                this._attributeAttachmentLevel = AttachmentLevel.None; // redundant but specified for readability
            }
        }

        /// <summary>
        /// Set <see cref="ComponentType"/> from string. If the string is not valid then <see cref="ComponentType"/> is set to <see cref="SdmxComponentType.None"/>
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        public void SetType(string type)
        {
            if (string.IsNullOrEmpty(type) || !Enum.TryParse(type, true, out this._componentType))
            {
                this._componentType = SdmxComponentType.None; // redundant but specified for readability
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Conditionally set the effective CrossSectional attachment level if <paramref name="value"/> is larger than <see cref="_effectiveCrossSectionalAttachmentLevel"/>
        /// </summary>
        /// <param name="value">
        /// The new level to use
        /// </param>
        private void SetEffectiveXsLevel(CrossSectionalLevels value)
        {
            if (value > this._effectiveCrossSectionalAttachmentLevel)
            {
                this._effectiveCrossSectionalAttachmentLevel = value;
            }
        }

        /// <summary>
        /// Set or unset the <paramref name="level"/> from <see cref="_crossSectionalAttachmentLevel"/>.
        /// Also the <see cref="_effectiveCrossSectionalAttachmentLevel"/> is set or unset depending on the current <see cref="_effectiveCrossSectionalAttachmentLevel"/> value and the <paramref name="value"/>
        /// </summary>
        /// <param name="value">
        /// A value indicating whether the <paramref name="level"/> should be set or unset
        /// </param>
        /// <param name="level">
        /// The <see cref="CrossSectionalLevels"/> to set or unset
        /// </param>
        private void SetLevel(bool value, CrossSectionalLevels level)
        {
            if (value)
            {
                this._crossSectionalAttachmentLevel |= level;
                this.SetEffectiveXsLevel(level);
            }
            else
            {
                this._crossSectionalAttachmentLevel &= ~level;
                if (this._effectiveCrossSectionalAttachmentLevel == level)
                {
                    this._effectiveCrossSectionalAttachmentLevel = CrossSectionalLevels.None;
                }
            }
        }

        #endregion
    }
}