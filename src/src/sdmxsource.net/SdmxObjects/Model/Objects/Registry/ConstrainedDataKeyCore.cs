// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstrainedDataKeyBeanImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The constrained data key core.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Registry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Org.Sdmx.Resources.SdmxMl.Schemas.V21.Common;
    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Constants.InterfaceConstant;
    using Org.Sdmxsource.Sdmx.Api.Exception;
    using Org.Sdmxsource.Sdmx.Api.Model.Data;
    using Org.Sdmxsource.Sdmx.Api.Model.Mutable.Registry;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Base;
    using Org.Sdmxsource.Sdmx.Api.Model.Objects.Registry;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data;
    using Org.Sdmxsource.Sdmx.SdmxObjects.Model.Objects.Base;
    using Org.Sdmxsource.Util;

    /// <summary>
    ///   The constrained data key core.
    /// </summary>
    [Serializable]
    public class ConstrainedDataKeyCore : SdmxStructureCore, IConstrainedDataKey
    {
        #region Fields

        /// <summary>
        ///   The _key values.
        /// </summary>
        private readonly IList<IKeyValue> _keyValues;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM MUTABLE OBJECT                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainedDataKeyCore"/> class.
        /// </summary>
        /// <param name="mutable">
        /// The mutable. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        public ConstrainedDataKeyCore(IConstrainedDataKeyMutableObject mutable, IConstraintDataKeySet parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstrainedDataKey), parent)
        {
            this._keyValues = new List<IKeyValue>();

            foreach (IKeyValue each in mutable.KeyValues)
            {
                this._keyValues.Add(new KeyValueImpl(each.Code, each.Concept));
            }

            this.Validate();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////BUILD FROM V2.1 SCHEMA                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstrainedDataKeyCore"/> class.
        /// </summary>
        /// <param name="dataKeyType">
        /// The data key type. 
        /// </param>
        /// <param name="parent">
        /// The parent. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">
        /// Throws Validate exception.
        /// </exception>
        public ConstrainedDataKeyCore(DistinctKeyType dataKeyType, IConstraintDataKeySet parent)
            : base(SdmxStructureType.GetFromEnum(SdmxStructureEnumType.ConstrainedDataKey), parent)
        {
            this._keyValues = new List<IKeyValue>();

            foreach (DinstinctKeyValueType componentValueSet in dataKeyType.GetTypedKeyValue<DinstinctKeyValueType>())
            {
                string concept = componentValueSet.id;
                if (componentValueSet.Value == null || componentValueSet.Value.Count < 1
                    || componentValueSet.Value.Count > 1)
                {
                    throw new SdmxSemmanticException("KeyValue expected to contain a single value");
                }

                string valueren = componentValueSet.Value[0].TypedValue;
                this._keyValues.Add(new KeyValueImpl(valueren, concept));
            }

            this.Validate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////DEEP EQUALS                             //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Public Properties

        /// <summary>
        ///   Gets the key values.
        /// </summary>
        public virtual IList<IKeyValue> KeyValues
        {
            get
            {
                return new List<IKeyValue>(this._keyValues);
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deep equals.
        /// </summary>
        /// <param name="sdmxObject">
        /// The agencySchemeMutable. 
        /// </param>
        /// <param name="includeFinalProperties"> </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool DeepEquals(ISdmxObject sdmxObject, bool includeFinalProperties)
        {
            if (sdmxObject == null) return false;

            if (sdmxObject.StructureType == this.StructureType)
            {
                var that = (IConstrainedDataKey)sdmxObject;
                if (!ObjectUtil.EquivalentCollection(this.KeyValues, that.KeyValues))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////VALIDATE                 //////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="ConstraintDataKeySetCore"/>.
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/> ; otherwise, false. 
        /// </returns>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/> . 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            // Can match another of same type
            if (!(obj is IConstrainedDataKey))
            {
                return false;
            }

            var other = (IConstrainedDataKey)obj;
            if (other.KeyValues.Count != this.KeyValues.Count)
            {
                return false;
            }

            foreach (IKeyValue otherKv in other.KeyValues)
            {
                bool found = false;

                foreach (IKeyValue thisKv in this.KeyValues)
                {
                    if (otherKv.Equals(thisKv))
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The <see cref="int" /> . </returns>
        public override int GetHashCode()
        {
            return this._keyValues != null ? this._keyValues.GetHashCode() : 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="other">
        /// The other. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        protected bool Equals(ConstrainedDataKeyCore other)
        {
            return Equals(this._keyValues, other._keyValues);
        }

        /// <summary>
        ///   The validate.
        /// </summary>
        /// <exception cref="SdmxSemmanticException">Throws Validate exception.</exception>
        private void Validate()
        {
            ISet<string> idSet = new HashSet<string>();
            int count = 0;
            int coundWildCard = 0;

            foreach (IKeyValue kv in this._keyValues)
            {
                if (idSet.Contains(kv.Concept))
                {
                    throw new SdmxSemmanticException(
                        "Constrained data key contains more then one value for dimension id: " + kv.Concept);
                }

                idSet.Add(kv.Concept);
                if (!kv.Code.Equals(ContentConstraintObject.WildcardCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    count++;
                }
                else
                {
                    coundWildCard++;
                }
            }
            if(count == 1 && coundWildCard == 1)
                throw new SdmxSemmanticException("Can not define a datakey set with only one code.  Please use Cube Region instead to mark code for inclusion or exclusion");
        }

        public IKeyValue GetKeyValue(string dimensionId)
        {
            return this.KeyValues.FirstOrDefault(kv => kv.Concept.Equals(dimensionId));
        }

        #endregion
    }
}