// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The component mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base
{
    using System;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   The component mutable core.
    /// </summary>
    [Serializable]
    public abstract class ComponentMutableCore : IdentifiableMutableCore, IComponentMutableObject
    {
        #region Fields

        /// <summary>
        ///   The concept ref.
        /// </summary>
        private IStructureReference conceptRef;

        /// <summary>
        ///   The representation.
        /// </summary>
        private IRepresentationMutableObject representation;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMutableCore"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        public ComponentMutableCore(SdmxStructureType structureType)
            : base(structureType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public ComponentMutableCore(IComponent objTarget)
            : base(objTarget)
        {
            if (objTarget.Representation != null)
            {
                this.representation = new RepresentationMutableCore(objTarget.Representation);
            }

            if (objTarget.ConceptRef != null)
            {
                this.conceptRef = objTarget.ConceptRef.CreateMutableInstance();
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the concept ref.
        /// </summary>
        public virtual IStructureReference ConceptRef
        {
            get
            {
                return this.conceptRef;
            }

            set
            {
                this.conceptRef = value;
            }
        }

        /// <summary>
        ///   Gets or sets the representation.
        /// </summary>
        public virtual IRepresentationMutableObject Representation
        {
            get
            {
                return this.representation;
            }

            set
            {
                this.representation = value;
            }
        }

        #endregion
    }
}