// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SdmxStructureCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The sdmx object structure core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;

    /// <summary>
    ///   The sdmx object structure core.
    /// </summary>
    [Serializable]
    public abstract class SdmxStructureCore : SdmxObjectCore, ISdmxStructure
    {
        #region Fields

        /// <summary>
        ///   The _parent.
        /// </summary>
        private readonly ISdmxStructure _parent;

        /// <summary>
        ///   The identifiable composites.
        /// </summary>
        private ISet<IIdentifiableObject> _identifiableComposites;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM ITSELF, CREATES STUB OBJECT //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////    
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureCore"/> class.
        /// </summary>
        /// <param name="structureType">
        /// The structure type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected internal SdmxStructureCore(SdmxStructureType structureType, ISdmxStructure parent)
            : base(structureType, parent)
        {
            this.StructureType = structureType;
            this._parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureCore"/> class.
        /// </summary>
        /// <param name="agencyScheme">
        /// The agencyScheme. 
        /// </param>
        protected internal SdmxStructureCore(ISdmxStructure agencyScheme)
            : base(agencyScheme)
        {
            this.StructureType = agencyScheme.StructureType;
            this._parent = agencyScheme.Parent;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="SdmxStructureCore"/> class.
        /// </summary>
        /// <param name="mutableObject">
        /// The mutable object. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        protected SdmxStructureCore(IMutableObject mutableObject, ISdmxStructure parent)
            : base(mutableObject, parent)
        {
            this._parent = parent;
        }

        #endregion

        ////public override SdmxStructureType StructureType {
        ////  get {
        ////        return this.StructureType;
        ////    }
        ////}

        // $$$ ovveride _
        #region Public Properties

        /// <summary>
        ///   Gets the identifiable composites.
        /// </summary>
        public ISet<IIdentifiableObject> IdentifiableComposites
        {
            get
            {
                if (this._identifiableComposites == null)
                {
                    this._identifiableComposites = new HashSet<IIdentifiableObject>();

                    foreach (ISdmxObject currentComposite in this.Composites)
                    {
                        if (currentComposite.StructureType.IsIdentifiable
                            && !currentComposite.StructureType.IsMaintainable)
                        {
                            this._identifiableComposites.Add((IIdentifiableObject) currentComposite);
                        }
                    }
                }

                return new HashSet<IIdentifiableObject>(this._identifiableComposites);
            }
        }

        /// <summary>
        ///   Gets the identifiable parent.
        /// </summary>
        public virtual IIdentifiableObject IdentifiableParent
        {
            get
            {
                ISdmxObject currentParent = this.Parent;
                while (currentParent != null)
                {
                    if (currentParent.StructureType.IsIdentifiable)
                    {
                        return (IIdentifiableObject) currentParent;
                    }

                    currentParent = currentParent.Parent;
                }

                return null;
            }
        }

        /// <summary>
        ///   Gets the maintainable parent.
        /// </summary>
        public IMaintainableObject MaintainableParent
        {
            get
            {
                var maintainableObject = this as IMaintainableObject;
                if (maintainableObject != null)
                {
                    return maintainableObject;
                }

                var obj = this._parent as IMaintainableObject;
                if (obj != null)
                {
                    return obj;
                }

                return this._parent.MaintainableParent;
            }
        }

        /// <summary>
        ///   Gets the parent.
        /// </summary>
        public new ISdmxStructure Parent
        {
            get
            {
                return this._parent;
            }
        }

        #endregion

        #region Methods

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////COMPOSITES								 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////	

        /// <summary>
        ///   The get composites internal.
        /// </summary>
        protected override ISet<ISdmxObject> GetCompositesInternal()
        {
            return new HashSet<ISdmxObject>();
        }

        #endregion
    }
}