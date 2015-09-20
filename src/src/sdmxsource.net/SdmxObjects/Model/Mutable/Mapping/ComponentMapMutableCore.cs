// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The component map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The component map mutable core.
    /// </summary>
    [Serializable]
    public class ComponentMapMutableCore : AnnotableMutableCore, IComponentMapMutableObject
    {
        #region Fields

        /// <summary>
        ///   The map concept ref.
        /// </summary>
        private string mapConceptRef;

        /// <summary>
        ///   The map target concept ref.
        /// </summary>
        private string mapTargetConceptRef;

        /// <summary>
        ///   The rep map ref.
        /// </summary>
        private IRepresentationMapRefMutableObject repMapRef;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ComponentMapMutableCore" /> class.
        /// </summary>
        public ComponentMapMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ComponentMap))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMapMutableCore"/> class.
        /// </summary>
        /// <param name="compMap">
        /// The comp map. 
        /// </param>
        public ComponentMapMutableCore(IComponentMapObject compMap)
            : base(compMap)
        {
            if (compMap.MapConceptRef != null)
            {
                this.mapConceptRef = compMap.MapConceptRef;
            }

            if (compMap.MapTargetConceptRef != null)
            {
                this.mapTargetConceptRef = compMap.MapTargetConceptRef;
            }

            if (compMap.RepMapRef != null)
            {
                this.repMapRef = new RepresentationMapRefMutableCore(compMap.RepMapRef);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the map concept ref.
        /// </summary>
        public virtual string MapConceptRef
        {
            get
            {
                return this.mapConceptRef;
            }

            set
            {
                this.mapConceptRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the map target concept ref.
        /// </summary>
        public virtual string MapTargetConceptRef
        {
            get
            {
                return this.mapTargetConceptRef;
            }

            set
            {
                this.mapTargetConceptRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the rep map ref.
        /// </summary>
        public virtual IRepresentationMapRefMutableObject RepMapRef
        {
            get
            {
                return this.repMapRef;
            }

            set
            {
                this.repMapRef = value;
            }
        }

        #endregion
    }
}