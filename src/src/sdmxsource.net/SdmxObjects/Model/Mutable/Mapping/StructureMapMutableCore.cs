// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StructureMapMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The structure map mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Mapping
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Mapping;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Mapping;

    /// <summary>
    ///   The structure map mutable core.
    /// </summary>
    [Serializable]
    public class StructureMapMutableCore : SchemeMapMutableCore, IStructureMapMutableObject
    {
        #region Fields

        /// <summary>
        ///   The components.
        /// </summary>
        private readonly IList<IComponentMapMutableObject> _components;

        /// <summary>
        ///   The extension.
        /// </summary>
        private bool _extension;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="StructureMapMutableCore" /> class.
        /// </summary>
        public StructureMapMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.StructureMap))
        {
            this._components = new List<IComponentMapMutableObject>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapMutableCore"/> class.
        /// </summary>
        /// <param name="objTarget">
        /// The obj target. 
        /// </param>
        public StructureMapMutableCore(IStructureMapObject objTarget)
            : base(objTarget)
        {
            this._extension = objTarget.Extension;

            // create mutable list of component maps
            if (objTarget.Components != null)
            {
                foreach (IComponentMapObject componentMapObject in objTarget.Components)
                {
                    this.AddComponent(new ComponentMapMutableCore(componentMapObject));
                }
            }
        }

        #endregion

        // has Component Map objTarget
        #region Public Properties

        /// <summary>
        ///   Gets the components.
        /// </summary>
        public IList<IComponentMapMutableObject> Components
        {
            get
            {
                return this._components;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether extension.
        /// </summary>
        public bool Extension
        {
            get
            {
                return this._extension;
            }

            set
            {
                this._extension = value;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add component.
        /// </summary>
        /// <param name="component">
        /// The component. 
        /// </param>
        public void AddComponent(IComponentMapMutableObject component)
        {
            this._components.Add(component);
        }

        #endregion
    }
}