// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstrainedDataKeyMutableBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constrained data key mutable core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Mutable.Base;

    /// <summary>
    ///   The constrained data key mutable core.
    /// </summary>
    [Serializable]
    public class ConstrainedDataKeyMutableCore : MutableCore, IConstrainedDataKeyMutableObject
    {
        #region Fields

        /// <summary>
        ///   The key values.
        /// </summary>
        private IList<IKeyValue> keyValues;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConstrainedDataKeyMutableCore" /> class.
        /// </summary>
        public ConstrainedDataKeyMutableCore()
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstrainedDataKey))
        {
            this.keyValues = new List<IKeyValue>();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM IMMUTABLE OBJECT                 //////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainedDataKeyMutableCore"/> class.
        /// </summary>
        /// <param name="immutable">
        /// The immutable. 
        /// </param>
        public ConstrainedDataKeyMutableCore(IConstrainedDataKey immutable)
            : base(immutable)
        {
            this.keyValues = new List<IKeyValue>();

            foreach (IKeyValue each in immutable.KeyValues)
            {
                this.keyValues.Add(new KeyValueImpl(each.Code, each.Concept));
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the key values.
        /// </summary>
        public virtual IList<IKeyValue> KeyValues
        {
            get
            {
                return new ReadOnlyCollection<IKeyValue>(this.keyValues);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add key value.
        /// </summary>
        /// <param name="keyvalue">
        /// The keyvalue. 
        /// </param>
        public virtual void AddKeyValue(IKeyValue keyvalue)
        {
            if (keyvalue != null)
            {
                this.keyValues.Add(keyvalue);
            }
        }

        #endregion
    }
}