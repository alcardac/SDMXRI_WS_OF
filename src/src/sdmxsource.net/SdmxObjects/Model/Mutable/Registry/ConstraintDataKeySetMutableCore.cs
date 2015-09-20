// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstraintDataKeySetMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constraint data key set mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The constraint data key set mutable core.
    /// </summary>
    [Serializable]
    public class ConstraintDataKeySetMutableCore : MutableCore, IConstraintDataKeySetMutableObject
    {
        #region Fields

        /// <summary>
        ///   The constrained keys.
        /// </summary>
        private IList<IConstrainedDataKeyMutableObject> constrainedKeys;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConstraintDataKeySetMutableCore" /> class.
        /// </summary>
        public ConstraintDataKeySetMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstrainedDataKeyset))
        {
            this.constrainedKeys = new List<IConstrainedDataKeyMutableObject>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstraintDataKeySetMutableCore"/> class.
        /// </summary>
        /// <param name="immutable">
        /// The immutable. 
        /// </param>
        public ConstraintDataKeySetMutableCore(IConstraintDataKeySet immutable)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstrainedDataKeyset))
        {
            this.constrainedKeys = new List<IConstrainedDataKeyMutableObject>();

            foreach (IConstrainedDataKey each in immutable.ConstrainedDataKeys)
            {
                this.constrainedKeys.Add(new ConstrainedDataKeyMutableCore(each));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the constrained data keys.
        /// </summary>
        public virtual IList<IConstrainedDataKeyMutableObject> ConstrainedDataKeys
        {
            get
            {
                return this.constrainedKeys;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add constrained data key.
        /// </summary>
        /// <param name="key">
        /// The key. 
        /// </param>
        public virtual void AddConstrainedDataKey(IConstrainedDataKeyMutableObject key)
        {
            if (key == null)
            {
                this.constrainedKeys = new List<IConstrainedDataKeyMutableObject>();
            }

            this.constrainedKeys.Add(key);
        }

        #endregion
    }
}