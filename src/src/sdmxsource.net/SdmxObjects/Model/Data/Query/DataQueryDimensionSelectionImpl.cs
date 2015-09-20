// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataQueryDimensionSelectionImpl.cs" company="EUROSTAT">
//   TODO
// </copyright>
// <summary>
//   The data query dimension selection impl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Org.Sdmxsource.Sdmx.SdmxObjects.Model.Data.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Org.Sdmxsource.Sdmx.Api.Constants;
    using Org.Sdmxsource.Sdmx.Api.Model.Data.Query;
    using Org.Sdmxsource.Util;
    using Org.Sdmxsource.Sdmx.Api.Exception;

    /// <summary>
    ///   The data query dimension selection impl.
    /// </summary>
    [Serializable]
    public class DataQueryDimensionSelectionImpl : IDataQuerySelection
    {
        #region Fields

        /// <summary>
        ///   The _concept.
        /// </summary>
        private readonly string _concept;

        /// <summary>
        ///   The _values.
        /// </summary>
        private readonly ISet<string> _values;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryDimensionSelectionImpl"/> class.
        /// </summary>
        /// <param name="concept0">
        /// The concept 0. 
        /// </param>
        /// <param name="valueren">
        /// The valueren. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">Throws Model Exception.
        /// </exception>
        public DataQueryDimensionSelectionImpl(string concept0, params string[] valueren)
        {
            this._values = new HashSet<string>();
            if (string.IsNullOrWhiteSpace(concept0))
            {
                throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMissingConcept);
            }

            this._concept = concept0;
            if (valueren == null || valueren.Length == 0)
            {
                throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMissingConceptValue);
            }

            foreach (string currentValue in valueren)
            {
                this._values.Add(currentValue);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataQueryDimensionSelectionImpl"/> class.
        /// </summary>
        /// <param name="concept0">
        /// The concept 0. 
        /// </param>
        /// <param name="values1">
        /// The values 1. 
        /// </param>
        /// <exception cref="SdmxSemmanticException">Throws Model Exception.
        /// </exception>
        public DataQueryDimensionSelectionImpl(string concept0, ISet<string> values1)
        {
            this._values = new HashSet<string>();
            if (string.IsNullOrWhiteSpace(concept0))
            {
                throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMissingConcept);
            }

            this._concept = concept0;
            if (!ObjectUtil.ValidCollection(values1))
            {
                throw new SdmxSemmanticException(ExceptionCode.QuerySelectionMissingConceptValue);
            }

            this._values = new HashSet<string>(values1);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the dimension id.
        /// </summary>
        public virtual string ComponentId
        {
            get
            {
                return this._concept;
            }
        }

        /// <summary>
        ///   Gets the value.
        /// </summary>
        /// ///
        /// <exception cref="ArgumentException">Throws ArgumentException.</exception>
        public virtual string Value
        {
            get
            {
                if (this._values.Count > 1)
                {
                    throw new ArgumentException("More then one value exists for this selection");
                }

                return this._values.ToArray()[0];
            }
        }

        /// <summary>
        ///   Gets the values.
        /// </summary>
        public virtual ISet<string> Values
        {
            get
            {
                return this._values;
            }
        }

        public virtual bool HasMultipleValues
        {
            get
            {
                return this._values.Count > 1;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add value.
        /// </summary>
        /// <param name="valueren">
        /// The valueren. 
        /// </param>
        public void AddValue(string valueren)
        {
            this._values.Add(valueren);
        }

        /// <summary>
        /// The equals.
        /// </summary>
        /// <param name="obj">
        /// The obj. 
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> . 
        /// </returns>
        public override bool Equals(object obj)
        {
            var that = obj as IDataQuerySelection;
            if (that != null)
            {
                if (this.ComponentId.Equals(that.ComponentId))
                {
                    return this.Values.SequenceEqual(that.Values);
                }
            }

            return false;
        }

        /// <summary>
        ///   The get hash code.
        /// </summary>
        /// <returns> The <see cref="int" /> . </returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }


        /// <summary>
        ///   The to string.
        /// </summary>
        /// <returns> The <see cref="string" /> . </returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(string.Empty);
            sb.Append(this._concept);
            sb.Append(string.Empty);
            sb.Append(" : ");
            string concat = string.Empty;
            foreach (string currentValue in this._values)
            {
                sb.Append(concat);
                sb.Append(currentValue);
                concat = ",";
            }

            return sb.ToString();
        }

        #endregion
    }
}