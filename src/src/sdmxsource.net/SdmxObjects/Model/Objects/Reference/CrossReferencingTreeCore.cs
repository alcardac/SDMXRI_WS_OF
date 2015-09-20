// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrossReferencingTreeCore.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The cross referencing tree impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Reference
{
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Reference;

    /// <summary>
    ///   The cross referencing tree impl.
    /// </summary>
    public class CrossReferencingTreeCore : ICrossReferencingTree
    {
        #region Fields

        /// <summary>
        ///   The imaintianable.
        /// </summary>
        private readonly IMaintainableObject maintainableObject;

        /// <summary>
        ///   The referencing objects.
        /// </summary>
        private readonly IList<ICrossReferencingTree> referencingObjects;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CrossReferencingTreeCore"/> class.
        /// </summary>
        /// <param name="maintainable">
        /// The maintainable. 
        /// </param>
        /// <param name="referencingObjects">
        /// The referencing objects 
        /// </param>
        public CrossReferencingTreeCore(
            IMaintainableObject maintainable, IList<ICrossReferencingTree> referencingObjects)
        {
            this.referencingObjects = new List<ICrossReferencingTree>();
            this.maintainableObject = maintainable;
            if (referencingObjects != null)
            {
                this.referencingObjects = referencingObjects;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the maintainable.
        /// </summary>
        public virtual IMaintainableObject Maintainable
        {
            get
            {
                return this.maintainableObject;
            }
        }

        /// <summary>
        ///   Gets the referencing structures.
        /// </summary>
        public virtual IList<ICrossReferencingTree> ReferencingStructures
        {
            get
            {
                return new List<ICrossReferencingTree>(this.referencingObjects);
            }
        }

        #endregion
    }
}