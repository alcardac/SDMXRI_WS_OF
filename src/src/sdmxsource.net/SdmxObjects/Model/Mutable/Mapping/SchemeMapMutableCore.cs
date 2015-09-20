// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SchemeMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The scheme map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The scheme map mutable core.
    /// </summary>
    [Serializable]
    public abstract class SchemeMapMutableCore : NameableMutableCore, ISchemeMapMutableObject
    {
        #region Fields

        /// <summary>
        ///   The source ref.
        /// </summary>
        private IStructureReference sourceRef;

        /// <summary>
        ///   The target ref.
        /// </summary>
        private IStructureReference targetRef;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemeMapMutableCore"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        public SchemeMapMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemeMapMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public SchemeMapMutableCore(ISchemeMapObject objTarget)
            : base(objTarget)
        {
            if (objTarget.SourceRef != null)
            {
                this.sourceRef = objTarget.SourceRef.CreateMutableInstance();
            }

            if (objTarget.TargetRef != null)
            {
                this.targetRef = objTarget.TargetRef.CreateMutableInstance();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the source ref.
        /// </summary>
        public virtual IStructureReference SourceRef
        {
            get
            {
                return this.sourceRef;
            }

            set
            {
                this.sourceRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the target ref.
        /// </summary>
        public virtual IStructureReference TargetRef
        {
            get
            {
                return this.targetRef;
            }

            set
            {
                this.targetRef = value;
            }
        }

        #endregion
    }
}