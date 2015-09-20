// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossReferenceTreeMutableCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   Used to send to external applications that require a default constructor
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Reference
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Manager.Retrieval;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Reference;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   Used to send to external applications that require a default constructor
    /// </summary>
    public class CrossReferenceTreeMutableCore : ICrossReferenceTreeMutable
    {
        #region Fields

        /// <summary>
        ///   The _referencing objectList.
        /// </summary>
        private readonly IList<ICrossReferenceTreeMutable> _referencingObjects;

        private IMaintainableMutableObject _maintainableMutableObject;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="CrossReferenceTreeMutableCore" /> class.
        /// </summary>
        public CrossReferenceTreeMutableCore()
        {
            this._referencingObjects = new List<ICrossReferenceTreeMutable>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceTreeMutableCore"/> class.
        /// </summary>
        /// <param name="crossReferencingTree">
        /// The cross referencing tree. 
        /// </param>
        public CrossReferenceTreeMutableCore(ICrossReferencingTree crossReferencingTree)
            : this(crossReferencingTree, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferenceTreeMutableCore"/> class.
        /// </summary>
        /// <param name="crossReferencingTree">
        /// The cross referencing tree. 
        /// </param>
        /// <param name="serviceRetrievalManager">
        /// The service retrieval manager. 
        /// </param>
        public CrossReferenceTreeMutableCore(
            ICrossReferencingTree crossReferencingTree, IServiceRetrievalManager serviceRetrievalManager)
        {
            this._referencingObjects = new List<ICrossReferenceTreeMutable>();
            if (serviceRetrievalManager != null)
            {
                this._maintainableMutableObject =
                    serviceRetrievalManager.CreateStub(crossReferencingTree.Maintainable).MutableInstance;
            }
            else
            {
                this._maintainableMutableObject = crossReferencingTree.Maintainable.MutableInstance;
            }

            foreach (ICrossReferencingTree currentChildReference in crossReferencingTree.ReferencingStructures)
            {
                this._referencingObjects.Add(new CrossReferenceTreeMutableCore(currentChildReference));
            }
        }

        #endregion

        #region Public Properties

        public virtual IMaintainableMutableObject MaintianableObject
        {
            get
            {
                return this._maintainableMutableObject;
            }
            set
            {
                this._maintainableMutableObject = value;
            }
        }

        /// <summary>
        ///   Gets or sets the maintianable.
        /// </summary>
       

        /// <summary>
        ///   Gets or sets the referencing objectList.
        /// </summary>
        public IList<ICrossReferenceTreeMutable> ReferencingObjects
        {
            get
            {
                return this._referencingObjects;
            }
        }

        #endregion
    }
}